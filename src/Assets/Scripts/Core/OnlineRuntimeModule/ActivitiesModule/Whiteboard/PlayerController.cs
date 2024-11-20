using QFramework;
using Unity.Netcode;

namespace Core.OnlineRuntimeModule.ActivitiesModule.Whiteboard
{
    public class PlayerController: NetworkBehaviour, IController
    {
        public void Draw()
        {
            
        }

        public IArchitecture GetArchitecture()
        {
            return CoreArchitecture.Interface;
        }
    }
}