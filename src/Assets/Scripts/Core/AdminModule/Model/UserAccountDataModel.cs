using System.Collections.Generic;
using Core.FriendModule;
using Core.UserAccountModule.Model;
using QFramework;

namespace Core.AdminModule.Model
{
    public class UserAccountDataModel: AbstractModel
    {
        public List<UserAccountData> UserList;
        
        protected override void OnInit()
        {
            UserList = new();
        }
    }
}