using Newtonsoft.Json;

namespace Core.NetworkModule.Controller
{
    [JsonObject]
    public class SendMessagePacket
    {
        public string uid;
        public string fid;
        public string id_cons;
        public string msg;
    }
}