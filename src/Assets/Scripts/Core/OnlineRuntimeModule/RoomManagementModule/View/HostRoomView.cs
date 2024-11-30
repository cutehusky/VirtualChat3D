using System;
using System.Collections.Generic;
using Core.MVC;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Core.OnlineRuntimeModule.RoomManagementModule.View
{
    public class HostRoomView: ViewBase
    {
        public TMP_InputField ip;
        public TMP_InputField port;
        public Button host;
        
        public override void Render(ModelBase model)
        {
            ip.text = "localhost";
            port.text = "8888";
        }

        public override void OnInit()
        {
            
        }
    }
}