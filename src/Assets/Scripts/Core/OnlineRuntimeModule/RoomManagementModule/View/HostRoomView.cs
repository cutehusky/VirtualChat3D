using System;
using System.Collections.Generic;
using Core.MVC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.OnlineRuntimeModule.RoomManagementModule.View
{
    public class HostRoomView: ViewBase
    {
        public TMP_InputField ip;
        public TMP_InputField port;
        public Toggle accessType;
        public Button create;
        
        public RectTransform scrollViewParent;
        public GameObject listItemTemplate;
        public List<RoomListItem> items; 
        
        public override void Render(ModelBase model)
        {
            
        }

        public override void OnInit()
        {
            
        }
    }
}