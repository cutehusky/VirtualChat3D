using System.Collections.Generic;
using Core.AdminModule.Controller;
using Core.AdminModule.View;
using Core.CharacterCustomizationModule.Controller;
using Core.CharacterCustomizationModule.View;
using Core.ChatBotModule.Controller;
using Core.ChatBotModule.View;
using Core.FirebaseDatabaseModule.Model;
using Core.FriendModule.Controller;
using Core.FriendModule.View;
using Core.MessageModule.Controller;
using Core.MessageModule.View;
using Core.OnlineRuntimeModule.RoomManagementModule.Controller;
using Core.OnlineRuntimeModule.RoomManagementModule.View;
using Core.UserAccountModule.Controller;
using Core.UserAccountModule.Model;
using Core.UserAccountModule.View;
using Firebase;
using Firebase.Extensions;
using QFramework;
using UMI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utilities;

namespace Core.MVC
{
    public class AppMain : MonoSingleton<AppMain>, IController
    {
        public FirebaseApp App;
        public LoginView LoginView;
        public SignUpView SignUpView;
        public UserProfileView UserProfileView;
        public SystemMonitorView SystemMonitorView;
        public UserListView UserListView;
        public ModelListView ModelListView;
        public ChatBotView ChatBotView;
        public FriendListView FriendListView;
        public MessageView MessageView;
        public HostRoomView hostRoomView;
        public JoinRoomView joinRoomView;
        
        public ViewBase currentView;

        public UserProfileController UserProfileController;
        public UserAuthController UserAuthController;
        public SystemInfoController SystemInfoController;
        public UserManagementController UserManagementController;
        public CharacterCustomizationController CharacterCustomizationController;
        public GeminiController GeminiController;
        public FriendManagementController FriendManagementController;
        public MessageController MessageController;
        public RoomManager RoomManager;

        public CanvasScaler canvasScaler;
        public RectTransform canvas;

        public void SetHorizontal()
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            canvasScaler.referenceResolution = new Vector2(1920, 1080);
        }

        public void SetVertical()
        {
            Screen.orientation = ScreenOrientation.Portrait;
            canvasScaler.referenceResolution = new Vector2(1080, 1920);
        }

        protected override void Awake()
        {
            MobileInput.Init();
            MobileInput.OnKeyboardAction += OnKeyboardAction;
            MobileInput.OnOrientationChange += OnOrientationChange;
            
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    App = FirebaseApp.DefaultInstance;
                    Debug.Log("Firebase started");
                    this.GetModel<FirebaseRealTimeDatabaseModel>().InitFirebase();
                    this.GetModel<FirebaseAuthModel>().InitFirebase();
                    this.GetModel<FirebaseStorageModel>().InitFirebase();
                    OnInit();
                }  
                else
                {
                    Debug.LogError(System.String.Format(
                        "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                }
            });
        }

        public void OnInit()
        {
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
            SystemInfoController = new();
            SystemInfoController.OnInit(new List<ViewBase>
            {
                SystemMonitorView
            });
            UserManagementController = new();
            UserManagementController.OnInit(new List<ViewBase>
            {
                UserListView
            });
            CharacterCustomizationController = new();
            CharacterCustomizationController.OnInit(new List<ViewBase>
            {
                ModelListView
            });
            GeminiController = new();
            GeminiController.OnInit(new List<ViewBase>
            {
                ChatBotView
            });
            FriendManagementController = new();
            FriendManagementController.OnInit(new List<ViewBase>
            {
                FriendListView
            });
            MessageController = new();
            MessageController.OnInit(new List<ViewBase>
            {
                MessageView
            });
            RoomManager = new();
            RoomManager.OnInit(new List<ViewBase>()
            {
                hostRoomView, joinRoomView
            });

            Firebase.Analytics.FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            Debug.Log("opening signup view");
            OpenSignUpView();
            //OpenChatBotView(); 
            //OpenModelListView();
            ////Invoke("Test", 3); 
            //UnityThread.initUnityThread();
        }

        public void Test() 
        {
            //OpenMessageView("0", "0");
            OpenFriendListView();
        }

        void OnOrientationChange(HardwareOrientation orientation) {
            Debug.Log($"orientation {orientation}");
        }

        private float _keyboardHeight = 0;
        void OnKeyboardAction(bool isShow, int height) {
            Debug.Log($"Keyboard show: {isShow}");
            Debug.Log($"Keyboard height {height}");
            var ratio = (float)Screen.height / canvas.sizeDelta.y / MobileInput.GetScreenScale();
            Debug.Log(ratio);
            _keyboardHeight = height / ratio;
            Debug.Log($"_keyboardHeight {_keyboardHeight}");
            if (currentView)
                currentView.MoveUpWhenOpenKeyboard(_keyboardHeight);
        }
        
        public void OpenSignUpView()
        {
            SetVertical();
            currentView = UserAuthController.OpenSignUpView();
            currentView.MoveUpWhenOpenKeyboard(_keyboardHeight);
        }

        public void OpenHostRoomView()
        {
            SetVertical();
            currentView = RoomManager.OpenHostRoomView();
            currentView.MoveUpWhenOpenKeyboard(_keyboardHeight);
        }

        public void OpenJoinRoomView()
        {
            SetVertical();
            currentView = RoomManager.OpenJoinRoomView();
            currentView.MoveUpWhenOpenKeyboard(_keyboardHeight);
        }
        
        public void CloseCurrentView()
        {
            if (currentView != null)
            {
                currentView.Hide();
                currentView.MoveUpWhenOpenKeyboard(0);
            }
        }

        public void OpenUserProfileView()
        {
            SetVertical();
            currentView = UserProfileController.OpenUserProfileView();
            currentView.MoveUpWhenOpenKeyboard(_keyboardHeight);
        }

        public void OpenLoginView()
        {
            SetVertical();
            currentView = UserAuthController.OpenLoginView();
            currentView.MoveUpWhenOpenKeyboard(_keyboardHeight);
        }

        public void OpenSystemMonitorView()
        {
            SetVertical();
            currentView = SystemInfoController.OpenSystemMonitorView(); 
            currentView.MoveUpWhenOpenKeyboard(_keyboardHeight);
        }

        public void OpenUserListView()
        {
            SetVertical();
            currentView = UserManagementController.OpenUserListView();
            currentView.MoveUpWhenOpenKeyboard(_keyboardHeight);
        }

        public void OpenModelListView()
        {
            SetHorizontal();
            currentView = CharacterCustomizationController.OpenModelListView();
        }

        public void OpenChatBotView()
        {
            SetHorizontal();
            currentView = GeminiController.OpenChatBotView();
        }

        public void OpenFriendListView()
        {
            SetVertical();
            currentView = FriendManagementController.OpenFriendListView();
            currentView.MoveUpWhenOpenKeyboard(_keyboardHeight);
        }

        public void OpenMessageView(string chatSessionId, string friendId)
        {
            SetVertical();
            currentView = MessageController.OpenMessageView(chatSessionId, friendId);
            currentView.MoveUpWhenOpenKeyboard(_keyboardHeight);
        }


        public IArchitecture GetArchitecture()
        {
            return CoreArchitecture.Interface;
        }
    }
}