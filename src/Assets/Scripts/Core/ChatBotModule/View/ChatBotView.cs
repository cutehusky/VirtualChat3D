using System;
using Core.ChatBotModule.Controller;
using Core.ChatBotModule.Model;
using Core.MessageModule.View;
using Core.MVC;
using JimmysUnityUtilities;
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
        public RuntimeAnimatorController animatorController;
        public override void Render(ModelBase model)
        {
            var geminiDataModel = model as GeminiDataModel;
            geminiDataModel.ChatBotAvatar.GetComponent<Animator>().runtimeAnimatorController = animatorController;
            chatBotModelPoint.DestroyAllChildren();
            geminiDataModel.ChatBotAvatar.transform.SetParent(chatBotModelPoint, false);
            chatBotExpressionControl = geminiDataModel.ChatBotAvatar.GetComponent<ExpressionControl>();
            userInput.gameObject.SetActive(false);
            chatBotOutput.gameObject.SetActive(false);
        }

        public void SetInputText(string text)
        {
            userInput.gameObject.SetActive(true);
            userInput.text.Source = text;
            userInput.role.text = "User";
        }

        public void SetOutputText(string text)
        {
            chatBotOutput.gameObject.SetActive(true);
            chatBotOutput.text.Source = text;
            chatBotOutput.role.text = "ChatBot";
        }
        
        public override void OnInit()
        {
            
        }
    }
}