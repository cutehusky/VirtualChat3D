using Newtonsoft.Json;

namespace Core.NetworkModule.Controller
{
    [JsonObject]
    public class UserVerifyPacket
    {
        public string uid;
        public string token;
    }
}