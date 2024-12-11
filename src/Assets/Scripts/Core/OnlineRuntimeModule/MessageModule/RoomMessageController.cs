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

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsOwner)
            {
                this.GetModel<RoomMessageDataModel>().ChatSession.ChatData.Clear();
                OnInit(AppMain.Instance.roomMessageView);
                Debug.Log("test");
            }
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
            this.GetModel<PlayerInputAction>().GetTrigger("OpenChatView").Register(() =>
            {
                _roomMessageView.Display();
                _roomMessageView.Render(
                    this.GetModel<RoomMessageDataModel>().ChatSession,
                    this.GetModel<UserProfileDataModel>().UserProfileData.UserId);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.GetModel<PlayerInputAction>().GetTrigger("CloseChatView").Register(() =>
            {
                _roomMessageView.Hide();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.GetModel<PlayerInputAction>().GetTrigger("RefreshChatView").Register(() =>
            {
                _roomMessageView.RefreshList();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        [ServerRpc(RequireOwnership = false)]
        public void SendMessageServerRpc(string userID, string message)
        {
            SendMessageClientRpc(userID, message);
        }

        [ClientRpc]
        public void SendMessageClientRpc(string userID, string message)
        {
            this.GetModel<RoomMessageDataModel>().ChatSession.ChatData.Add(new ChatMessage()
            {
                Content = message,
                UserId = userID
            });
            this.GetModel<PlayerInputAction>().TriggerEvent("RefreshChatView");
        }
        
        public IArchitecture GetArchitecture()
        {
            return CoreArchitecture.Interface;
        }
    }
}