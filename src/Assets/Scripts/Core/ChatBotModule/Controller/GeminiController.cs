using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ChatBotModule.View;
using Core.MVC;

namespace Core.ChatBotModule.Controller
{
    public class GeminiController: ControllerBase
    {
        private ChatBotView _chatBotView;
        private ExpressionControl _chatBotExpressionControl;
        public async Task<string> OnChat(string text)
        {
            return null;
        }

        public void ExpressionControl(EEmotion emotion)
        {
            
        }

        public void NewChat(string geminiModelName, string avatarId)
        {
            
        }

        public void EndChat()
        {
            
        }
        
        public override void OnInit(List<ViewBase> view)
        {
            _chatBotView = view[0] as ChatBotView;
        }

        public ViewBase OpenChatBotView()
        {
            // CODE HERE
            return null;
        }
    }
}