using Core.MessageModule.Model;
using Core.MVC;
using Core.OnlineRuntimeModule.InputModule.Model;
using Core.UserAccountModule.Model;
using QFramework;
using Unity.Netcode;
using UnityEngine;

namespace Core.OnlineRuntimeModule.MessageModule
{
    public class RoomMessageController: NetworkBehaviour, IController
    {
        private RoomMessageView _roomMessageView;
        private ChatSession _chatSession;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _chatSession = new();
            OnInit(AppMain.Instance.roomMessageView);
        }

        public void OnInit(RoomMessageView view)
        {
            _roomMessageView = view; 
            _roomMessageView.send.onClick.RemoveAllListeners();
            _roomMessageView.send.onClick.AddListener(() =>
            {
                var text = _roomMessageView.chatInput.Text;
                _roomMessageView.chatInput.Text = "";
#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IOS)
                _roomMessageView.TMP_chatInput.text = "";
#endif
                SendMessageServerRpc(this.GetModel<UserProfileDataModel>().UserProfileData.UserId, text);
                
            });
            this.GetModel<PlayerInputAction>().GetTrigger("OpenChatView").Register(OpenMessageView).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.GetModel<PlayerInputAction>().GetTrigger("CloseChatView").Register(() =>
            {
                _roomMessageView.Hide();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        public void OpenMessageView()
        {
            _roomMessageView.Display();
            _roomMessageView.Render(_chatSession, this.GetModel<UserProfileDataModel>().UserProfileData.UserId);
        }

        [ServerRpc(RequireOwnership = false)]
        public void SendMessageServerRpc(string userID, string message)
        {
            SendMessageClientRpc(userID, message);
        }

        [ClientRpc]
        public void SendMessageClientRpc(string userID, string message)
        {
            if (!IsClient)
                return;
            _chatSession.ChatData.Add(new ChatMessage()
            {
                Content = message,
                UserId = userID
            });
            _roomMessageView.RefreshList();
        }
        
        public IArchitecture GetArchitecture()
        {
            return CoreArchitecture.Interface;
        }
    }
}