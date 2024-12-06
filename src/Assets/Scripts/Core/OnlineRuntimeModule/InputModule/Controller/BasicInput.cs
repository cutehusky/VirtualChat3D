using Core.InputModule.Component.Sensor;
using Core.OnlineRuntimeModule.InputModule.Controller.Touch;
using Core.OnlineRuntimeModule.InputModule.Model;
using Plugins.EditorExtend.ExecutionOrder.Scripts;
using QFramework;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;
using IController = QFramework.IController;

namespace Core.OnlineRuntimeModule.InputModule.Controller
{
    /// <summary>
    /// Map unity player input to event trigger
    /// </summary>
    [ExecuteAfter(typeof(GyroscopeInput))]
    [ExecuteAfter(typeof(OnScreenButton))]
    [ExecuteAfter(typeof(OnScreenTouchpad))]
    [ExecuteAfter(typeof(OnScreenJoystick))]
    [RequireComponent(typeof(PlayerInput))]
    public class BasicInput : MonoSingleton<BasicInput>, IController
    {
        private PlayerInput _playerInput;
        
        public bool IsCurrentDeviceMouse => _playerInput.currentControlScheme == "Keyboard&Mouse";

        protected override void Awake()
        {
            base.Awake();
            _playerInput = GetComponent<PlayerInput>();
        }
        
        private void OnApplicationFocus(bool hasFocus)
        {
           // SetCursorState(hasFocus);
        }

        public void SetCursorState(bool isLock)
        {
            this.GetModel<PlayerInputAction>().CursorLocked = isLock;
            Cursor.lockState = isLock ? CursorLockMode.Locked : CursorLockMode.None;
        }

        public void Look(InputAction.CallbackContext context)
        {
            this.GetModel<PlayerInputAction>().TriggerEvent("Look", context.ReadValue<Vector2>());
        }

        public void Move(InputAction.CallbackContext context)
        {
            this.GetModel<PlayerInputAction>().TriggerEvent("Move", context.ReadValue<Vector2>());
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (!context.ReadValueAsButton())
                this.GetModel<PlayerInputAction>().TriggerEvent("Jump");
        }

        public void Crouch(InputAction.CallbackContext context)
        {
            if (!context.ReadValueAsButton())
                this.GetModel<PlayerInputAction>().TriggerEvent("Crouch");
        }

        public void Sprint(InputAction.CallbackContext context)
        {
            this.GetModel<PlayerInputAction>().TriggerEvent("Sprint", context.ReadValueAsButton());
        }

        public IArchitecture GetArchitecture()
        {
            return CoreArchitecture.Interface;
        }
    }
}
