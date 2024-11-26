using System.Collections;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.Core.NetworkModule.Controller
{
    [JsonObject]
    public class FriendPacket 
    {
        public string uid;
        public string fid;
    }
}