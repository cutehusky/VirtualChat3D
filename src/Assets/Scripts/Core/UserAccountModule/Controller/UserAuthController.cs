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

        string notifyContent;

        public void SignUp()
        {
            this.GetModel<FirebaseAuthModel>().Auth.CreateUserWithEmailAndPasswordAsync(SignUpView.email.text, SignUpView.password.text).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    notifyContent = "Sign up was canceled by Firebase.";
                    UnityThread.executeInUpdate(() =>
                    {
                        Debug.Log("test");
                        SignUpView.SetNotice(notifyContent);
                    });

                    return;
                }
                if (task.IsFaulted)
                {
                    
                    UnityThread.executeInUpdate(() =>
                    {
                        Debug.Log("test");
                        SignUpView.SetNotice("Sign up encountered a Firebase error: " + task.Exception);
                    });

                    return;
                }
                UnityThread.executeInUpdate(()=>
                {
                    Debug.Log("test");
                    notifyContent = "Sign up successed by Firebase.";

                    SignUpView.SetNotice(notifyContent);
                });
                //SignUpView.SetNotice("Firebase user created successfully");
            });
        }

        public void Update()
        {
            SignUpView.SetNotice(notifyContent);
        }

        public void ResetPassword()
        {
            Debug.Log($"reset password with email: {LoginView.email.text}");

        }
        
        public void Login()
        {
            Debug.Log($"Login with email: {LoginView.email.text} pass: {LoginView.password.text}");
            this.GetModel<FirebaseAuthModel>().Auth.SignInWithEmailAndPasswordAsync(LoginView.email.text, LoginView.password.text).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    UnityThread.executeInUpdate(() =>
                    {
                        //LoginView.SetNotice("Login was canceled by Firebase.");
                    });
                    return;
                }
                if (task.IsFaulted)
                {
                    UnityThread.executeInUpdate(() =>
                    {
                        //LoginView.SetNotice("Login encountered a Firebase error: " + task.Exception);
                        Debug.Log("Login encountered a Firebase error: " + task.Exception);
                    });
                    return;
                }
                UnityThread.executeInUpdate(() =>
                {
                    //LoginView.SetNotice("Firebase user signed in successfully");
                    Debug.Log("Firebase user signed in successfully");
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