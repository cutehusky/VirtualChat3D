using System.Collections.Generic;
using Core.MVC;

namespace Core.MessageModule.Model
{
    public class MessageDataModel: ModelBase
    {
        public Dictionary<string, ChatSession> ChatSessions;
        public string CurrentChatSessionId = "0"; // for test
        public string CurrentChatFriendId = "0"; // for test
        public ChatSession CurrentChatSession
        {
            get
            {
                if (ChatSessions.TryGetValue(CurrentChatSessionId, out var res))
                    return res;
                return null;
            }
        }

        protected override void OnInit()
        {
            CurrentChatSessionId = "0";
            ChatSessions = new Dictionary<string, ChatSession>()
            {
                { "0", new ChatSession()
                {
                    ChatData = new List<ChatMessage>()
                }},
            };
        }
    }
}