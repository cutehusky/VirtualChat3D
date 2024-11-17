using System.Collections.Generic;
using Core.MVC;

namespace Core.MessageModule.Model
{
    public class MessageDataModel: ModelBase
    {
        public Dictionary<string, ChatSession> ChatSessions = new();
        protected override void OnInit()
        {
            
        }
    }
}