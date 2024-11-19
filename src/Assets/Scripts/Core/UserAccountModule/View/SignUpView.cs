using System;
using System.Text.RegularExpressions;
using Core.MVC;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Core.UserAccountModule.View
{
    public class SignUpView: ViewBase
    {
        public TMP_InputField email;
        public TMP_InputField password;
        public TMP_InputField re_password;
        public TMP_Text notice;
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
            re_password.text = "";
        }

        public void SetNotice(string notice)
        {
            this.notice.text = notice;
            Hide();
            Display();
            
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
            password.onValueChanged.AddListener(s =>
            {
                if (s == re_password.text)
                    email.textComponent.color = Color.green;
                else
                    email.textComponent.color = Color.red;
            });
            re_password.onValueChanged.AddListener(s =>
            {
                if (s == password.text)
                    email.textComponent.color = Color.green;
                else
                    email.textComponent.color = Color.red;
            });
        }
    }
}