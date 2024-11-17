using System.Collections.Generic;
using Core.FriendModule;
using Core.MVC;
using QFramework;
using UnityEngine;

namespace Core.CharacterCustomizeModule.Model
{
    public class CharacterModelDataModel: ModelBase
    {
        public List<string> ModelId;
        public string ChatRoomSelectModelId;
        public string ChatBotSelectModelId;
        
        protected override void OnInit()
        {
            
        }
    }
}