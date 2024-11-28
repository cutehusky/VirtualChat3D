using System;
using System.Diagnostics;
using Core.MVC;
using Core.UserAccountModule.Model;
using TMPro;
using UI.Dates;
using UnityEngine.UI;

namespace Core.UserAccountModule.View
{
    public class UserProfileView: ViewBase
    {
        public TMP_Text userId;
        public TMP_InputField username;
        public TMP_InputField description;
        public Image avatar;
        public DatePicker dateOfBirth;
        public Button save;
        public Button cancel;
        public Button resetPassword;
        public Button signOut;

        public override void Render(ModelBase model)
        {
            var userData = (model as UserProfileDataModel).UserProfileData;
            userId.text = userData.UserId;
            username.text = userData.Username;
            description.text = userData.Description;
        }

        public override void OnInit()
        {
            
        }
    }
}