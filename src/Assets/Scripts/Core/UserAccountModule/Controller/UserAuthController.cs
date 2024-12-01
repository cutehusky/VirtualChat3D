using System;
using System.Collections.Generic;
using Core.MVC;
using Core.NetworkModule.Controller;
using Core.UserAccountModule.Model;
using Core.UserAccountModule.View;
using Firebase.Extensions;
using Newtonsoft.Json;
using QFramework;
using UnityEngine;

namespace Core.UserAccountModule.Controller
{
    public class UserAuthController: ControllerBase
    {
        private LoginView _loginView;
        private SignUpView _signUpView;
        private UserProfileView _userProfileView;
        
        public ViewBase OpenSignUpView()
        {
            AppMain.Instance.CloseCurrentView();
            _signUpView.Display();
            _signUpView.Render(null);
            return _signUpView;
        }

        public ViewBase OpenLoginView()
        {
            AppMain.Instance.CloseCurrentView();
            _loginView.Display();
            _loginView.Render(null);
            return _loginView;
        }

        public void SignUp()
        {
            if (_signUpView.password.Text != _signUpView.re_password.Text)
            {
                return;
            }
            this.GetModel<FirebaseAuthModel>().Auth.CreateUserWithEmailAndPasswordAsync(_signUpView.email.Text, _signUpView.password.Text).ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    _signUpView.SetNotice("Sign up was canceled by Firebase.");
                    return;
                }
                if (task.IsFaulted)
                {
                    _signUpView.SetNotice("Sign up encountered a Firebase error: " + task.Exception.Message);
                    return;
                }
                _signUpView.SetNotice("Firebase user created successfully");
                SocketIO.Instance.SendWebSocketMessage("createUser", new signupPacket() { uid = task.Result.User.UserId });
            });
        }

        [JsonObject]
        public class signupPacket
        {
            public string uid;
        }
        public void ResetPassword()
        {
            Debug.Log($"reset password with email: {_loginView.email.Text}");

        }
        
        public void Login()
        {
            Debug.Log($"Login with email: {_loginView.email.Text} pass: {_loginView.password.Text}");
            this.GetModel<FirebaseAuthModel>().Auth.SignInWithEmailAndPasswordAsync(
                _loginView.email.Text, 
                _loginView.password.Text).ContinueWithOnMainThread(task =>
            { 
                if (task.IsCanceled)
                {
                    _loginView.SetNotice("Login was canceled by Firebase.");
                    return;
                }
                if (task.IsFaulted)
                {
                    _loginView.SetNotice("Login encountered a Firebase error: " + task.Exception);
                    return;
                }
                _loginView.SetNotice("Firebase user signed in successfully");
                Firebase.Auth.AuthResult result = task.Result;
                this.GetModel<UserProfileDataModel>().FetchProfile(this.GetModel<FirebaseAuthModel>().Auth, 
                    () => { AppMain.Instance.OpenUserProfileView(); }, () => { });
            });
        }

        public void SignOut()
        {
            // clear data
            AppMain.Instance.OpenLoginView();
        }

        public override void OnInit(List<ViewBase> view)
        {
            base.OnInit(view);
            _loginView = view[0] as LoginView;
            _loginView.signUp.onClick.AddListener(() =>
            {
                AppMain.Instance.OpenSignUpView();
            });
            _loginView.login.onClick.AddListener(Login);
            _loginView.resetPassword.onClick.AddListener(ResetPassword);
            _signUpView = view[1] as SignUpView;
            _signUpView.login.onClick.AddListener(() =>
            {
                AppMain.Instance.OpenLoginView();
            });
            _signUpView.signUp.onClick.AddListener(SignUp);
            _userProfileView = view[2] as UserProfileView;
            _userProfileView.signOut.onClick.AddListener(SignOut);
        }
    }
}