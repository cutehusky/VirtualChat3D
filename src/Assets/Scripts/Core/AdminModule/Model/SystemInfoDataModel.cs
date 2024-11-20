using System.Collections.Generic;
using Core.MVC;

namespace Core.AdminModule.Model
{
    public class SystemInfoDataModel: ModelBase
    {
        public List<string> Logs;
        public int OnlineUserCount;
        public string ServerStatus;
        protected override void OnInit()
        {
            
        }
    }
}