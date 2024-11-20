using System;
using System.Collections.Generic;
using Core.MVC;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Core.FriendModule.View
{
    public class FriendListView: ViewBase
    {
        public RectTransform friendListScrollViewParent;
        public GameObject listItemTemplate;
        public List<FriendListItem> items;
        public TMP_InputField userIdSearch;
        public Button addFriendButton;
        
        public override void Render(ModelBase model)
        {
            
        }

        public override void OnInit()
        {
        }
    }
}