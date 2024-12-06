using System.Collections.Generic;
using Core.FriendModule.View;
using Core.MVC;
using Core.FriendModule.Model;
using QFramework;
using Core.NetworkModule.Controller;
using Core.UserAccountModule.Model;
using System;
using UnityEngine;

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
                var text = _friendListView.userIdSearch.Text;
                SendFriendRequest(text);
                _friendListView.userIdSearch.Text = "";
#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IOS)
                _friendListView.TMP_userIdSearch.text = "";
#endif
            });
            _friendListView.OpenMessageView += (chatSession,fid) =>
            {
                AppMain.Instance.OpenMessageView(chatSession,fid);
            };
            
            _friendListView.OnRemoveFriend += RemoveFriend;
            _friendListView.OnRequestAccept += AcceptFriendRequest;
            _friendListView.OnRequestRefuse += RefuseFriendRequest;
            
            SocketIO.Instance.AddUnityCallback("viewFriendReply", (res) => {
               var packets = res.GetValue<UserDataPacket[]>();
                this.GetModel<FriendDataModel>().FriendList.Clear();
                foreach (var packet in packets) {
                    this.GetModel<FriendDataModel>().FriendList.Add(packet.uid,new FriendData()
                    {
                        ChatSessionId = packet.id_cons,
                        UserId = packet.uid,
                        Username = packet.username,
                        DateOfBirth = DateTimeOffset.FromUnixTimeMilliseconds(packet.birthday).DateTime,
                        Description = packet.description,
                    });
                }
                _friendListView.RefreshList(); 
            });
            
            SocketIO.Instance.AddUnityCallback("viewFriendRequestReply", (res) => {
                var packets = res.GetValue<UserDataPacket[]>();
                this.GetModel<FriendDataModel>().RequestList.Clear();
                foreach (var packet in packets)
                {
                    this.GetModel<FriendDataModel>().RequestList.Add(packet.uid, new FriendData()
                    {
                        ChatSessionId = packet.id_cons,
                        UserId = packet.uid,
                        Username = packet.username,
                        DateOfBirth = DateTimeOffset.FromUnixTimeMilliseconds(packet.birthday).DateTime,
                        Description = packet.description,
                    });
                }
                _friendListView.RefreshList(); 
            });
            
            SocketIO.Instance.AddUnityCallback("friendRequestReply", (res) => {
                var packets = res.GetValue<FriendReqPacket>();
            });
            
            SocketIO.Instance.AddUnityCallback("receivedFriendRequest", (res) => {
                LoadFriendList();
            });
             
            SocketIO.Instance.AddUnityCallback("friendRequestAcceptReply", (res) => {
                LoadFriendList();
            });
            
            SocketIO.Instance.AddUnityCallback("friendRequestRefuseReply", (res) => {
                LoadFriendList();
            });
            
            SocketIO.Instance.AddUnityCallback("friendRemoveReply", (res) => {
                LoadFriendList();
            });
            
            SocketIO.Instance.AddUnityCallback("friendRemove", (res) => {
                LoadFriendList();
            });
        }


        public void LoadFriendList()
        {
            this.GetModel<FriendDataModel>().FriendList.Clear();
            SocketIO.Instance.SendWebSocketMessage("processViewFriendList", new FriendReqPacket()
            {
                uid = this.GetModel<UserProfileDataModel>().UserProfileData.UserId
            });

            SocketIO.Instance.SendWebSocketMessage("processViewFriendRequestList", new FriendReqPacket()
            {
                uid = this.GetModel<UserProfileDataModel>().UserProfileData.UserId
            });
            _friendListView.RefreshList(); 
        }

        public void SendFriendRequest(string targetUserId)
        {
            SocketIO.Instance.SendWebSocketMessage("sendFriendRequest", new FriendReqPacket()
            {
                uid = this.GetModel<UserProfileDataModel>().UserProfileData.UserId,
                fid = targetUserId
            });
        }

        public void AcceptFriendRequest(string requesterId)
        {
            SocketIO.Instance.SendWebSocketMessage("friendRequestAccept", new FriendReqPacket()
            {
                uid = this.GetModel<UserProfileDataModel>().UserProfileData.UserId,
                fid = requesterId
            });
        }

        public void RefuseFriendRequest(string requesterId)
        {
            SocketIO.Instance.SendWebSocketMessage("friendRequestRefuse", new FriendReqPacket()
            {
                uid = this.GetModel<UserProfileDataModel>().UserProfileData.UserId,
                fid = requesterId
            });
        }


        public void RemoveFriend(string friendId)
        {
            SocketIO.Instance.SendWebSocketMessage("processRemoveFriend", new FriendReqPacket()
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