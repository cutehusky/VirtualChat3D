using Core.CharacterCustomizationModule.Model;
using Core.OnlineRuntimeModule.InputModule.Model;
using QFramework;
using Unity.Cinemachine;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using Utilities.AvgPool;
using Utilities.Netcode;
using Random = UnityEngine.Random;

namespace Core.OnlineRuntimeModule.CharacterControl
{
    public class PlayerController: NetworkBehaviour, IController
    {
        [SerializeField] private Transform lookPoint;
        private readonly NetworkVariable<FixedString512Bytes> _modelName = new("",
            NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        private Animator _animator;
        private Transform _actorTransform;
        private CharacterController _controller;
        
        [Header(("Move"))] 
        [SerializeField] public float speedMultiply = 1f;
        [SerializeField] protected float moveSpeed = 2.5f;
        [SerializeField] protected float crouchSpeed = 2.5f;
        [SerializeField] protected float sprintSpeed = 4f;
        [SerializeField] protected float speedDamp = 0.1f;
        [SerializeField] public float slopeMultiply = 1.0f; 
        
        [Header("Rotation")]
        [SerializeField] protected float rotationSmoothTime = 0.1f;
        
        [Header("Jump and Gravity")] 
        [SerializeField] private float fallThreshold = 0.75f;
        [SerializeField] public float jumpHeight = 1f;
        [SerializeField] public float sprintJumpHeight = 1.25f;
        [SerializeField] protected float gravity = -9.81f;
        [SerializeField] protected float fallMultiply = 1.5f;
        [SerializeField] protected float terminalVelocity = 1000.0f;
        [SerializeField] protected float jumpTimeout = 0.1f;
        [SerializeField] protected float freeFallTimeout = 0.1f;

        [Header("Ground check")]
        [SerializeField] protected float groundCheckOffset = 0.5f;
        [SerializeField] protected LayerMask groundCheckMask;
        
        [Header("Camera")]
        public Transform mainCamera;
        [SerializeField] private float topClamp = 60.0f;
        [SerializeField] private float bottomClamp = -40.0f;
        [SerializeField] private float cameraPitchOverride;
        [SerializeField] private CinemachineVirtualCameraBase _cameraControl;

        private float _cameraYaw;
        private float _cameraPitch;
        
        private int _animMotionScaleID;
        private int _animYSpeedID;
        private int _animMotionMultipleID;
        private int _animMoveSpeedID;
        private int _animGroundedID;
        private int _animFreeFallID;
        private int _animCrouchID;
        private int _animAimID;
        
        private Vector2 _moveDir;

        private bool _isJumping;
        private bool _isGrounded;
        private bool _isMidAir;
        private bool _isSprint;
        
        private float _jumpTimeoutDelta;
        private float _freeFallTimeoutDelta;
        private readonly Vector3AvgPool _moveAvg = new(5);
        private float _verticalVelocity;
        
        private float _rotationSpeed;
        private float _rotationTarget;
        private const float Threshold = 0.01f;
        
        public void Awake()
        {
            _actorTransform = transform;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            var modelLoader = this.GetModel<CharacterModelDataModel>();
            if (IsOwner) {
                modelLoader.CreateRoomCharacter(((vrm, modelName) =>
                {
                    _modelName.Value = modelName;
                    SetModelServerRpc(modelName);
                    vrm.transform.SetParent(transform, false);
                    GetComponent<ClientAnimator>().Animator.avatar = vrm.GetComponent<Animator>().avatar;
                    vrm.GetComponent<Animator>().enabled = false;
                    _animator = GetComponent<ClientAnimator>().Animator;
                    transform.position = new Vector3(Random.value * 5, 0, 0);
                    AssignAnimationIDs();
                }));
                _controller = GetComponent<CharacterController>();
            }
        }
        
        [ClientRpc]
        public void SetModelClientRpc(string modelName, ClientRpcParams clientRpcParams = default)
        {
            if (!IsOwner)
            {
                Debug.Log("load model of client");
                var modelLoader = this.GetModel<CharacterModelDataModel>();
                modelLoader.CreateCharacter(modelName, (vrm) =>
                {
                    vrm.transform.SetParent(transform, false);
                    GetComponent<ClientAnimator>().Animator.avatar = vrm.GetComponent<Animator>().avatar;
                    vrm.GetComponent<Animator>().enabled = false;
                });
            }
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void SetModelServerRpc(string modelName)
        {
            SetModelClientRpc(modelName);
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void SetModelServerRpc(string modelName, ulong uid)
        {
            var clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new[]{uid}
                }
            };
            SetModelClientRpc(modelName, clientRpcParams);
        }
       
        public void Start()
        {
            if (IsOwner)
            {
                InputRegister();
                mainCamera = Camera.main.transform;
                _cameraControl.Priority.Value = 100;
                return;
            }
            _cameraControl.Priority.Value = 0;
            if (_modelName.Value != "")
                SetModelServerRpc(_modelName.Value.ToString(), NetworkManager.Singleton.LocalClientId);
        }

        public void Look(Vector2 lookDir)
        {
            _cameraYaw += lookDir.x;
            _cameraPitch += lookDir.y; 
        }

        public void Move(Vector2 moveDir)
        {
            _moveDir = moveDir;
        }
        
        protected virtual void OnAnimatorMove()
        {
            if (_animator != null)
                if (!_isJumping && !_isMidAir)
                {
                    _moveAvg.Add(_animator.velocity);
                    var moveDir = _animator.deltaPosition;
                    moveDir.y = _verticalVelocity * Time.deltaTime;
                    MoveActor(moveDir);
                } else {
                    var moveDir = _moveAvg.Avg;
                    moveDir.y = _verticalVelocity;
                    _moveAvg.Add(moveDir);
                    MoveActor(moveDir * Time.deltaTime);
                }
        }
        
        private void GroundCheck()
        {
            var radius = _controller.radius;
            var position = _actorTransform.position;
            _isGrounded = Physics.SphereCast(position + groundCheckOffset * Vector3.up,
                radius, Vector3.down, out _
                , groundCheckOffset - radius + 2 * _controller.skinWidth
                , groundCheckMask);
            if (!_isGrounded && !_isJumping && !_isMidAir)
                _isGrounded = Physics.SphereCast(position + groundCheckOffset * Vector3.up,
                    radius, Vector3.down, out _
                    , groundCheckOffset - radius 
                      + 2 * _controller.skinWidth + fallThreshold
                    , groundCheckMask);
            _animator.SetBool(_animGroundedID, _isGrounded);
        }

        private void SetGravity(float v = -2)
        {
            _verticalVelocity = v;
            _animator.SetFloat(_animYSpeedID, _verticalVelocity
                , speedDamp, Time.deltaTime);
        }
        
        private void CalculateGravity()
        {
            if (Mathf.Abs(_verticalVelocity) < terminalVelocity)
                _verticalVelocity += (_verticalVelocity < 0
                    ? gravity * fallMultiply * Time.deltaTime
                    : gravity * Time.deltaTime);
            _animator.SetFloat(_animYSpeedID, _verticalVelocity
                , speedDamp, Time.deltaTime);
        }

        private void AssignAnimationIDs()
        {
            // float
            _animMotionScaleID = Animator.StringToHash("MotionScale");
            _animYSpeedID = Animator.StringToHash("YSpeed");
            _animMotionMultipleID = Animator.StringToHash("MotionMultiple");
            _animMoveSpeedID = Animator.StringToHash("MoveSpeed");
            // bool
            _animFreeFallID = Animator.StringToHash("FreeFalling");
            _animGroundedID = Animator.StringToHash("Grounded");
            _animCrouchID = Animator.StringToHash("isCrouching");

            // initial value
            _animator.SetFloat(_animMotionMultipleID, (1 / _animator.humanScale) * speedMultiply);
            _animator.SetFloat(_animMoveSpeedID, 0);
            _animator.SetFloat(_animMotionScaleID, 1 / _animator.humanScale);
           _animator.SetFloat(_animYSpeedID, 0);
           _animator.SetBool(_animGroundedID, true);
           _animator.SetBool(_animFreeFallID, false);
           _animator.SetBool(_animCrouchID, false);
        }
        
        private void MoveActor(Vector3 moveDir)
        {
            if (!Physics.Raycast(_actorTransform.position, Vector3.down,
                    out var hit, _controller.stepOffset, groundCheckMask))
            {
                _controller.Move(moveDir);
                return;
            }
            var xz = new Vector3(moveDir.x, 0, moveDir.z);
            var newMoveDir = Vector3.ProjectOnPlane(xz
                , hit.normal).normalized * xz.magnitude;
            newMoveDir.y += moveDir.y;
            if (Vector3.Angle(hit.normal, Vector3.up) > _controller.slopeLimit)
                newMoveDir += Vector3.ProjectOnPlane(Vector3.up, hit.normal)
                              * Time.deltaTime * gravity * slopeMultiply;
            _controller.Move(newMoveDir);
        }


        private void InputRegister()
        {
            this.GetModel<PlayerInputAction>().GetVector2Event("Look").Register(Look);
            this.GetModel<PlayerInputAction>().GetVector2Event("Move").Register(Move);
            this.GetModel<PlayerInputAction>().GetBoolEvent("Sprint").Register(SetSprint);
            this.GetModel<PlayerInputAction>().GetTrigger("Jump").Register(Jump);
        }
        

        private float _jumpHeight = 0;
        private void Jump()
        {
            if (!_isGrounded || _isJumping)
                return;
            if (_jumpTimeoutDelta > 0)
                return;
            var curSpeed = _moveAvg.Avg.magnitude;
            _jumpHeight = Mathf.Lerp(jumpHeight, sprintJumpHeight
                , (Mathf.Clamp(curSpeed + Threshold, moveSpeed
                      , sprintSpeed) - moveSpeed)
                  / (sprintSpeed - moveSpeed));
            _isJumping = true;
        }
        
        private void SetSprint(bool isSprint)
        {
            _isSprint = isSprint;
        }
        
        protected void Timeout()
        {
            if (_jumpTimeoutDelta > 0)
                _jumpTimeoutDelta -= Time.deltaTime;
            if (_freeFallTimeoutDelta > 0 && !_isGrounded)
                _freeFallTimeoutDelta -= Time.deltaTime;
        }

        private void OnEnable()
        {
            var rotation = lookPoint.rotation;
            _cameraPitch = rotation.eulerAngles.x;
            _cameraYaw = rotation.eulerAngles.y;
        }
        
        public void Update()
        {
            if (_animator == null || !IsOwner)
                return;
            GroundCheck();
            Timeout();
            
            _cameraYaw = ClampAngle(_cameraYaw, float.MinValue , float.MaxValue);
            _cameraPitch = ClampAngle( _cameraPitch, bottomClamp - cameraPitchOverride,
                topClamp - cameraPitchOverride);
            lookPoint.rotation = Quaternion.Euler(_cameraPitch + cameraPitchOverride, 
                _cameraYaw,0);
            
            if (_isJumping)
            {
                SetGravity(Mathf.Sqrt(_jumpHeight * -2f * gravity));
                MoveAndRotate(Vector2.zero, 0);
                if (!_isGrounded)
                {
                    _isJumping = false;
                    _isMidAir = true;
                    _animator.SetBool(_animFreeFallID, true);
                    _jumpTimeoutDelta = jumpTimeout;
                }
            } else if (_isMidAir)
            {
                CalculateGravity();
                MoveAndRotate(Vector2.zero, 0);
                if (_isGrounded)
                {
                    _freeFallTimeoutDelta = freeFallTimeout;
                    _isMidAir = false;
                    _animator.SetBool(_animFreeFallID, false);
                }
            } else
            {
                SetGravity();
                MoveAndRotate(_moveDir, _isSprint ? 
                    sprintSpeed : moveSpeed);
                if (!_isGrounded && _freeFallTimeoutDelta <= 0)
                {
                    _isMidAir = true;
                    _animator.SetBool(_animFreeFallID, true);
                }
            }
        }

        protected virtual void MoveAndRotate(Vector2 moveDir, float speed)
        {
            _animator.SetFloat(_animMoveSpeedID, moveDir.magnitude * speed,
                speedDamp, Time.deltaTime);
            _animator.SetFloat(_animMotionMultipleID,
                (1 / _animator.humanScale) * speedMultiply * Mathf.Min(moveDir.magnitude,1),
                speedDamp, Time.deltaTime);
            if (moveDir.sqrMagnitude * speed > Threshold)
                _rotationTarget = Mathf.Atan2(moveDir.x, moveDir.y) * Mathf.Rad2Deg 
                                 + mainCamera.eulerAngles.y;
            SetRotation(Quaternion.Euler(0.0f,
                Mathf.SmoothDampAngle(_actorTransform.eulerAngles.y, _rotationTarget
                    , ref _rotationSpeed, rotationSmoothTime), 0.0f));
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f)
                lfAngle += 360f;
            if (lfAngle > 360f)
                lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
        
        private void SetRotation(Quaternion rotation)
        {
            lookPoint.SetParent(null);
            _actorTransform.rotation = rotation;
            lookPoint.SetParent(_actorTransform);
        }

        public IArchitecture GetArchitecture()
        {
            return CoreArchitecture.Interface;
        }
    }
}