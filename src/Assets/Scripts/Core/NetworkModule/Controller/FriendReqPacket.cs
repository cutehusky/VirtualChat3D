using Newtonsoft.Json;

namespace Core.NetworkModule.Controller
{
    [JsonObject]
    public class FriendReqPacket 
    {
        public string uid;
        public string fid;
    }
}