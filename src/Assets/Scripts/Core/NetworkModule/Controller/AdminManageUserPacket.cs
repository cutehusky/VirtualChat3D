using System.Collections;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.Core.NetworkModule.Controller
{
    [JsonObject]
    public class AdminManageUserPacket
    {
        public string uid;
    }
}