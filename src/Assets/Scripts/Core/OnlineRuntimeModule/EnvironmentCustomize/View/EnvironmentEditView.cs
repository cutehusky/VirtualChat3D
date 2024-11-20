using System;
using System.Collections.Generic;
using Core.MVC;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.OnlineRuntimeModule.EnvironmentCustomize.View
{
    public class EnvironmentEditView: ViewBase
    {
        public Transform rootOfScene;
        public List<GameObject> itemToPut;
        
        public RectTransform objectListScrollViewParent;
        public List<ObjectListItem> items;
        public GameObject listItemTemplate;
        public ObjectPuttingArea objectPuttingArea;
     
        public override void Render(ModelBase model)
        {
            
        }

        public override void OnInit()
        {
            
        }
    }
}