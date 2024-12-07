using System.Collections.Generic;
using Core.MVC;
using Core.UserAccountModule.Model;
using Firebase;
using Firebase.Database;

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

        public void FetchRoomsList()
        {
            FirebaseDatabase.DefaultInstance.GetReference($"Account/{UserProfileDataModel.UserProfileData.UserId}/Rooms")
                .GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                        return;
                    }
                    else if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result;
                        foreach (DataSnapshot roomSnapshot in snapshot.Children)
                        {
                            RoomData roomData = new RoomData();
                            roomData.RoomId = roomSnapshot.Key;
                            roomData.AccessType = (EAccessType)roomSnapshot.Child("AccessType").GetValue(false);
                            RoomsData.Add(roomData);
                        }
                    }
                });
        }
    }
}