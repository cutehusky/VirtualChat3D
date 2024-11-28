using System.Collections.Generic;
using Assets.Scripts.Core.AdminModule.Model;
using Core.FriendModule;
using Core.MVC;
using Core.UserAccountModule.Model;
using QFramework;

namespace Core.AdminModule.Model
{
    public class UserAccountDataModel: ModelBase
    {
        public List<UserData> UserList;
        
        protected override void OnInit()
        {
            UserList = new();
        }
    }
}