using System.Collections.Generic;
using Core.MVC;
using Uralstech.UGemini.Models.Content;

namespace Core.ChatBotModule.Model
{
    public class GeminiDataModel: ModelBase
    {
        public List<GeminiContent> History;
        public string CurrentGeminiModelName;

        protected override void OnInit()
        {

        }
        
    }
}