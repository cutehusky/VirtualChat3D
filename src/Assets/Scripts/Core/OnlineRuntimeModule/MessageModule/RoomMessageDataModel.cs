using Core.MessageModule.Model;
using Core.MVC;

namespace Core.OnlineRuntimeModule.MessageModule
{
    public class RoomMessageDataModel: ModelBase
    {
        public ChatSession ChatSession;

        protected override void OnInit()
        {
            ChatSession = new();
        }
    }
}