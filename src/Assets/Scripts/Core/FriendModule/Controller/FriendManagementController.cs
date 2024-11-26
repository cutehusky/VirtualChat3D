using System.Collections.Generic;
using System.Threading.Tasks;
using Core.FriendModule.View;
using Core.MVC;
using Core.FriendModule.Model;
using QFramework;
using UnityEngine;
using TMPro;
using Core.NetworkModule.Controller;
using Assets.Scripts.Core.NetworkModule.Controller;
using Core.UserAccountModule.Model;
using System;

namespace Core.FriendModule.Controller
{
    public class FriendManagementController : ControllerBase
    {
        private FriendListView _friendListView;
        private FriendDataModel _friendDataModel;

        public override void OnInit(List<ViewBase> view)
        {
            _friendListView = view[0] as FriendListView;
            _friendDataModel = this.GetModel<FriendDataModel>();
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
            // Ensure the FriendListView is properly initialized
            if (_friendListView == null)
            {
                Debug.LogError("FriendListView is not initialized.");
                return null;
            }

            LoadFriendList();
            _friendListView.Display();
            _friendListView.Render(this.GetModel<FriendDataModel>());
            return _friendListView;
        }
    }
}