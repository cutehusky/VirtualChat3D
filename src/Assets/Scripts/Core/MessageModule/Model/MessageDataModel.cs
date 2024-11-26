using System.Collections.Generic;
using Core.MVC;

namespace Core.MessageModule.Model
{
    public class MessageDataModel: ModelBase
    {
        public ChatSession CurrentChatSession;

        protected override void OnInit()
        {
            CurrentChatSession = new();
        }
    }
}