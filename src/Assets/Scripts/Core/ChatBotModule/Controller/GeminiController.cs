using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ChatBotModule.View;
using Core.MVC;

namespace Core.ChatBotModule.Controller
{
    public class GeminiController: ControllerBase
    {
        private ChatBotView _chatBotView;
        public async Task<string> OnChat(string text)
        {
            return null;
        }

        public void ExpressionControl(EEmotion emotion)
        {
            
        }

        public void NewChat(string geminiModelName)
        {
            
        }
        
        public override void OnInit(List<ViewBase> view)
        {
            
        }
    }
}