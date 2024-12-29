using System.Collections.Generic;
using Core.AdminModule.Model;
using Core.AdminModule.View;
using Core.MVC;
using Core.NetworkModule.Controller;
using Core.UserAccountModule.Model;
using Newtonsoft.Json;
using QFramework;
using UnityEngine.Splines;

namespace Core.AdminModule.Controller
{
    [JsonObject]
    class AnalyticData
    {
        public string country;
        public int activeUsers;
    };

    [JsonObject]
    class SystemData
    {
        public int active_user;
        public string cpu;
        public long cpu_speed;
        public long ram;
    }
    public class SystemInfoController: ControllerBase
    {
        private SystemMonitorView _systemMonitorView;
        public void LoadSystemInfo()
        {
            SocketIO.Instance.SendWebSocketMessage("viewSystemInfo", new AdminReqPacket());
        }
        
        public override void OnInit(List<ViewBase> view)
        {
            base.OnInit(view);
            _systemMonitorView = view[0] as SystemMonitorView;
            SocketIO.Instance.AddUnityCallback("system", (res) =>
            {
                var sysPackage = res.GetValue<SystemData>();
                this.GetModel<SystemInfoDataModel>().Cpu = sysPackage.cpu;
                this.GetModel<SystemInfoDataModel>().Ram = sysPackage.ram;
                this.GetModel<SystemInfoDataModel>().CpuSpeed = sysPackage.cpu_speed;
                this.GetModel<SystemInfoDataModel>().OnlineUserCount = sysPackage.active_user;
                _systemMonitorView.Render(this.GetModel<SystemInfoDataModel>());
            });
            SocketIO.Instance.AddUnityCallback("analytic", (res) =>
            {
                var anltPackage = res.GetValue<AnalyticData[]>();
                foreach (var data in anltPackage)
                {
                    this.GetModel<SystemInfoDataModel>().Countries.Add(new Country()
                    {
                        activeUsers = data.activeUsers,
                        name = data.country
                    });
                }
                _systemMonitorView.RefreshList();
            });
        }

        public ViewBase OpenSystemMonitorView()
        {
            AppMain.Instance.CloseCurrentView();
            LoadSystemInfo();
            _systemMonitorView.Display(this.GetModel<UserProfileDataModel>().UserProfileData.IsAdmin);
            _systemMonitorView.Render(this.GetModel<SystemInfoDataModel>());
            return _systemMonitorView;
        }
    }
}