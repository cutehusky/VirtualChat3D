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
        
        public void LoadModelList()
        {
            
        }

        public byte[] LoadModelFromInternal(string fileName)
        {
            return null;
        }
        
        public GameObject CreateCharacter(string modelId)
        {
            return null;
        }
        
        protected override void OnInit()
        {
            
        }
    }
}