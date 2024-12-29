using System;
using Core.OnlineRuntimeModule.InputModule.Model;
using QFramework;
using Unity.Netcode;

namespace Core.OnlineRuntimeModule.CharacterControl
{
    public class PlayerAnimationController: NetworkBehaviour, IController
    {
        private PlayerController _playerController;
        public void Start()
        {
            if (IsOwner)
            {
                _playerController = GetComponent<PlayerController>();
                InputRegister();
            }
        }
        
        public void InputRegister()
        {
            this.GetModel<PlayerInputAction>().GetTrigger("Applause").Register(() =>
            {
                if (_playerController && _playerController.animator)
                    _playerController.animator.SetTrigger("Applause");
            });
            this.GetModel<PlayerInputAction>().GetTrigger("Waves").Register(() =>
            {
                if (_playerController && _playerController.animator)
                    _playerController.animator.SetTrigger("Waves");
            });
            this.GetModel<PlayerInputAction>().GetTrigger("Crying").Register(() =>
            {
                if (_playerController && _playerController.animator)
                    _playerController.animator.SetTrigger("Crying");
            });
            this.GetModel<PlayerInputAction>().GetTrigger("Laughing").Register(() =>
            {
                if (_playerController && _playerController.animator)
                    _playerController.animator.SetTrigger("Laughing");
            });
        }

        public IArchitecture GetArchitecture()
        {
            return CoreArchitecture.Interface;
        }
    }
}