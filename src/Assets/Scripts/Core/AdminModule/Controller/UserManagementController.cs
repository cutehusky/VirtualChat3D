using System.Collections.Generic;
using Core.AdminModule.View;
using Core.MVC;
using QFramework;
using Core.AdminModule.Model;
using Core.NetworkModule.Controller;
using Assets.Scripts.Core.NetworkModule.Controller;
using System;
using Core.UserAccountModule.Model;
using Core.FriendModule.Model;
using Core.FriendModule.View;


namespace Core.AdminModule.Controller
{
    public class UserManagementController : ControllerBase
    {
        private UserListView _userListView;
        public void RemoveUser(string userId)
        {
            SocketIO.Instance.SendWebSocketMessage("processRemoveUser", new AdminManageUserPacket()
            {
                uid = userId
            });
        }
        
        public void LockUser(string userId)
        {
            SocketIO.Instance.SendWebSocketMessage("processLockUser", new AdminManageUserPacket()
            {
                uid = userId
            });
        }

        public void UnlockUser(string userId)
        {
            SocketIO.Instance.SendWebSocketMessage("processUnlockUser", new AdminManageUserPacket()
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