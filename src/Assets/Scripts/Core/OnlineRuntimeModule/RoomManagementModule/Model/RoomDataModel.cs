using System.Collections.Generic;
using Core.MVC;
using Core.UserAccountModule.Model;

namespace Core.OnlineRuntimeModule.RoomManagementModule.Model
{
    public class RoomDataModel: ModelBase
    {
        public List<RoomData> RoomsData;
        public RoomData CurrentHostRoomData;
        public Dictionary<ulong, UserAccountData> CurrentHostRoomJoinedUser;
        
        protected override void OnInit()
        {
            CurrentHostRoomJoinedUser = new();
            RoomsData = new();
            CurrentHostRoomData = new();
        }

        public void RemoveUserFromList(ulong clientId)
        {
            CurrentHostRoomJoinedUser.Remove(clientId);
        }
        
        public void AddUserToList(string userId,  string username, ulong clientId)
        {
            CurrentHostRoomJoinedUser.Add(
                clientId, new UserAccountData()
                {
                    UserId = userId,
                    Username = username
                });
        }
    }
}