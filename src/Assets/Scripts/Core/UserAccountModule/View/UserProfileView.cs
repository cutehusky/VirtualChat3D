using System;
using System.Diagnostics;
using Core.MVC;
using Core.UI;
using Core.UserAccountModule.Model;
using TMPro;
using UI.Dates;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace Core.UserAccountModule.View
{
    public class UserProfileView: ViewBase
    {
        public TMP_InputField userId;
        public TMP_InputField username;
        public TMP_InputField description;
        public TMP_InputField email;
        public Image avatar;
        public DatePicker dateOfBirth;
        public Button save;
        public Button reset;
        public Button resetPassword;
        public Button signOut;

        public override void Render(ModelBase model)
        {
            var userData = (model as UserProfileDataModel).UserProfileData;
            Debug.Log(userData.UserId);
            Debug.Log(userData.Description);
            userId.text = userData.UserId.ToString();
            username.text = userData.Username;
            description.text = userData.Description;
            dateOfBirth.SelectedDate = userData.DateOfBirth;
            email.text = userData.Email;
        }

        public override void OnInit()
        {
            
        }
    }
}