using Core.MVC;
using Core.UserAccountModule.View;

namespace Core.UserAccountModule.Controller
{
    public class UserProfileController: ControllerBase
    {
        private UserProfileView _view;
        
        public void LoadUserProfile()
        {
            
        }
        
        public void EditUsername()
        {
            
        }

        public void EditDescription()
        {
            
        }

        public void EditDateOfBirth()
        {
            
        }

        public void EditAvatar()
        {
            
        }
        
        public override void OnInit(ViewBase view)
        {
            _view = (UserProfileView) view;
        }
    }
}