using System.Collections.Generic;
using Core.FriendModule.View;
using Core.MVC;
using Core.FriendModule.Model;
using QFramework;
using Core.NetworkModule.Controller;
using Assets.Scripts.Core.NetworkModule.Controller;
using Core.UserAccountModule.Model;
using System;

namespace Core.FriendModule.Controller
{
    public class FriendManagementController : ControllerBase
    {
        private FriendListView _friendListView;

        public override void OnInit(List<ViewBase> view)
        {
            base.OnInit(view);
            _friendListView = view[0] as FriendListView;
            _friendListView.addFriendButton.onClick.AddListener(() =>
            {
                var text = _friendListView.userIdSearch.text;
                SendFriendRequest(text);
                _friendListView.userIdSearch.text = "";
            });
            _friendListView.OpenMessageView += (chatSession,fid) =>
            {
                AppMain.Instance.OpenMessageView(chatSession,fid);
            };
            _friendListView.OnRemoveFriend += RemoveFriend;
            _friendListView.OnRequestAccept += AcceptFriendRequest;
            _friendListView.OnRequestRefuse += RefuseFriendRequest;
            
            SocketIO.Instance.AddUnityCallback("viewFriendReply", (res) => {
               var packets = res.GetValue<FriendRepPacket[]>();
                foreach (var packet in packets) {
                    this.GetModel<FriendDataModel>().FriendList.Add(new FriendData()
                    {
                        ChatSessionId = packet.id_cons,
                        IsAccepted = true,
                        UserId = packet.uid,
                        Username = packet.username,
                        DateOfBirth = DateTimeOffset.FromUnixTimeMilliseconds(packet.birthday).DateTime,
                        Description = packet.description,

                    });

                }
            });
            SocketIO.Instance.AddUnityCallback("viewFriendRequestReply", (res) => {
                var packets = res.GetValue<FriendRepPacket[]>();
                foreach (var packet in packets)
                {
                    this.GetModel<FriendDataModel>().FriendList.Add(new FriendData()
                    {
                        ChatSessionId = packet.id_cons,
                        IsAccepted = false,
                        UserId = packet.uid,
                        Username = packet.username,
                        DateOfBirth = DateTimeOffset.FromUnixTimeMilliseconds(packet.birthday).DateTime,
                        Description = packet.description,
                    });
                }
            });
            SocketIO.Instance.AddUnityCallback("sendFriendRequestReply", (res) => {
                var packets = res.GetValue<FriendRepPacket>();
            });
            SocketIO.Instance.AddUnityCallback("friendRequestAcceptReply", (res) => {
                var packets = res.GetValue<FriendRepPacket>();
            });
            SocketIO.Instance.AddUnityCallback("friendRequestRefuseReply", (res) => {
                var packets = res.GetValue<FriendRepPacket>();
            });
            SocketIO.Instance.AddUnityCallback("processRemoveFriendReply", (res) => {
                var packets = res.GetValue<FriendRepPacket>();
            });

        }


        public void LoadFriendList()
        {
            this.GetModel<FriendDataModel>().FriendList.Clear();
            SocketIO.Instance.SendWebSocketMessage("processViewFriendList", new FriendPacket()
            {
                uid = this.GetModel<UserProfileDataModel>().UserProfileData.UserId
            });

            SocketIO.Instance.SendWebSocketMessage("processViewFriendRequestList", new FriendPacket()
            {
                uid = this.GetModel<UserProfileDataModel>().UserProfileData.UserId
            });
            //for (int i = 0; i < 20; i++)
            //{
            //    this.GetModel<FriendDataModel>().FriendList.Add(new FriendData()
            //    {
            //        ChatSessionId = "0",
            //        IsAccepted = true,
            //        UserId = i.ToString(),
            //        Username = i.ToString(),
            //        DateOfBirth = DateTimeOffset.FromUnixTimeMilliseconds(123456789).DateTime,
            //        Description = "testing",
            //    });
            //}
        }

        public void SendFriendRequest(string targetUserId)
        {
            SocketIO.Instance.SendWebSocketMessage("sendFriendRequest", new FriendPacket()
            {
                uid = this.GetModel<UserProfileDataModel>().UserProfileData.UserId,
                fid = targetUserId
            });
        }

        public void AcceptFriendRequest(string requesterId)
        {
            SocketIO.Instance.SendWebSocketMessage("friendRequestAccept", new FriendPacket()
            {
                uid = this.GetModel<UserProfileDataModel>().UserProfileData.UserId,
                fid = requesterId
            });
        }

        public void RefuseFriendRequest(string requesterId)
        {
            SocketIO.Instance.SendWebSocketMessage("friendRequestRefuse", new FriendPacket()
            {
                uid = this.GetModel<UserProfileDataModel>().UserProfileData.UserId,
                fid = requesterId
            });
        }


        public void RemoveFriend(string friendId)
        {
            SocketIO.Instance.SendWebSocketMessage("processRemoveFriend", new FriendPacket()
            {
                uid = this.GetModel<UserProfileDataModel>().UserProfileData.UserId,
                fid = friendId
            });
        }

        public ViewBase OpenFriendListView()
        {
            AppMain.Instance.CloseCurrentView();
            LoadFriendList();
            _friendListView.Display();
            _friendListView.Render(this.GetModel<FriendDataModel>());
            return _friendListView;
        }
    }
}