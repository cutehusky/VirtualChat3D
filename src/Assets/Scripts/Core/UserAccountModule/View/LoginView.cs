using Core.MVC;
using TMPro;
using UnityEngine.UI;

namespace Core.UserAccountModule.View
{
    public class LoginView : ViewBase
    {
        public TMP_InputField email;
        public TMP_InputField password;
        public Button resetPassword;
        
        public bool PasswordCheck(string value)
        {
            return true;
        }
        
        public bool EmailCheck()
        {
            return true;
        }

        public override void Render(ModelBase model)
        {
            
        }

        public override void OnInit()
        {
            
        }
    }
}