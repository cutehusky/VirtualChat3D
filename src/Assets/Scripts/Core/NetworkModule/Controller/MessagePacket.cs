using Newtonsoft.Json;

namespace Core.NetworkModule.Controller
{
    [JsonObject]
    public class MessagePacket
    {
        public string uid;
        public string fid;
        public string id_cons;
        public string msg;
        public long timestamp;
    }
}