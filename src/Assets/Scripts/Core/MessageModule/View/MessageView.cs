using System;
using System.Collections.Generic;
using Core.MVC;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Core.MessageModule.View
{
    public class MessageView: ViewBase
    {
        public TMP_InputField chatInput;
        public Button send;
        public RectTransform messageScrollViewParent;
        public GameObject listItemTemplate;
        public List<ChatBox> items;
        
        public override void Render(ModelBase model)
        {
            
        }

        public override void OnInit()
        {
        }
    }
}