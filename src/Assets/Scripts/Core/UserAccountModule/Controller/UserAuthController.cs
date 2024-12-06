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
        //private string GetFriendlyErrorMessage(AggregateException exception)
        //{
        //    foreach (var innerException in exception.Flatten().InnerExceptions)
        //    {
        //        if (innerException is FirebaseException firebaseEx)
        //        {
        //            switch (firebaseEx.ErrorCode)
        //            {
        //                case AuthErrorCodes.EmailAlreadyInUse:
        //                    return "Email is already in use.";
        //                case AuthErrorCodes.InvalidEmail:
        //                    return "Invalid email format.";
        //                case AuthErrorCodes.WeakPassword:
        //                    return "Password is too weak.";
        //                case AuthErrorCodes.NetworkRequestFailed:
        //                    return "Network error, please try again.";
        //                default:
        //                    return "Unexpected error occurred.";
        //            }
        //        }
        //    }
        //    return "An unknown error occurred.";
        //}

        private string ConvertException(string exception)
        {
            if (exception == null)
            {
                return null;
            }

            if (exception.Contains("The user account has been disabled by an administrator"))
            {
                return "Your account has been disabled by an administrator!";
            }

            if (exception.Contains("The given password is invalid"))
            {
                return "The given password is invalid";
            }
            if (exception.Contains("The email address is already in use by another account"))
            {
                return "The email address is already in use by another account.";
            }
            if (exception.Contains("An internal error has occurred"))
            {
                return "Wrong Email or Password";
            }
            if (exception.Contains("We have blocked all requests from this device due to unusual activity. Try again later."))
            {
                return "We have blocked all requests from this device due to unusual activity. Try again later.";
            }

            return exception;  // Debug for more errors in the future.
        }


        public void SignUp()
        {
            if (_signUpView.password.Text != _signUpView.re_password.Text)
            {
                _signUpView.SetNotice("The password confirm is not the same with the password");
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
                    _signUpView.SetNotice(task.Exception != null
                        ? ConvertException(task.Exception.Message.ToString())
                        : "An unknown error occurred.");
                    //Debug.Log(task.Exception.Message.ToString());
                    //Debug.Log(task.Exception.ToString());
                    return;
                }
                _signUpView.SetNotice("Firebase user created successfully");
                SocketIO.Instance.SendWebSocketMessage("createUser", new SignUpReqPacket() { uid = task.Result.User.UserId });
            });
        }
        
        public void ResetPassword()
        {
            Debug.Log($"reset password with email: {_loginView.email.Text}");
        }

        
        public float SendTokenDeltaTime = 0;
        public readonly float SendTokenTimeout = 3;
        public override void Update()
        {
            if (this.GetModel<FirebaseAuthModel>().IsSentToken 
                || this.GetModel<UserProfileDataModel>().UserProfileData.UserId == "")
                return;
            if (SendTokenDeltaTime > 0)
            {
                SendTokenDeltaTime -= Time.deltaTime;
                return;
            }
            SocketIO.Instance.SendWebSocketMessage("fetch", new UserVerifyPacket()
            {
                uid = this.GetModel<UserProfileDataModel>().UserProfileData.UserId
            }); 
            SendTokenDeltaTime = SendTokenTimeout;
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
                    _loginView.SetNotice(task.Exception != null
                        ? ConvertException(task.Exception.Message.ToString())
                        : "An unknown error occurred.");
                    //Debug.Log(task.Exception.InnerException);
                    //Debug.Log(task.Exception.ToString());
                    return; 
                }
                _loginView.SetNotice("Firebase user signed in successfully");
                this.GetModel<FirebaseAuthModel>().IsSentToken = false;
                SendTokenDeltaTime = 0;
                this.GetModel<UserProfileDataModel>().FetchProfile(this.GetModel<FirebaseAuthModel>().Auth, 
                    () => { AppMain.Instance.OpenUserProfileView(); }, () => { });
            });
        }

        public void SignOut()
        {
            // clear data
            this.GetModel<UserProfileDataModel>().UserProfileData = new();
            this.GetModel<FirebaseAuthModel>().IsSentToken = false;
            SendTokenDeltaTime = 0;
            SocketIO.Instance.SendWebSocketMessage("clear", new AdminReqPacket()
            {
                uid = this.GetModel<UserProfileDataModel>().UserProfileData.UserId
            });
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
            SocketIO.Instance.AddUnityCallback("fetchReply", (res) =>
            {
                var data = res.GetValue<UserVerifyPacket>();
                if (data.uid == this.GetModel<UserProfileDataModel>().UserProfileData.UserId)
                {
                    this.GetModel<FirebaseAuthModel>().IsSentToken = true;
                    return;
                }
                SignOut();
            });
            SocketIO.Instance.AddUnityCallback("logout", (res) =>
            {
                SignOut();
            });
        }
    }
}