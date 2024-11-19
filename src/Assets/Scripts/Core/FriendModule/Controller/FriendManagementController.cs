using System.Collections.Generic;
using Core.FriendModule.View;
using Core.MVC;

namespace Core.MessageModule.Controller
{
    public class FriendManagementController : ControllerBase
    {
        private FriendListView _friendListView;
        public void LoadFriendList()
        {

        }

        public void AddFriend(string userId)
        {
            
        }
        
        public void AcceptFriend(string userId)
        {
            
        }

        public void RemoveFriend(string userId)
        {
            
        }

        public override void OnInit(List<ViewBase> view)
        {
            _friendListView = view[0] as FriendListView
        }

        public void OpenFriendListView ()
        {
            // CODE HERE
        }
    }
}