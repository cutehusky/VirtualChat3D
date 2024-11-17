using System.Collections.Generic;
using Core.MVC;
using Uralstech.UGemini.Models.Content;

namespace Core.ChatBotModule.Model
{
    public class GeminiDataModel: ModelBase
    {
        public List<GeminiContent> History;

        protected override void OnInit()
        {

        }
        
    }
}