using System.Collections.Generic;
using Core.MVC;
using Core.UserAccountModule.Model;
using Core.UserAccountModule.View;
using QFramework;
using UnityEngine;

namespace Core.UserAccountModule.Controller
{
    public class UserProfileController: ControllerBase
    {
        private UserProfileView _userProfileView;

        public ViewBase OpenUserProfileView()
        {
            AppMain.Instance.CloseCurrentView();
            _userProfileView.Display();
            _userProfileView.Render(this.GetModel<UserProfileDataModel>());
            return _userProfileView;
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
            base.OnInit(view);
            _userProfileView = (UserProfileView) view[0];
            _userProfileView.reset.onClick.AddListener(() =>
            {

            });
            _userProfileView.save.onClick.AddListener(() =>
            {
                
            });
            _userProfileView.resetPassword.onClick.AddListener(() =>
            {
                
            });
        }
    }
}