using Core.MessageModule.Model;
using Core.MVC;
using QFramework;

namespace Core.MessageModule.Controller
{
    public class MessageController: ControllerBase
    {
        /// <summary>
        /// Mark all new message as read
        /// </summary>
        /// <param name="userId"></param>
        public void ReadMessageOfUser(string userId)
        {
            
        }

        public void SendMessage(string userId, string data)
        {
            
        }
        
        /// <summary>
        ///  Receive new message from server
        /// </summary>
        public void ReceiveNewMessage()
        {
            
        }
        
        public bool IsHaveNewMessageFromUser(string userId)
        {
            if (this.GetModel<MessageDataModel>().ChatSessions.TryGetValue(userId, out var chatSession))
                return chatSession.NewChatData.Count > 0;
            return false;
        }
        
           
        /// <summary>
        /// Load all message from server
        /// </summary>
        public void LoadMessage()
        {
            
        }


        public override void OnInit(ViewBase view)
        {
            
        }
    }
}