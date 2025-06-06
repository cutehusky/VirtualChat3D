﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Core.MessageModule.Model;
using Core.MVC;
using UnityEngine;
using Uralstech.UGemini;
using Uralstech.UGemini.Models.Content;
using Uralstech.UGemini.Models.Generation.Chat;

namespace Core.ChatBotModule.Model
{
    public class GeminiDataModel: ModelBase
    {
        private List<GeminiContent> _history;
        public ChatSession ChatHistory;
        public GameObject ChatBotAvatar;
        private const string ChatBotModelId = "gemini-1.5-pro";
        public bool IsProcessing;

        protected override void OnInit()
        {
            _history = new();
            ChatHistory = new();
        }

        public void InitHistory()
        {
            _history.Clear();
            _history.Add(
                GeminiContent.GetContent("In this conversation, all response messages must comply with the following response specifications: - Pretend you are my girlfriend. - Will respond in a cute tone, sometimes acting coquettishly, sometimes naughtily, and sometimes getting a little angry. \"Baby\" or \"baby\" are often added to conversations. answer question and give emotion in format *emotion* (there are 5 emotion: Happy, Sad, Surprise, Angry, Neutral)", GeminiRole.User)
            );
            ChatHistory.ChatData.Clear();
        }

        public async Task<string> GetResponse(string text)
        {
            _history.Add(GeminiContent.GetContent(text, GeminiRole.User));
            IsProcessing = true;
            var response = await GeminiManager.Instance.Request<GeminiChatResponse>(
                new GeminiChatRequest(ChatBotModelId)
                {
                    Contents = _history.ToArray(),
                } 
            );
            if (response.Candidates == null || response.Candidates.Length == 0)
                return "No internet or your message violate the Gemini's ToS, Please start a new chat";
            IsProcessing = false;
            _history.Add(response.Candidates[0].Content);
            return response.Parts[0].Text;
        }
    }
}