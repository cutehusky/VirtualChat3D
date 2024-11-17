using QFramework;
using Unity.Netcode;

namespace Core.OnlineRuntimeModule.CharacterControlModule.Controller
{
    public class PlayerController: NetworkBehaviour, IController
    {
        public void Jump()
        {
            
        }

        public void SetCrouch(bool isCrouch)
        {
            
        }

        public void SetSprint(bool isSprint)
        {
            
        }

        public void MoveAndRotate()
        {
            
        }

        public void PlayAnimation(EAnimation animationName)
        {
            
        }
        
        public IArchitecture GetArchitecture()
        {
            return CoreArchitecture.Interface;
        }
    }
}