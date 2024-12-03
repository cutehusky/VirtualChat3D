using System.Collections.Generic;
using Core.MVC;
using QFramework;

namespace Core.FriendModule.Model
{
    public class FriendDataModel: ModelBase
    {
        public List<FriendData> FriendList;
        public List<FriendData> RequestList;
        protected override void OnInit()
        {
            FriendList = new();
            RequestList = new();
        }
    }
}