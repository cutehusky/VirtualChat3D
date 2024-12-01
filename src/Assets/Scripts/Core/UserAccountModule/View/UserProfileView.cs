using System;
using System.Diagnostics;
using Core.MVC;
using Core.UI;
using Core.UserAccountModule.Model;
using TMPro;
using UI.Dates;
using UMI;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace Core.UserAccountModule.View
{
    public class UserProfileView: ViewBase
    {
        public TMP_InputField userId;
        public TMP_InputField email;
        public MobileInputField username;
        public MobileInputField description;
        
        public TMP_InputField TMP_username;
        public TMP_InputField TMP_description;
        
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
            email.text = userData.Email;
            username.Text = userData.Username;
            description.Text = userData.Description;
            dateOfBirth.SelectedDate = userData.DateOfBirth;
#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IOS)
            TMP_username.text = userData.Username;
            TMP_description.text = userData.Description;
#endif
        }

        public override void OnInit()
        {
        }
    }
}