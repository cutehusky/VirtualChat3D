using System.Collections.Generic;
using Core.MVC;
using UnityEngine;

namespace Core.OnlineRuntimeModule.RoomManagementModule.View
{
    public class JoinedUserListView: ViewBase
    {
        public RectTransform scrollViewParent;
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