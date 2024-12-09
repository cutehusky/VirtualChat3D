using System;
using System.Collections.Generic;
using Core.MVC;
using TMPro;

namespace Core.AdminModule.View
{
    public class SystemMonitorView: ViewBase
    {
        public TMP_Text OnlineUserCount;
        public TMP_Text CpuSpeed;
        public TMP_Text Cpu;
        public TMP_Text Ram;

        public override void Render(ModelBase model)
        {
            
        }

        public override void OnInit()
        {
            
        }
    }
}