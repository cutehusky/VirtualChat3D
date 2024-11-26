using Newtonsoft.Json;

namespace Core.NetworkModule.Controller
{
    [JsonObject]
    public class ChatSessionPacket
    {
        public string id_cons;
        public string uid;
    }
}