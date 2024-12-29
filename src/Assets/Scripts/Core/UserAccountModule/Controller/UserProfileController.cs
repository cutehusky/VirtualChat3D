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
            _userProfileView.Display(this.GetModel<UserProfileDataModel>().UserProfileData.IsAdmin); 
            _userProfileView.Render(this.GetModel<UserProfileDataModel>());
            return _userProfileView;
        }

        public void SaveUserProfile()
        {
            this.GetModel<UserProfileDataModel>().UserProfileData.Description = _userProfileView.description.Text;
            this.GetModel<UserProfileDataModel>().UserProfileData.Username = _userProfileView.username.Text;
            this.GetModel<UserProfileDataModel>().UserProfileData.DateOfBirth = _userProfileView.dateOfBirth.SelectedDate;
            this.GetModel<UserProfileDataModel>().SaveProfile(this.GetModel<FirebaseAuthModel>().Auth);
        }
        
        public override void OnInit(List<ViewBase> view)
        {
            base.OnInit(view);
            _userProfileView = (UserProfileView) view[0];
            _userProfileView.reset.onClick.AddListener(() =>
            { 
                _userProfileView.description.Text = this.GetModel<UserProfileDataModel>().UserProfileData.Description;
                _userProfileView.username.Text = this.GetModel<UserProfileDataModel>().UserProfileData.Username;
                _userProfileView.dateOfBirth.SelectedDate = this.GetModel<UserProfileDataModel>().UserProfileData.DateOfBirth;
            });
            _userProfileView.save.onClick.AddListener(SaveUserProfile);
            _userProfileView.resetPassword.onClick.AddListener(() =>
            {
                
            });
        }
    }
}