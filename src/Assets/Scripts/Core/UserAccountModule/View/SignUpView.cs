using System;
using System.Text.RegularExpressions;
using Core.MVC;
using TMPro;
using UMI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Core.UserAccountModule.View
{
    public class SignUpView: ViewBase
    {
        public MobileInputField email;
        public MobileInputField password;
        public MobileInputField re_password;
        public TMP_InputField TMP_email;
        public TMP_InputField TMP_password;
        public TMP_InputField TMP_re_password;
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
            
            email.Text = "";
            password.Text = "";
            re_password.Text = "";
#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IOS)
            TMP_email.text = "";
            TMP_password.text = "";
            TMP_re_password.text = "";
#endif
            notice.text = "";
        }

        public void SetNotice(string notice)
        {
            this.notice.text = $"<color=red>{notice}</color>"; ;
        }
        public void SetNoticeSuccess(string notice)
        {
            this.notice.text = $"<color=green>{notice}</color>";
        }

        public override void OnInit()
        {
            TMP_email.onValueChanged.AddListener(s =>
            {
                if (s.Length == 0)
                {
                    email.SetTextColor(Color.green);
                    return;
                }
                email.SetTextColor(EmailCheck(s) ? Color.green : Color.red);
            });
        }
    }
}