using System;
using System.Collections.Generic;
using Core.MVC;
using UnityEngine;
using UnityEngine.UI;

namespace Core.CharacterCustomizationModule.View
{
    public class ModelListView: ViewBase
    {
        public RectTransform scrollViewParent;
        public GameObject listItemTemplate;
        public List<ModelListViewItem> items;
        public Button select;
        public Transform previewModelPoint;
        public override void Render(ModelBase model)
        {
            
        }

        public override void OnInit()
        {
        }
    }
}