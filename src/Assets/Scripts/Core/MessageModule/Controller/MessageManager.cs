using Core.MessageModule.Model;
using Core.MVC;
using QFramework;

namespace Core.MessageModule.Controller
{
    public class MessageManager: MonoSingletonControllerBase
    {
        /// <summary>
        ///  Receive new message from server
        /// </summary>
        public void ReceiveNewMessage()
        {
            
        }

        /// <summary>
        /// Mark all new message as read
        /// </summary>
        /// <param name="userId"></param>
        public void ReadMessageOfUser(string userId)
        {
            
        }
        
        /// <summary>
        /// Load all message from server
        /// </summary>
        public void LoadMessage()
        {
            
        }

        public bool IsHaveNewMessageFromUser(string userId)
        {
            if (this.GetModel<MessageDataModel>().ChatSessions.TryGetValue(userId, out var chatSession))
                return chatSession.NewChatData.Count > 0;
            return false;
        }

        public void SendMessage(string userId, string data)
        {
            
        }
        
        public override void OnInit()
        {
            
        }
    }
}