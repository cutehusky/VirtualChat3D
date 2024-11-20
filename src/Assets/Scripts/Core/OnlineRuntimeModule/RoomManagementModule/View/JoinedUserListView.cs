using System;
using System.Collections.Generic;
using Core.MVC;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.OnlineRuntimeModule.RoomManagementModule.View
{
    public class JoinedUserListView: ViewBase
    {
        public RectTransform joinedUserListScrollViewParent;
        public GameObject listItemTemplate;
        public List<JoinedUserListItem> items;
        
        public override void Render(ModelBase model)
        {
            
        }

        public override void OnInit()
        {
        }
    }
}