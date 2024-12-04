using System.Collections.Generic;
using Core.MVC;
using QFramework;

namespace Core.FriendModule.Model
{
    public class FriendDataModel: ModelBase
    {
        public Dictionary<string, FriendData> FriendList;
        public Dictionary<string, FriendData> RequestList;
        protected override void OnInit()
        {
            FriendList = new();
            RequestList = new();
        }
    }
}