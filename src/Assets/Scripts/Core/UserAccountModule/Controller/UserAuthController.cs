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
            this.GetModel<FirebaseAuthModel>().Auth.CreateUserWithEmailAndPasswordAsync(_signUpView.email.text, _signUpView.password.text).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    UnityThread.executeInUpdate(() =>
                    {
                        _signUpView.SetNotice("Sign up was canceled by Firebase.");
                    });

                    return;
                }
                if (task.IsFaulted)
                {
                    UnityThread.executeInUpdate(() =>
                    {
                        _signUpView.SetNotice("Sign up encountered a Firebase error: " + task.Exception);
                    });
                    return;
                }
                UnityThread.executeInUpdate(()=>
                {
                    _signUpView.SetNotice("Firebase user created successfully");
                });
            });
        }

        public void ResetPassword()
        {
            Debug.Log($"reset password with email: {_loginView.email.text}");

        }
        
        public void Login()
        {
            Debug.Log($"Login with email: {_loginView.email.text} pass: {_loginView.password.text}");
            this.GetModel<FirebaseAuthModel>().Auth.SignInWithEmailAndPasswordAsync(_loginView.email.text, _loginView.password.text).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    UnityThread.executeInUpdate(() =>
                    {
                        _loginView.SetNotice("Login was canceled by Firebase.");
                    });
                    return;
                }
                if (task.IsFaulted)
                {
                    UnityThread.executeInUpdate(() =>
                    {
                        _loginView.SetNotice("Login encountered a Firebase error: " + task.Exception);
                    });
                    return;
                }
                UnityThread.executeInUpdate(() =>
                {
                    _loginView.SetNotice("Firebase user signed in successfully");
                    AppMain.Instance.OpenUserProfileView();
                });
            });
        }

        public void SignOut()
        {
            // clear data
            AppMain.Instance.OpenLoginView();
        }

        public override void OnInit(List<ViewBase> view)
        {
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