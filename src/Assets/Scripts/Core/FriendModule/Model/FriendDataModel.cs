using System.Collections.Generic;
using Core.FriendModule;
using Core.UserAccountModule.Model;
using QFramework;

namespace Core.MessageModule.Model
{
    public class FriendDataModel: AbstractModel
    {
        public List<FriendData> FriendList;
        protected override void OnInit()
        {
            FriendList = new();
        }
    }
}