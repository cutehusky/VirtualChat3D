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

        public override void OnInit()
        {
            email.onValueChanged.AddListener(s =>
            {
                if (EmailCheck(s))
                    email.textComponent.color = Color.green;
                else
                    email.textComponent.color = Color.red;
            });
        }
    }
}