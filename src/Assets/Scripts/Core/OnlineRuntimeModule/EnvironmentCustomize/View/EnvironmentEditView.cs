using System;
using System.Collections.Generic;
using Core.MVC;
using UnityEngine;

namespace Core.OnlineRuntimeModule.EnvironmentCustomize.View
{
    public class EnvironmentEditView: ViewBase
    {
        public Transform rootOfScene;
        public List<GameObject> itemToPut;
        
        public RectTransform scrollViewParent;
        public List<ObjectListItem> items;
     
        public override void Render(ModelBase model)
        {
            
        }

        public override void OnInit()
        {
            
        }
    }
}