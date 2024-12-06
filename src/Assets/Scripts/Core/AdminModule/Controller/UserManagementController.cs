using System.Collections.Generic;
using Core.AdminModule.View;
using Core.MVC;
using QFramework;
using Core.AdminModule.Model;
using Core.NetworkModule.Controller;
using System;
using Core.UserAccountModule.Model;
using Core.FriendModule.Model;
using Core.FriendModule.View;
using Assets.Scripts.Core.AdminModule.Model;
using UnityEngine;
using static Unity.Networking.Transport.Utilities.ReliableUtility;


namespace Core.AdminModule.Controller
{
    public class UserManagementController : ControllerBase
    {
        private UserListView _userListView;
        public void RemoveUser(string userId)
        {
            SocketIO.Instance.SendWebSocketMessage("processRemoveUser", new AdminReqPacket()
            {
                uid = userId
            });
        }
        
        public void LockUser(string userId)
        {
            SocketIO.Instance.SendWebSocketMessage("processLockUser", new AdminReqPacket()
            {
                uid = userId
            });
        }

        public void UnlockUser(string userId)
        {
            SocketIO.Instance.SendWebSocketMessage("processUnlockUser", new AdminReqPacket()
            {
                uid = userId
            });
        }
        
        public void LoadUserList()
        {
            this.GetModel<UserAccountDataModel>().UserList.Clear();
            SocketIO.Instance.SendWebSocketMessage("getUserList", "");
        }
        
        public override void OnInit(List<ViewBase> view)
        {
            base.OnInit(view);
            _userListView = view[0] as UserListView;
            _userListView.OnLockUser += LockUser;
            _userListView.OnUnlockUser += UnlockUser;
            _userListView.OnRemoveUser += RemoveUser;
            SocketIO.Instance.AddUnityCallback("lockUserReply", (res) =>
            {
                LoadUserList();
            });
            SocketIO.Instance.AddUnityCallback("unlockUserReply", (res) =>
            {
                LoadUserList();
            });
            SocketIO.Instance.AddUnityCallback("removeUserReply", (res) =>
            {
                LoadUserList();
            });
            SocketIO.Instance.AddUnityCallback("getUserListReply", (res) =>
            {
                this.GetModel<UserAccountDataModel>().UserList.Clear();
                var data = res.GetValue<UserDataPacket[]>();
                foreach (var item in data) {
                    this.GetModel<UserAccountDataModel>().UserList.Add(new UserData()
                    {
                        UserId = item.uid,
                        DateOfBirth = DateTimeOffset.FromUnixTimeMilliseconds(item.birthday).DateTime,
                        Description = item.description,
                        Username = item.username,
                        IsLock = !item.status
                    }); 
                }
                _userListView.RefreshList();
            });
        }

        public ViewBase OpenUserListView()
        {
            AppMain.Instance.CloseCurrentView();
            LoadUserList();
            _userListView.Display();
            _userListView.Render(this.GetModel<UserAccountDataModel>());
            return _userListView;
        }
    }
}