using System.Collections.Generic;
using QFramework;

namespace Core.FriendModule.Model
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