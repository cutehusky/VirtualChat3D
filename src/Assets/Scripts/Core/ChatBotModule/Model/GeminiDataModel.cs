using System.Collections.Generic;
using Core.MVC;
using UnityEngine;
using Uralstech.UGemini.Models.Content;

namespace Core.ChatBotModule.Model
{
    public class GeminiDataModel: ModelBase
    {
        public List<GeminiContent> History;
        public string CurrentGeminiModelName;
        public GameObject ChatBotAvatar;

        protected override void OnInit()
        {

        }
        
    }
}