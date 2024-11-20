using System;
using Core.ChatBotModule.Controller;
using Core.MessageModule.View;
using Core.MVC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.ChatBotModule.View
{
    public class ChatBotView: ViewBase
    {
        public Button newChat;
        public Transform chatBotModelPoint;
        public ExpressionControl chatBotExpressionControl;
        public TMP_InputField chatInput;
        public Button send;
        public ChatBox userInput;
        public ChatBox chatBotOutput;
        public override void Render(ModelBase model)
        {
            
        }

        public override void OnInit()
        {
            
        }
    }
}