using System;
using System.Text.RegularExpressions;
using Core.MVC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UserAccountModule.View
{
    public class LoginView : ViewBase
    {
        public TMP_InputField email;
        public TMP_InputField password;
        public TMP_Text notice;
        public Button resetPassword;
        public Button login;
        public Button signUp;
        
        public static bool EmailCheck(string s)
        {
            string regex = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            return Regex.IsMatch(s, regex);
        }

        public override void Render(ModelBase model)
        {
            email.text = "";
            password.text = "";
        }

        public void SetNotice(string notice)
        {
            this.notice.text = notice;
            Hide();
            Display();
            
        }

        public override void OnInit()
        {
            login.onClick.AddListener(() =>
            {
                if (email.text == "" || password.text == "")
                    notice.text = "Please fill in email and password!";
            });
        }
    }
}