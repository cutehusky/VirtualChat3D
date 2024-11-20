using System.Collections.Generic;
using Core.MVC;
using QFramework;
using Unity.Netcode;

namespace Core.OnlineRuntimeModule.MessageModule.Controller
{
    public class MessageController: NetworkBehaviour, IController
    {
        private MessageView _messageView;
        public void OnInit(MessageView view)
        {
            
        }

        [ServerRpc(RequireOwnership = false)]
        public void SendMessageServerRpc(string username, string message)
        {
            
        }

        [ClientRpc]
        public void SendMessageClientRpc(string username, string message)
        {
            
        }
        
        public IArchitecture GetArchitecture()
        {
            return CoreArchitecture.Interface;
        }
    }
}