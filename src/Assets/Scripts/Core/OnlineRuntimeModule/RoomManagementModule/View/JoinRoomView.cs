using System;
using Core.MVC;
using TMPro;
using UnityEngine.UI;

namespace Core.OnlineRuntimeModule.RoomManagementModule.View
{
    public class JoinRoomView: ViewBase
    {
        public TMP_InputField ip;
        public TMP_InputField port;
        public Button join;
        public override void Render(ModelBase model)
        {
            ip.text = "";
            port.text = "";
        }

        public override void OnInit()
        {
        }
    }
}