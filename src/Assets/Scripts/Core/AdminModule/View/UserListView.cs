using System.Collections.Generic;
using Core.FriendModule.View;
using Core.MVC;
using TMPro;
using UnityEngine;

namespace Core.AdminModule.View
{
    public class UserListView: ViewBase
    {
        public RectTransform scrollViewParent;
        public GameObject listItemTemplate;
        public List<FriendListItem> items;
        public TMP_InputField userIdSearch;
        
        public override void Render(ModelBase model)
        {
            
        }

        public override void OnInit()
        {
        }
    }
}