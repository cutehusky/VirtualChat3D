using System.Collections.Generic;
using Core.MessageModule.View;
using Core.MVC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.OnlineRuntimeModule.MessageModule
{
    public class MessageView: ViewBase
    {
        public TMP_InputField chatInput;
        public RectTransform scrollViewParent;
        public GameObject listItemTemplate;
        public List<ChatBox> items;
        public Button send;
        public TMP_Text _3DOnCharacterTopText;
        
        public override void Render(ModelBase model)
        {
            
        }

        public override void OnInit()
        {
        }
    }
}