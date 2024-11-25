using System.Collections.Generic;

namespace Core.MessageModule.Model
{
    public class ChatSession
    {
        public List<ChatMessage> ChatData = new();
        public string FriendId = "0";
        public string ChatSessionId = "0";
    }
}