using System;
using System.Collections.Generic;
using System.Globalization;
using Core.MessageModule.Model;
using Core.MessageModule.View;
using Core.MVC;
using Core.NetworkModule.Controller;
using Core.UserAccountModule.Model;
using QFramework;
using UnityEngine;

namespace Core.MessageModule.Controller
{
    public class MessageController: ControllerBase
    {
        private MessageView _messageView;

        public void SendMessage()
        {
            var text = _messageView.chatInput.Text;
            if (text.Length <= 0)
                return;
            _messageView.chatInput.Text = "";
#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IOS)
            _messageView.TMP_chatInput.text = "";
#endif
            
            SocketIO.Instance.SendWebSocketMessage("fetch", new UserVerifyPacket()
            {
                uid = this.GetModel<UserProfileDataModel>().UserProfileData.UserId,
                token = ""
            });  // for test only
            
            SocketIO.Instance.SendWebSocketMessage("sendMessage", new MessagePacket
            {
                fid = this.GetModel<MessageDataModel>().CurrentChatSession.FriendId,
                id_cons = this.GetModel<MessageDataModel>().CurrentChatSession.ChatSessionId,
                msg = text,
                uid = this.GetModel<UserProfileDataModel>().UserProfileData.UserId
            });
        }

        private void AddMessage(string userId, string content, string time, string chatSessionId)
        {
            if (this.GetModel<MessageDataModel>().CurrentChatSession.ChatSessionId == chatSessionId)
            {
                this.GetModel<MessageDataModel>().CurrentChatSession.ChatData.Add(new ChatMessage()
                {
                    UserId = userId,
                    Content = content,
                    Time = time
                });
                _messageView.RefreshList();
            }
        }
            
        /// <summary>
        /// Load all message from server
        /// </summary>
        public void LoadMessage(string chatSessionId, string friendId)
        {
            this.GetModel<MessageDataModel>().CurrentChatSession.ChatSessionId = chatSessionId;
            this.GetModel<MessageDataModel>().CurrentChatSession.FriendId = friendId;
            SocketIO.Instance.SendWebSocketMessage("viewMessage",  new ChatSessionPacket()
            {
                uid = this.GetModel<UserProfileDataModel>().UserProfileData.UserId,
                id_cons = chatSessionId
            });
        }

        public void ReadMessage(string chatSessionId)
        {
            if (this.GetModel<MessageDataModel>().CurrentChatSession.ChatSessionId == chatSessionId)
            {
                SocketIO.Instance.SendWebSocketMessage("readMessage", new ChatSessionPacket()
                {
                    uid = this.GetModel<UserProfileDataModel>().UserProfileData.UserId,
                    id_cons = chatSessionId
                });
            }
        }
        
        public override void OnInit(List<ViewBase> view)
        {
            base.OnInit(view);
            _messageView = view[0] as MessageView;
            _messageView.send.onClick.AddListener((SendMessage));
            _messageView.back.onClick.AddListener(() =>
            {
                AppMain.Instance.OpenFriendListView();
            });
            SocketIO.Instance.AddUnityCallback("viewMessageReply", (res) =>
            { 
                string id_cons = "";
                Debug.Log("test");
                var packets = res.GetValue<MessagePacket[]>();
                foreach (var packet in packets)
                {
                    AddMessage(packet.uid,
                        packet.msg,
                        DateTimeOffset.FromUnixTimeMilliseconds(packet.timestamp).DateTime
                            .ToString(CultureInfo.InvariantCulture), packet.id_cons);
                    id_cons = packet.id_cons;
                }
                if (id_cons != "")
                    ReadMessage(id_cons);
            });
            SocketIO.Instance.AddUnityCallback("receivedMessage",(res) =>
            {
                var packet = res.GetValue<MessagePacket>();
                Debug.Log($"Receive packet {packet.fid} {packet.uid} {packet.id_cons} {packet.msg}");
                AddMessage(packet.uid,
                    packet.msg,
                    DateTimeOffset.FromUnixTimeMilliseconds(packet.timestamp).DateTime
                        .ToString(CultureInfo.InvariantCulture), packet.id_cons);
                ReadMessage(packet.id_cons);
            });
            SocketIO.Instance.AddUnityCallback("sendMessageReply",(res) =>
            {
                var packet = res.GetValue<MessagePacket>();
                Debug.Log($"Receive packet {packet.fid} {packet.uid} {packet.id_cons} {packet.msg}");
                AddMessage(packet.uid,
                    packet.msg,
                    DateTimeOffset.FromUnixTimeMilliseconds(packet.timestamp).DateTime
                        .ToString(CultureInfo.InvariantCulture), packet.id_cons);
            });
        }

        public ViewBase OpenMessageView(string chatSessionId, string friendId)
        {
            AppMain.Instance.CloseCurrentView();
            LoadMessage(chatSessionId, friendId);
            _messageView.Display();
            _messageView.Render(this.GetModel<MessageDataModel>());
            return _messageView;
        }
    }
}