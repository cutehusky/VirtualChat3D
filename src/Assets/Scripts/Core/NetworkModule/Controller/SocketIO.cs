using System;
using System.Collections.Generic;
using Core.MVC;
using Core.NetworkModule.Model;
using Newtonsoft.Json;
using Plugins.EditorExtend.ExecutionOrder.Scripts;
using QFramework;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using SocketIOClient.Transport;
using UnityEngine;
using Utilities;

namespace Core.NetworkModule.Controller
{
    [ExecuteBefore(typeof(AppMain))]
    public class SocketIO: MonoSingleton<SocketIO>, IController
    {
        [JsonObject]
        public class KeyRepPacket
        {
            public string Header;
            public string Value;
        }
        
        private SocketIOUnity _socket;

        private float _sendKeyDeltaTime = 0;
        private bool _isSentAESKey = false;
        
        public string ip;
        public float sendKeyTimeout = 10f;
        
        public void SendWebSocketMessageAES(string eventName, string data)
        {
            if (_socket is not { Connected: true } || !_isSentAESKey)
                return;
            var encrypted = Convert.ToBase64String(
                this.GetModel<EncryptionProvider>().EncryptAES128ECB(data));
            _socket.Emit(eventName, encrypted);
        }
        
        public void SendWebSocketMessage<T>(string eventName, T data)
        {
            if (_socket is not { Connected: true })
                return;
            _socket.Emit(eventName, data);
        }

        private void SendKey()
        {
            if (_socket is not { Connected: true })
                return;
            if (_isSentAESKey)
                return;
            if (_sendKeyDeltaTime > 0)
            {
                _sendKeyDeltaTime -= Time.deltaTime;
                return;
            }
            _socket.Emit("clientAESKey", 
                    this.GetModel<EncryptionProvider>().AESEncrytedKey);
            _sendKeyDeltaTime = sendKeyTimeout;
        }

        protected override void Awake()
        {
            OnInit();
        }
        
        void Update()
        {
            SendKey();
            /*
            // testing
            if (Input.GetMouseButtonDown(0))
            {
                SendWebSocketMessageRSA("messageClient2ServerRSA",
                    "hello from client");
                SendWebSocketMessageAES("messageClient2ServerAES",
                    "hello from client");
            }
            */
        }

        public void AddUnityCallback(string eventName,  Action<SocketIOResponse> callback)
        {
            _socket.OnUnityThread(eventName, callback);
        }
        
        public void AddCallback(string eventName,  Action<SocketIOResponse> callback)
        {
            _socket.On(eventName, callback);
        }
        
        private async void OnDestroy()
        {
            await _socket.DisconnectAsync();
        }
        
        private async void OnApplicationQuit()
        {
            await _socket.DisconnectAsync();
        }

        public async void OnInit()
        {
            this.GetModel<EncryptionProvider>().SetServerKey("<RSAKeyValue><Modulus>iDk6uA51M3XpWp/6rbjPMrJUqGPZXowRJKODTxx1iTFZUi/IgGWL3el1QrI72c8lMS/gAsadUlDw8n6cch8kMQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
            
            var uri = new Uri("http://"+ ip + ":3000");
            _socket = new SocketIOUnity(uri, new SocketIOOptions
            {
                Query = new Dictionary<string, string>
                {
                    {"token", "UNITY" }
                }
                ,
                Transport = TransportProtocol.WebSocket,
            });
            
            _socket.JsonSerializer = new NewtonsoftJsonSerializer();
            _socket.unityThreadScope = SocketIOUnity.UnityThreadScope.Update; 
            
            _socket.OnUnityThread("clientAESKeyRep", (response) =>
            {
                var obj = response.GetValue<KeyRepPacket>();
                var data = this.GetModel<EncryptionProvider>().DecryptAES128ECB(Convert.FromBase64String(obj.Value));
                if (data == "hello from server")
                {
                    _isSentAESKey = true;
                    Debug.Log($"{DateTime.Now} AES rep: {data}");
                } 
            });
            
            /*
            // testing
            socket.OnUnityThread("messageServer2ClientAES", (response) =>
            {
                var obj = response.GetValue<KeyRepPacket>();
                var data = this.GetModel<EncryptionProvider>().DecryptAES128ECB(Convert.FromBase64String(obj.Value));
                Debug.Log("AES: " + data);
            });
            */
            
            _socket.OnConnected += async (sender, e) =>
            {
                Debug.Log($"{DateTime.Now} Connected");
                _isSentAESKey = false;
                _sendKeyDeltaTime = 0;
            };
            
            _socket.OnDisconnected += async (sender, e) =>
            {
                Debug.Log($"{DateTime.Now} Disconnected: reason = {e}");
                _isSentAESKey = false;
                _sendKeyDeltaTime = 0;
            };
            
            _socket.OnError += async (sender, e) =>
            {
                Debug.Log($"{DateTime.Now} Error: reason = {e}");
                _isSentAESKey = false;
                _sendKeyDeltaTime = 0;
            };
            
            _socket.OnReconnectAttempt += (sender, e) =>
            {
                Debug.Log($"{DateTime.Now} Reconnecting: attempt = {e}");
                _isSentAESKey = false;
                _sendKeyDeltaTime = 0;
            };
            
            _socket.OnReconnectError += (sender, exception) =>
            {
                Debug.Log($"{DateTime.Now} Reconnect Error: reason = {exception}");
                _isSentAESKey = false;
                _sendKeyDeltaTime = 0;
            };
            
            _socket.OnReconnectFailed += (sender, exception) =>
            {
                Debug.Log($"{DateTime.Now} Reconnect Failed: reason = {exception}");
                _isSentAESKey = false;
                _sendKeyDeltaTime = 0;
            };
            
            _socket.OnReconnected += (sender, e) =>
            {
                Debug.Log($"{DateTime.Now} Reconnected: attempt = {e}");
                _isSentAESKey = false;
                _sendKeyDeltaTime = 0;
            };
            await _socket.ConnectAsync();
        }

        public IArchitecture GetArchitecture()
        {
            return CoreArchitecture.Interface;
        }
    }
}