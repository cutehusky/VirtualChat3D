using Newtonsoft.Json;

namespace Core.NetworkModule.Controller
{
    [JsonObject]
    public class UserDataPacket
    {
        public string uid;
        public string id_cons;
        public string description;
        public long birthday;
        public string username;
        public bool status;
    }
}