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
        public string CurrentEditingRoomId;
        public List<EnvironmentItemData> CurrentEditingEnvironmentData;
        public bool IsPlacingItem;

        protected override void OnInit()
        {
            CurrentEditingEnvironmentData = new();
        }

        public void FetchRoomsEnvironment(string userId)
        {
            FirebaseDatabase.DefaultInstance.GetReference($"Account/{userId}/Rooms/{CurrentEditingRoomId}/Environment")
                .GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.LogError("FetchRoomsEnvironment failed.");
                        return;
                    }
                    else if (task.IsCompleted)
                    {
                        CurrentEditingEnvironmentData.Clear();
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

                            CurrentEditingEnvironmentData.Add(itemData);
                        }
                    }
                });
        }

        public void SaveRoomEnvironmentData(string userId)
        {
            var environmentRef = FirebaseDatabase.DefaultInstance
                .GetReference($"Account/{userId}/Rooms/{CurrentEditingRoomId}/Environment");

            foreach (var itemData in CurrentEditingEnvironmentData){
                if (string.IsNullOrEmpty(itemData.UID))
                {
                    Debug.LogError("UID is null or empty. Cannot push data.");
                    return;
                }

                var itemRef = environmentRef.Child(itemData.UID);
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