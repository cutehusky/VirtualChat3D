using System.Collections.Generic;
using Core.MVC;
using QFramework;

namespace Core.FriendModule.Model
{
    public class FriendDataModel: ModelBase
    {
        public List<FriendData> FriendList;
        protected override void OnInit()
        {
            FriendList = new();
        }
    }
}