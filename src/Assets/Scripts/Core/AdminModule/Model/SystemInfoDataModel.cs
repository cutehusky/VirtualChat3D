using System.Collections.Generic;
using Core.MVC;

namespace Core.AdminModule.Model
{
    public class Country
    {
        public string name;
        public int activeUsers;
    }
    public class SystemInfoDataModel: ModelBase
    {
        public int OnlineUserCount;
        public string Cpu;
        public long CpuSpeed;
        public long Ram;
        public List<Country> Countries;
        protected override void OnInit()
        {
            Countries = new List<Country>();
        }
    }
}