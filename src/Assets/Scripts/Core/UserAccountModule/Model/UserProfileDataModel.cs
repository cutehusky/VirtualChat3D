using System;
using Core.MVC;
using QFramework;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

namespace Core.UserAccountModule.Model
{
    public class UserProfileDataModel: ModelBase
    {
        public UserAccountData UserProfileData = new();

        public void SaveProfile(FirebaseAuth auth)
        {
            var reference = FirebaseDatabase.DefaultInstance.GetReference($"Account/{auth.CurrentUser.UserId}");
            reference.Child("description").SetValueAsync(UserProfileData.Description);
            reference.Child("username").SetValueAsync(UserProfileData.Username);
            reference.Child("birthday").SetValueAsync(((DateTimeOffset)UserProfileData.DateOfBirth).ToUnixTimeMilliseconds() + 3600 * 24 * 1000);
        }
        
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