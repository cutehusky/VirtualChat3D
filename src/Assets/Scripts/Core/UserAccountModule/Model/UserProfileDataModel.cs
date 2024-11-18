using System;
using Core.MVC;
using QFramework;
using UnityEngine;

namespace Core.UserAccountModule.Model
{
    public class UserProfileDataModel: ModelBase
    {
        public UserAccountData UserProfileData = new();
        
        protected override void OnInit()
        {
        }
    }
}