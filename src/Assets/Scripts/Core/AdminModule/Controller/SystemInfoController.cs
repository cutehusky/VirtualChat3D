using System.Collections.Generic;
using Core.AdminModule.View;
using Core.MVC;

namespace Core.AdminModule.Controller
{
    public class SystemInfoController: ControllerBase
    {
        private SystemMonitorView _systemMonitorView;
        public void LoadSystemInfo()
        {
            
        }
        
        public override void OnInit(List<ViewBase> view)
        {
            _systemMonitorView = view[0] as SystemMonitorView;
        }

        public void OpenSystemMonitorView()
        {
            // CODE HERE
        }
    }
}