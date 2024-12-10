using System.Collections.Generic;
using Core.MessageModule.Model;
using Core.MVC;
using Core.UserAccountModule.Model;
using QFramework;
using Unity.Netcode;

namespace Core.OnlineRuntimeModule.MessageModule.Controller
{
    public class MessageController: NetworkBehaviour, IController
    {
        private MessageView _messageView;
        private ChatSession _chatSession;

        public void OnInit(MessageView view)
        {
            _messageView = view;

            _messageView.send.onClick.AddListener(() =>
            {
                SendMessageServerRpc(this.GetModel<UserProfileDataModel>().UserProfileData.UserId, _messageView.chatInput.Text);
            });
        }

        public void OpenMessageView()
        {
            _messageView.Display();
            _messageView.Render(_chatSession, this.GetModel<UserProfileDataModel>().UserProfileData.UserId);
        }

        [ServerRpc(RequireOwnership = false)]
        public void SendMessageServerRpc(string userID, string message)
        {
            SendMessageClientRpc(userID, message);
        }

        [ClientRpc]
        public void SendMessageClientRpc(string userID, string message)
        {
            _chatSession.ChatData.Add(new ChatMessage()
            {
                Content = message,
                UserId = userID
            });
            _messageView.RefreshList();
        }
        
        public IArchitecture GetArchitecture()
        {
            return CoreArchitecture.Interface;
        }
    }
}