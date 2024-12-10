using System;
using Core.MVC;
using TMPro;
using UMI;
using UnityEngine.UI;

namespace Core.OnlineRuntimeModule.RoomManagementModule.View
{
    public class JoinRoomView: ViewBase
    {
        public MobileInputField ip;
        public MobileInputField port;
        public TMP_InputField TMP_ip;
        public TMP_InputField TMP_port;
        public Button join;
        public override void Render(ModelBase model)
        {
            ip.Text = "";
            port.Text = "";
#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IOS)
            TMP_ip.text = "localhost";
            TMP_port.text = "8888";
#endif
        }

        public override void OnInit()
        {
        }
    }
}