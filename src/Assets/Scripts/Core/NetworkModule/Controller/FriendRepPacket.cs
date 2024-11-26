using System.Collections;
using JimmysUnityUtilities;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.Core.NetworkModule.Controller
{
    [JsonObject]
    public class FriendRepPacket
    {
        public string uid;
        public string id_cons;
        public string description;
        public long birthday;
        public string username;
    }
}