using System;
using System.Collections.Generic;
using Core.MVC;
using Core.UserAccountModule.Model;
using Core.UserAccountModule.View;
using QFramework;
using UnityEngine;

namespace Core.UserAccountModule.Controller
{
    public class UserAuthController: ControllerBase
    {
        public LoginView LoginView;
        public SignUpView SignUpView;
        public UserProfileView UserProfileView;
        
        public ViewBase OpenSignUpView()
        {
            AppMain.Instance.CloseCurrentView();
            SignUpView.Display();
            SignUpView.Render(null);
            return SignUpView;
        }

        public ViewBase OpenLoginView()
        {
            AppMain.Instance.CloseCurrentView();
            LoginView.Display();
            LoginView.Render(null);
            return LoginView;
        }
        
        public void SignUp()
        {
            Debug.Log($"Sign up with email: {SignUpView.email.text} pass: {SignUpView.password.text}");
        }

        public void ResetPassword()
        {
            Debug.Log($"reset password with email: {LoginView.email.text}");

        }
        
        public void Login()
        {
            Debug.Log($"Login with email: {LoginView.email.text} pass: {LoginView.password.text}");
            // demo load data to model
            this.GetModel<UserProfileDataModel>().UserProfileData.UserId = "123";
            this.GetModel<UserProfileDataModel>().UserProfileData.Username = "hello";
            this.GetModel<UserProfileDataModel>().UserProfileData.Description = "hello 123";
            AppMain.Instance.OpenUserProfileView();
        }

        public void SignOut()
        {
            // clear data
            AppMain.Instance.OpenLoginView();
        }

        public override void OnInit(List<ViewBase> view)
        {
            LoginView = view[0] as LoginView;
            LoginView.signUp.onClick.AddListener(() =>
            {
                AppMain.Instance.OpenSignUpView();
            });
            LoginView.login.onClick.AddListener(Login);
            LoginView.resetPassword.onClick.AddListener(ResetPassword);
            SignUpView = view[1] as SignUpView;
            SignUpView.login.onClick.AddListener(() =>
            {
                AppMain.Instance.OpenLoginView();
            });
            SignUpView.signUp.onClick.AddListener(SignUp);
            UserProfileView = view[2] as UserProfileView;
            UserProfileView.signOut.onClick.AddListener(SignOut);
        }
    }
}