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
        /// <summary>
        /// Mark all new message as read
        /// </summary>
        /// <param name="userId"></param>
        public void ReadMessageOfUser()
        {
            
        }

        public void SendMessage()
        {
            SocketIO.Instance.SendWebSocketMessage("fetch", new UserVerifyPacket()
            {
                uid = "0",
                token = ""
            });
            SocketIO.Instance.SendWebSocketMessage("sendMessage", new SendMessagePacket
            {
                fid = this.GetModel<MessageDataModel>().CurrentChatFriendId,
                id_cons = this.GetModel<MessageDataModel>().CurrentChatSessionId,
                msg = _messageView.chatInput.text,
                uid = this.GetModel<UserProfileDataModel>().UserProfileData.UserId
            });
            AddMessage(this.GetModel<UserProfileDataModel>().UserProfileData.UserId,
                _messageView.chatInput.text,
                DateTime.Now.ToString(CultureInfo.InvariantCulture));
        }

        private void AddMessage(string userId, string content, string time)
        {
            this.GetModel<MessageDataModel>().CurrentChatSession.ChatData.Add(new ChatMessage()
            {
                UserId = userId,
                Content = content,
                Time = time
            });
            _messageView.RefreshList();
        }
        
        /// <summary>
        ///  Receive new message from server
        /// </summary>
        public void ReceiveNewMessage(SendMessagePacket packet)
        {
            Debug.Log($"Receive packet {packet.fid} {packet.uid} {packet.id_cons} {packet.msg}");
            AddMessage(packet.fid, packet.msg, DateTime.Now.ToString(CultureInfo.InvariantCulture));
        }
           
        /// <summary>
        /// Load all message from server
        /// </summary>
        public void LoadMessage()
        {
            
        }


        public override void OnInit(List<ViewBase> view)
        {
            _messageView = view[0] as MessageView;
            _messageView.send.onClick.AddListener((() =>
            {
                SendMessage();
            }));
            SocketIO.Instance.AddUnityCallback("receivedMessage",(res) =>
            {
                ReceiveNewMessage(res.GetValue<SendMessagePacket>());
            });
        }

        public ViewBase OpenMessageView()
        {
            _messageView.Display();
            _messageView.Render(this.GetModel<MessageDataModel>());
            return null;
        }
    }
}