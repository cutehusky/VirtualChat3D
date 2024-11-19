using System.Collections.Generic;
using Core.AdminModule.View;
using Core.MVC;

namespace Core.AdminModule.Controller
{
    public class UserManagementController : ControllerBase
    {
        private UserListView _userListView;
        public void RemoveUser(string userId)
        {
            
        }
        
        public void LockUser(string userId)
        {
            
        }

        public void UnlockUser(string userId)
        {
            
        }
        
        public void LoadUserList()
        {

        }
        
        public override void OnInit(List<ViewBase> view)
        {
            _userListView = view[0] as UserListView;
        }

        public ViewBase OpenUserListView()
        {
            // CODE HERE
            return null;
        }
    }
}