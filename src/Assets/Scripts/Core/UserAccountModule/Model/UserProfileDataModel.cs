using System;
using Core.MVC;
using QFramework;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using static Unity.Networking.Transport.Utilities.ReliableUtility;

namespace Core.UserAccountModule.Model
{
    public class UserProfileDataModel: ModelBase
    {
        public readonly UserAccountData UserProfileData = new();
        public void FetchProfile(FirebaseAuth auth, Action onSuccess, Action onFailure)
        {
            FirebaseDatabase.DefaultInstance.GetReference($"Account/{auth.CurrentUser.UserId}")
                .GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                        onFailure();
                    }
                    else if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result;

                        UserProfileData.Username = snapshot.Child("username").GetValue(false) as string;
                        UserProfileData.DateOfBirth = DateTimeOffset.FromUnixTimeMilliseconds((long)snapshot.Child("birthday").GetValue(false)).DateTime;
                        UserProfileData.Description = snapshot.Child("description").GetValue(true) as string;
                        UserProfileData.UserId = auth.CurrentUser.UserId;
                        UserProfileData.Email = auth.CurrentUser.Email;
                        Debug.Log(UserProfileData.UserId);
                        Debug.Log(UserProfileData.Username);
                        onSuccess();
                    }
                });
        }
        protected override void OnInit()
        {

        }
    }
}