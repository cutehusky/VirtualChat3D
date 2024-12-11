using System;
using System.Collections.Generic;
using Core.MVC;
using Core.UserAccountModule.Model;
using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using UnityEngine;

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

        public void FetchRoomsList(string UserId, Action onSuccess = null, Action onFail = null)
        {
            FirebaseDatabase.DefaultInstance.GetReference($"Account/{UserId}/Rooms")
                .GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                        onFail!();
                        return;
                    }
                    if (task.IsCompleted)
                    {
                        RoomsData.Clear();
                        DataSnapshot snapshot = task.Result;
                        foreach (DataSnapshot roomSnapshot in snapshot.Children)
                        {
                            RoomData roomData = new RoomData();
                            roomData.RoomId = roomSnapshot.Key;
                            roomData.AccessType = (EAccessType) Convert.ToInt16(roomSnapshot.Child("AccessType").GetRawJsonValue());
                            RoomsData.Add(roomData);
                        }
                        onSuccess!();
                    }
                });
        }

        public void DeleteRoom(string UserId, string roomId, Action onSuccess = null, Action onFail = null)
        {
            FirebaseDatabase.DefaultInstance
                .GetReference($"Account/{UserId}/Rooms/{roomId}")
                .RemoveValueAsync()
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        RoomsData.RemoveAll(room => room.RoomId == roomId);
                        Debug.Log($"Room {roomId} deleted successfully.");
                        onSuccess!();
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.LogError($"Failed to delete room {roomId}: {task.Exception}");
                        onFail!();
                    }
                });
        }

        public void CreateRoom(string UserId, bool isPrivate, Action onSuccess = null, Action onFail = null)
        {
            RoomData roomData = new RoomData();
            if (isPrivate)
            {
                roomData.AccessType = EAccessType.Friend;
            }
            else
            {
                roomData.AccessType = EAccessType.Anyone;
            }

            var newRoomRef = FirebaseDatabase.DefaultInstance.
            GetReference($"Account/{UserId}/Rooms").Push();

            string roomId = newRoomRef.Key;

            roomData.RoomId = roomId;
            
            newRoomRef.SetRawJsonValueAsync(JsonUtility.ToJson(roomData)).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    RoomsData.Add(roomData);
                    onSuccess!();
                    Debug.Log($"Room {roomId} created successfully.");
                }
                else if (task.IsFaulted)
                {
                    onFail!();
                    Debug.LogError($"Failed to create room {roomId}: {task.Exception}");
                }
            });
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