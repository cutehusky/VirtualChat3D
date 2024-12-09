using System.Collections.Generic;
using Core.MVC;

namespace Core.AdminModule.Model
{
    public class Country
    {
        public string name;
        public int activeUser;
    }
    public class SystemInfoDataModel: ModelBase
    {
        public int OnlineUserCount;
        public string Cpu;
        public int CpuSpeed;
        public int Ram;
        public List<Country> Countries;
        protected override void OnInit()
        {
            Countries = new List<Country>();
        }
    }
}