using System;
using Core.MVC;
using QFramework;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using static Unity.Networking.Transport.Utilities.ReliableUtility;

namespace Core.UserAccountModule.Model
{
    public class UserProfileDataModel: ModelBase
    {
        public UserAccountData UserProfileData = new();
        public void fetchProfile(FirebaseAuthModel auth, Action onSuccess, Action onFailure)
        {
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            FirebaseDatabase.DefaultInstance.GetReference($"Account/{auth.Auth.CurrentUser.UserId}")
                .GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                        onFailure();
                    }
                    else if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result;

                        this.UserProfileData.Username = snapshot.Child("username").GetValue(false) as string;
                        this.UserProfileData.DateOfBirth = DateTimeOffset.FromUnixTimeMilliseconds((long)snapshot.Child("birthday").GetValue(false)).DateTime;
                        this.UserProfileData.Description = snapshot.Child("description").GetValue(true) as string;
                        this.UserProfileData.UserId = auth.Auth.CurrentUser.UserId;
                        onSuccess();
                    }
                });
        }
        protected override void OnInit()
        {

        }
    }
}