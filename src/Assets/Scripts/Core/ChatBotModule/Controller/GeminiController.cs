using System;
using System.Collections.Generic;
using System.Globalization;
using Core.CharacterCustomizationModule.Model;
using Core.ChatBotModule.Model;
using Core.ChatBotModule.View;
using Core.MessageModule.Model;
using Core.MVC;
using QFramework;
using UnityEngine;
using UniVRM10;

namespace Core.ChatBotModule.Controller
{
    public class GeminiController: ControllerBase
    {
        private ChatBotView _chatBotView;
        
        private async void OnChat(string text)
        {
            if (this.GetModel<GeminiDataModel>().IsProcessing)
                return;
            var str = await this.GetModel<GeminiDataModel>().GetResponse(text);
            if (!this.GetModel<GeminiDataModel>().IsProcessing)
            {
                Debug.Log(str);
                str = ExpressionControl(str);
                Debug.Log(str);
                _chatBotView.send.interactable = true;
            } 
            AddMessage("ChatBot", str);
            _chatBotView.newChat.interactable = true;
        }

        private string ExpressionControl(string chatContent)
        {
            if (chatContent.IndexOf("*Sad*", StringComparison.Ordinal) != -1)
            {
                _chatBotView.chatBotExpressionControl.Sad();
                chatContent = chatContent.Replace("*Sad*", "");
            }
            else if (chatContent.IndexOf("*Happy*", StringComparison.Ordinal) != -1)
            {
                _chatBotView.chatBotExpressionControl.Fun();
                chatContent = chatContent.Replace("*Happy*", "");
            } else if (chatContent.IndexOf("*Angry*", StringComparison.Ordinal) != -1)
            {
                _chatBotView.chatBotExpressionControl.Angry();
                chatContent = chatContent.Replace("*Angry*", "");
            } else if (chatContent.IndexOf("*Surprise*", StringComparison.Ordinal) != -1)
            {
                _chatBotView.chatBotExpressionControl.Surprised();
                chatContent = chatContent.Replace("*Surprise*", "");
            } else if (chatContent.IndexOf("*Neutral*", StringComparison.Ordinal) != -1)
            {
                _chatBotView.chatBotExpressionControl.Neutral();
                chatContent = chatContent.Replace("*Neutral*", "");
            }
            return chatContent;
        }

        public void NewChat()
        {
            this.GetModel<GeminiDataModel>().InitHistory();
            this.GetModel<CharacterModelDataModel>().CreateChatBotCharacter((model =>
            {
                model.AddComponent<ExpressionControl>().vrm10Instance = model.GetComponent<Vrm10Instance>();
                this.GetModel<GeminiDataModel>().ChatBotAvatar = model;
                _chatBotView.Display(false);
                _chatBotView.Render(this.GetModel<GeminiDataModel>());
            }));
        }
        
        public override void OnInit(List<ViewBase> view)
        {
            base.OnInit(view);
            _chatBotView = view[0] as ChatBotView;
            _chatBotView.send.onClick.AddListener((() =>
            {
                var text = _chatBotView.chatInput.Text;
                _chatBotView.chatInput.Text = "";
#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IOS)
                _chatBotView.TMP_chatInput.text = "";
#endif
                _chatBotView.send.interactable = false;
                _chatBotView.newChat.interactable = false;
                AddMessage("User", text);
                OnChat(text);
            }));
            _chatBotView.newChat.onClick.AddListener((() =>
            {
                NewChat();
            }));
            _chatBotView.back.onClick.AddListener(() =>
            {
                AppMain.Instance.OpenHostRoomView();
            });
        }
        
        public void AddMessage(string role, string text)
        {
            this.GetModel<GeminiDataModel>().ChatHistory.ChatData.Add(
                new ChatMessage()
                {
                    Content = text,
                    UserId = role,
                    Time = DateTime.Now.ToString(CultureInfo.InvariantCulture)
                });
            _chatBotView.RefreshList();
        }

        public ViewBase OpenChatBotView()
        {
            AppMain.Instance.CloseCurrentView();
            NewChat();
            return _chatBotView;
        }
    }
}