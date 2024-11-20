using System;
using System.Collections.Generic;
using Core.MVC;
using UnityEngine;

namespace Core.CharacterCustomizationModule.Model
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
        
        public void CreateCharacter(string modelId, Action<GameObject> onVrmModelLoaded)
        {
            
        }
        
        protected override void OnInit()
        {
            
        }
    }
}