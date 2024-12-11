using System;
using System.Collections.Generic;
using Core.MVC;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Database;

namespace Core.OnlineRuntimeModule.EnvironmentCustomize.Model
{
    public class EnvironmentDataModel: ModelBase
    {
        public string CurrentActiveRoomId;
        public List<EnvironmentItemData> CurrentActiveEnvironmentData;
        public Dictionary<GameObject, EnvironmentItemData> InSceneObject;
        public bool IsPlacingItem;

        protected override void OnInit()
        {
            CurrentActiveEnvironmentData = new();
            InSceneObject = new();
        }

        public void FetchRoomsEnvironment(string userId, Action onSuccess =null, Action onFail = null)
        {
            FirebaseDatabase.DefaultInstance.GetReference($"Account/{userId}/Rooms/{CurrentActiveRoomId}/Environment")
                .GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.LogError("FetchRoomsEnvironment failed.");
                        onFail!();
                        return;
                    }
                    if (task.IsCompleted)
                    {
                        CurrentActiveEnvironmentData.Clear();
                        DataSnapshot snapshot = task.Result;
                        foreach (DataSnapshot itemSnapshot in snapshot.Children)
                        {
                            EnvironmentItemData itemData = new EnvironmentItemData();
                            itemData.UID = itemSnapshot.Key;
                            itemData.ID = int.Parse(itemSnapshot.Child("ID").GetValue(false).ToString());
                            itemData.PosX = float.Parse(itemSnapshot.Child("PosX").GetValue(false).ToString());
                            itemData.PosY = float.Parse(itemSnapshot.Child("PosY").GetValue(false).ToString());
                            itemData.PosZ = float.Parse(itemSnapshot.Child("PosZ").GetValue(false).ToString());
                            itemData.RotY = float.Parse(itemSnapshot.Child("RotY").GetValue(false).ToString());
                            CurrentActiveEnvironmentData.Add(itemData);
                        }
                        onSuccess!();
                    }
                });
        }

        public void SaveRoomEnvironmentData(string userId)
        {
            var environmentRef = FirebaseDatabase.DefaultInstance
                .GetReference($"Account/{userId}/Rooms/{CurrentActiveRoomId}/Environment");

            foreach (var itemData in CurrentActiveEnvironmentData){
                var itemRef =string.IsNullOrEmpty(itemData.UID) ?environmentRef.Push():  environmentRef.Child(itemData.UID);
                itemData.UID = itemRef.Key;
                itemRef.SetRawJsonValueAsync(JsonUtility.ToJson(itemData)).ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        Debug.Log($"Environment item with UID {itemData.UID} pushed successfully.");
                    }
                    else if (task.IsFaulted)
                    {
                        Debug.LogError($"Failed to push Environment item with UID {itemData.UID}: {task.Exception}");
                    }
                });
            }
        }
    }
}