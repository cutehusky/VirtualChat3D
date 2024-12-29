using System;
using Core.OnlineRuntimeModule.InputModule.Model;
using QFramework;
using Unity.Netcode;
using UniVRM10;

namespace Core.OnlineRuntimeModule.CharacterControl
{
    public class PlayerExpressionController : NetworkBehaviour, IController
    {
        private PlayerController _playerController;

        public void Start()
        {
            if (IsOwner)
                InputRegister();
            _playerController = GetComponent<PlayerController>();
        }

        public void InputRegister()
        {
            this.GetModel<PlayerInputAction>().GetTrigger("Neutral").Register(() =>
            {
                SetExpressionServerRpc(0);
            });
            this.GetModel<PlayerInputAction>().GetTrigger("Fun").Register(() =>
            {
                SetExpressionServerRpc(1);
            });
            this.GetModel<PlayerInputAction>().GetTrigger("Sad").Register(() =>
            {
                SetExpressionServerRpc(2);
            });
            this.GetModel<PlayerInputAction>().GetTrigger("Surprised").Register(() =>
            {
                SetExpressionServerRpc(3);
            });
            this.GetModel<PlayerInputAction>().GetTrigger("Angry").Register(() =>
            {
                SetExpressionServerRpc(4);
            });
            this.GetModel<PlayerInputAction>().GetTrigger("Relax").Register(() =>
            {
                SetExpressionServerRpc(5);
            });
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetExpressionServerRpc(int v)
        {
            SetExpressionClientRpc(v);
        }

        [ClientRpc]
        public void SetExpressionClientRpc(int v)
        {
            switch (v)
            {
                case 0:
                    Neutral();
                    break;
                case 1:
                    Fun();
                    break;
                case 2:
                    Sad();
                    break;
                case 3:
                    Surprised();
                    break;
                case 4:
                    Angry();
                    break;
                case 5:
                    Relax();
                    break;
            }
        }

        public void Relax()
        {
            if (!_playerController || !_playerController.vrm10Instance)
                return;
            var vrm10Instance = _playerController.vrm10Instance;
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Relaxed, 1);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Angry, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Surprised, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Sad, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Happy, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Neutral, 0);
        }

        public void Angry()
        {
            if (!_playerController || !_playerController.vrm10Instance)
                return;
            var vrm10Instance = _playerController.vrm10Instance;
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Relaxed, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Angry, 1);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Surprised, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Sad, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Happy, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Neutral, 0);
        }

        public void Surprised()
        {
            if (!_playerController || !_playerController.vrm10Instance)
                return;
            var vrm10Instance = _playerController.vrm10Instance;
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Relaxed, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Angry, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Surprised, 1);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Sad, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Happy, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Neutral, 0);
        }

        public void Sad()
        {
            if (!_playerController || !_playerController.vrm10Instance)
                return;
            var vrm10Instance = _playerController.vrm10Instance;
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Relaxed, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Angry, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Surprised, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Sad, 1);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Happy, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Neutral, 0);
        }

        public void Fun()
        {
            if (!_playerController || !_playerController.vrm10Instance)
                return;
            var vrm10Instance = _playerController.vrm10Instance;
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Relaxed, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Angry, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Surprised, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Sad, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Happy, 1);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Neutral, 0);
        }

        public void Neutral()
        {
            if (!_playerController || !_playerController.vrm10Instance)
                return;
            var vrm10Instance = _playerController.vrm10Instance;
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Relaxed, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Angry, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Surprised, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Sad, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Happy, 0);
            vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Neutral, 1);
        }

        public IArchitecture GetArchitecture()
        {
            return CoreArchitecture.Interface;
        }
    }
}