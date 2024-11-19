
using System;
using System.Collections.Generic;
using Core.FirebaseDatabaseModule.Model;
using Core.UserAccountModule.Controller;
using Core.UserAccountModule.Model;
using Core.UserAccountModule.View;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using QFramework;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace Core.MVC
{
    public class AppMain: MonoSingleton<AppMain>, IController
    {
        public LoginView LoginView;
        public SignUpView SignUpView;
        public UserProfileView UserProfileView;
        public FirebaseApp App;
        // list all view here
        //....
        public SystemMonitorView SystemMonitorView;
        public UserListView UserListView;
        public ModelListView ModelListView;
        public ChatBotView CharBotView;
        public FriendListItemView FriendListItemView;
        public FriendListView FriendListView;
        public MessageView MessageView;


        private ViewBase _currentView;

        public UserProfileController UserProfileController;

        public UserAuthController UserAuthController;
        // list all Controller here
        // ... 
        public SystemInfoController SystemInfoController;
        public UserManagementController UserManagementController;
        public CharacterCustomizationController CharacterCustomizationController;
        public GeminiController GeminiController;
        public FriendManagementController FriendManagementController;
        public MessageController MessageController;



        protected override void Awake()
        {
            // initial all view and controller here.
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available) {
                    App = FirebaseApp.DefaultInstance;
                    Debug.Log("Firebase started");
                    this.GetModel<FirebaseRealTimeDatabaseModel>().InitFirebase();
                    this.GetModel<FirebaseAuthModel>().InitFirebase();
                    this.GetModel<FirebaseStorageModel>().InitFirebase();
                } else {
                    Debug.LogError(System.String.Format(
                        "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                }
            });
            
            UserAuthController = new();
            UserAuthController.OnInit(new List<ViewBase>() 
            {
                LoginView,
                SignUpView,
                UserProfileView
            });
            UserProfileController = new();
            UserProfileController.OnInit(new List<ViewBase>()
            {
                UserProfileView
            });
            
            // 
            SystemInfoController = new();
            SystemInfoController.OnInit(new List<ViewBase>)(
            {
                SystemMonitorView
            });
            UserManagementController = new();
            UserManagementController.OnInit(new List<ViewBase>)(
            {
                UserListView
            });
            CharacterCustomizationController = new();
            CharacterCustomizationController.OnInit(new List<ViewBase>)(
            {
                ModelListView
            });
            GeminiController = new();
            GeminiController.OnInit(new List<ViewBase>)(
            {
                GeminiController
            });
            FriendManagementController = new();
            FriendManagementController.OnInit(new List<ViewBase>)(
            {
                FriendListView
            });
            MessageController = new();
            MessageController.OnInit(new List<ViewBase>)(
            {
                MessageView
            });



            _currentView = LoginView;
            LoginView.Display();
        }
        
        public void OpenSignUpView()
        { 
            _currentView = UserAuthController.OpenSignUpView();
        }

        public void CloseCurrentView()
        {
            _currentView.Hide();
        }
        
        public void OpenUserProfileView()
        {
            _currentView = UserProfileController.OpenUserProfileView();
        }

        public void OpenLoginView()
        {
            _currentView = UserAuthController.OpenLoginView();
        }
        
        // list all open view here
        // ...
        public void OpenSystemMonitorView()
        {
            _currentView = SystemInfoController.OpenSystemMonitorView();
        }

        public void OpenUserListView()
        {
            _currentView = UserManagementController.OpenUserListView();
        }

        public void OpenModelListView()
        {
            _currentView = CharacterCustomizationController.OpenModelListView();
        }

        public void OpenChatBotView()
        {
            _currentView = GeminiController.OpenChatBotView();
        }

        public void OpenFriendListView()
        {
            _currentView = FriendManagementController.OpenFriendListView();
        }

        public void OpenMessageView()
        {
            _currentView = MessageController.OpenMessageView();
        }


        public IArchitecture GetArchitecture()
        {
            return CoreArchitecture.Interface;
        }
    }
}