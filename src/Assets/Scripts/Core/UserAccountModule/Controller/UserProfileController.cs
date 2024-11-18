using System.Collections.Generic;
using Core.MVC;
using Core.UserAccountModule.Model;
using Core.UserAccountModule.View;
using QFramework;

namespace Core.UserAccountModule.Controller
{
    public class UserProfileController: ControllerBase
    {
        public UserProfileView UserProfileView;

        public ViewBase OpenUserProfileView()
        {
            AppMain.Instance.CloseCurrentView();
            UserProfileView.Display();
            UserProfileView.Render(this.GetModel<UserProfileDataModel>());
            return UserProfileView;
        }
        
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
        
        public override void OnInit(List<ViewBase> view)
        {
            UserProfileView = (UserProfileView) view[0];
        }
    }
}