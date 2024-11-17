using Core.MVC;
using TMPro;

namespace Core.UserAccountModule.View
{
    public class SignUpView: ViewBase
    {
        public TMP_InputField email;
        public TMP_InputField password;
        public TMP_InputField re_password;
        
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