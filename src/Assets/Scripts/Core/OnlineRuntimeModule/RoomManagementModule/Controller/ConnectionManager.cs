using System;
using System.Collections.Generic;
using Core.MessageModule.Model;
using Core.MVC;
using Core.OnlineRuntimeModule.CharacterControl;
using Core.OnlineRuntimeModule.InputModule.Model;
using Core.OnlineRuntimeModule.MessageModule;
using Core.OnlineRuntimeModule.RoomManagementModule.Model;
using Core.OnlineRuntimeModule.RoomManagementModule.View;
using Core.UserAccountModule.Model;
using QFramework;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.OnlineRuntimeModule.RoomManagementModule.Controller
{
    public class ConnectionManager: ControllerBase
    {
        private HostRoomView _hostRoomView;
        private JoinedUserListView _userListView;
        private JoinRoomView _joinRoomView;
        private CharacterControlView _characterControlView;
        private JoinedUserListView _joinedUserListView;
        private RoomMessageView _roomMessageView;

        public void ClientConnect(string ip, ushort port)
        {
            AppMain.Instance.unityTransport.ConnectionData.Address = ip;
            AppMain.Instance.unityTransport.ConnectionData.Port = port;
            NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.UTF8.GetBytes(
                this.GetModel<UserProfileDataModel>().UserProfileData.UserId);
            NetworkManager.Singleton.StartClient();
        }

        public void Host(string ip, ushort port)
        {
            AppMain.Instance.unityTransport.ConnectionData.Address = ip;
            AppMain.Instance.unityTransport.ConnectionData.Port = port;
            NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.UTF8.GetBytes(
                this.GetModel<UserProfileDataModel>().UserProfileData.UserId);
            NetworkManager.Singleton.StartHost();
        }

        public void KickUser(ulong id)
        {
            NetworkManager.Singleton.DisconnectClient(id, "You are kicked !!!");
        }

        public void Disconnect()
        {
            Debug.Log("disconnected");
            if (NetworkManager.Singleton.IsClient) 
                NetworkManager.Singleton.Shutdown();
        }

        public ChatSession ChatSession;

        public override void OnInit(List<ViewBase> view)
        {
            ChatSession = new();
            _hostRoomView = view[0] as HostRoomView;
            _joinRoomView = view[1] as JoinRoomView;
            _characterControlView = view[2] as CharacterControlView;
            _joinedUserListView = view[3] as JoinedUserListView;
            _roomMessageView = view[4] as RoomMessageView;
            _joinedUserListView.OnKick += KickUser;
            _characterControlView.openUserList.onClick.AddListener(() =>
            {
                if (!NetworkManager.Singleton.IsHost)
                    return;
                if (!_joinedUserListView.gameObject.activeSelf)
                    _joinedUserListView.Display();
                else
                    _joinedUserListView.Hide();
            });
            _characterControlView.openChat.onClick.AddListener(() =>
            {
                this.GetModel<PlayerInputAction>()
                    .TriggerEvent(!_roomMessageView.gameObject.activeSelf ? "OpenChatView" : "CloseChatView");
            });
            _characterControlView.outRoom.onClick.AddListener(Disconnect); 
            _hostRoomView.host.onClick.AddListener(() =>
            {
                AppMain.Instance.OpenCharacterControlView();
                this.GetModel<RoomDataModel>().CurrentHostRoomJoinedUser.Clear();
                Host(_hostRoomView.ip.Text, Convert.ToUInt16(_hostRoomView.port.Text));
            });
            _joinRoomView.join.onClick.AddListener(() =>
            {
                AppMain.Instance.OpenCharacterControlView();
                ClientConnect(_joinRoomView.ip.Text, Convert.ToUInt16(_joinRoomView.port.Text));
            });
            NetworkManager.Singleton.OnServerStarted += OnServerStarted;
            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
            NetworkManager.Singleton.OnServerStopped += OnServerStopped;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientStopped += OnClientStopped;
            NetworkManager.Singleton.NetworkConfig.ConnectionApproval = true;
        }

        private void OnClientConnected(ulong clientId)
        {
            _joinedUserListView.RefreshList();
        }

        private void OnClientStopped(bool isHost)
        {
            AppMain.Instance.OpenHostRoomView();
        }
        
        private void OnClientDisconnected(ulong clientId)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                this.GetModel<RoomDataModel>().RemoveUserFromList(clientId);
                _joinedUserListView.RefreshList();
                Debug.Log($"user with uid: {clientId} disconnected");
            }
            /*
            Debug.Log($"user with uid: {clientId} disconnected");
            Debug.Log($"user with uid: {NetworkManager.Singleton.LocalClientId} disconnected");
            if (clientId == 0 || clientId == NetworkManager.Singleton.LocalClientId)
                AppMain.Instance.OpenHostRoomView();
                */
        }
        
        private void OnServerStopped(bool isHost)
        { 
        }
        
        private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, 
            NetworkManager.ConnectionApprovalResponse response)
        {
            var clientData = request.Payload;
            var clientId = request.ClientNetworkId;
            if (clientData.Length > 0 )
            {
                var uid = System.Text.Encoding.UTF8.GetString(clientData);
                Debug.Log($"user with uid: {uid} joined");
                this.GetModel<RoomDataModel>().CurrentHostRoomJoinedUser.Add(
                    clientId, new UserAccountData()
                    {
                        UserId = uid,
                        Username = uid
                    });
                response.CreatePlayerObject = true;
                response.Approved = true; // for testing
            }
        }
        
        private void OnServerStarted()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                
            }
        }
    }
}