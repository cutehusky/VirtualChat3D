using System.Collections.Generic;
using Core.MVC;

namespace Core.MessageModule.Model
{
    public class MessageDataModel: ModelBase
    {
        public Dictionary<string, ChatSession> ChatSessions;
        public string CurrentChatSessionId;
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
                    {
                        new() {UserId = "0", Content = "Hello 1", Time = "00:00:00"},
                        new() {UserId = "1", Content = "Hello 2", Time = "00:00:00"},
                        new() {UserId = "0", Content = "Hello 3", Time = "00:00:00"},
                        new() {UserId = "1", Content = "Hello 4 \n Hello 4 \n Hello 4 \n Hello 4", Time = "00:00:00"},
                        new() {UserId = "0", Content = "Hello 1", Time = "00:00:00"},
                        new() {UserId = "1", Content = "Hello 2", Time = "00:00:00"},
                        new() {UserId = "0", Content = "Hello 3", Time = "00:00:00"},
                        new() {UserId = "1", Content = "Hello 4", Time = "00:00:00"},
                        new() {UserId = "0", Content = "Hello 1", Time = "00:00:00"},
                        new() {UserId = "1", Content = "Hello 2", Time = "00:00:00"},
                        new() {UserId = "0", Content = "Hello 3", Time = "00:00:00"},
                        new() {UserId = "1", Content = "Hello 4 \n Hello 4 \n Hello 4 \n Hello 4", Time = "00:00:00"},
                        new() {UserId = "0", Content = "Hello 1", Time = "00:00:00"},
                        new() {UserId = "1", Content = "Hello 2", Time = "00:00:00"},
                        new() {UserId = "0", Content = "Hello 3", Time = "00:00:00"},
                        new() {UserId = "1", Content = "Hello 4", Time = "00:00:00"},
                    },
                    NewChatData = new List<ChatMessage>()
                    {
                        new() {UserId = "0", Content = "Hello 5 \n Hello 5 Hello 5 \n Hello 5 Hello 5 Hello 5 Hello 5", Time = "00:00:00"},
                        new() {UserId = "1", Content = "Hello 6 \n Hello 6 \n Hello 6 \n Hello 6 \n Hello 6", Time = "00:00:00"},
                        new() {UserId = "0", Content = "Hello 7", Time = "00:00:00"},
                        new() {UserId = "1", Content = "Hello 8", Time = "00:00:00"},  
                        new() {UserId = "0", Content = "Hello 5", Time = "00:00:00"},
                        new() {UserId = "1", Content = "Hello 6", Time = "00:00:00"},
                        new() {UserId = "0", Content = "Hello 7", Time = "00:00:00"},
                        new() {UserId = "1", Content = "Hello 8", Time = "00:00:00"},
                        new() {UserId = "0", Content = "Hello 5 \n Hello 5 Hello 5 \n Hello 5 Hello 5 Hello 5 Hello 5", Time = "00:00:00"},
                        new() {UserId = "1", Content = "Hello 6 \n Hello 6 \n Hello 6 \n Hello 6 \n Hello 6", Time = "00:00:00"},
                        new() {UserId = "0", Content = "Hello 7", Time = "00:00:00"},
                        new() {UserId = "1", Content = "Hello 8", Time = "00:00:00"},  
                        new() {UserId = "0", Content = "Hello 5", Time = "00:00:00"},
                        new() {UserId = "1", Content = "Hello 6", Time = "00:00:00"},
                        new() {UserId = "0", Content = "Hello 7", Time = "00:00:00"},
                        new() {UserId = "1", Content = "Hello 8", Time = "00:00:00"},
                    }
                }},
            };
        }
    }
}