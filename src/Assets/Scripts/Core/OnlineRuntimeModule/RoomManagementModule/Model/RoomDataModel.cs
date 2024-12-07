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
                        RoomsData.Clear();
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

        public void DeleteRoom(string roomId)
        {
            FirebaseDatabase.DefaultInstance
                .GetReference($"Account/{UserProfileDataModel.UserProfileData.UserId}/Rooms/{roomId}")
                .RemoveValueAsync()
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        RoomsData.RemoveAll(room => room.RoomId == roomId);
                        Debug.Log($"Room {roomId} deleted successfully.");
                    }
                    else if (task.IsFaulted)
                    {
                        Debug.LogError($"Failed to delete room {roomId}: {task.Exception}");
                    }
                });
        }

        public void CreateRoom()
        {
            RoomData roomData = new RoomData();
            roomData.AccessType = EAccessType.Public;

            var newRoomRef = FirebaseDatabase.DefaultInstance.
            GetReference($"Account/{UserProfileDataModel.UserProfileData.UserId}/Rooms").Push();

            string roomId = newRoomRef.Key;

            roomData.RoomId = roomId;
            
            newRoomRef.SetRawJsonValueAsync(JsonUtility.ToJson(roomData)).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    FetchRoomsList();
                    RoomsData.Add(roomData);
                    Debug.Log($"Room {roomId} created successfully.");
                }
                else if (task.IsFaulted)
                {
                    Debug.LogError($"Failed to create room {roomId}: {task.Exception}");
                }
            });
        }
    }
}