using System;
using System.Collections.Generic;
using Core.MVC;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Core.CharacterCustomizationModule.View
{
    public class ModelListView: ViewBase
    {
        public RectTransform modelListScrollViewParent;
        public GameObject listItemTemplate;
        public List<ModelListItem> items;
        public Button selectAsChatBotAvatar;
        public Button selectAsCharacterInRoom;
        public Transform previewModelPoint;
        public override void Render(ModelBase model)
        {
            
        }

        public override void OnInit()
        {
        }
    }
}