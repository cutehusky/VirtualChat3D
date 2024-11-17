using System.Collections.Generic;
using Core.MVC;
using Core.UserAccountModule.Model;

namespace Core.OnlineRuntimeModule.RoomManagementModule.Model
{
    public class RoomDataModel: ModelBase
    {
        public List<RoomData> RoomsData;
        public RoomData CurrentHostRoomData;
        public List<UserAccountData> CurrentHostRoomJoinedUser;
        
        protected override void OnInit()
        {
            
        }
    }
}