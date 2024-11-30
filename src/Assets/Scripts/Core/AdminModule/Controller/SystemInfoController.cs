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
            //base.OnInit(view);
            _systemMonitorView = view[0] as SystemMonitorView;
        }

        public ViewBase OpenSystemMonitorView()
        {
            // CODE HERE
            return null;
        }
    }
}