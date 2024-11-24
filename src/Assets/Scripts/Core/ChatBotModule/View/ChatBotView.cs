using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Core.ChatBotModule.Controller;
using Core.ChatBotModule.Model;
using Core.MessageModule.Model;
using Core.MessageModule.View;
using Core.MVC;
using JimmysUnityUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.ChatBotModule.View
{
    public class ChatBotView: ViewBase, LoopScrollPrefabSource, LoopScrollDataSource
    {
        public Button newChat;
        public Transform chatBotModelPoint;
        public ExpressionControl chatBotExpressionControl;
        public TMP_InputField chatInput;
        public Button send;
        public RuntimeAnimatorController animatorController;
        public LoopScrollRect list;
        
        public ChatSession ChatSession;
        public override void Render(ModelBase model)
        {
            var geminiDataModel = model as GeminiDataModel;
            geminiDataModel.ChatBotAvatar.GetComponent<Animator>().runtimeAnimatorController = animatorController;
            chatBotModelPoint.DestroyAllChildren();
            geminiDataModel.ChatBotAvatar.transform.SetParent(chatBotModelPoint, false);
            chatBotExpressionControl = geminiDataModel.ChatBotAvatar.GetComponent<ExpressionControl>();
            ChatSession = geminiDataModel.ChatHistory;
            list.prefabSource = this;
            list.dataSource = this;
            list.totalCount = 0;
            list.RefillCells();
        }
        
        public void RefreshList()
        {
            list.totalCount = ChatSession.ChatData.Count;
            list.RefillCells();
        }
        
        public override void OnInit()
        {
          
        }
        
        public GameObject itemPrefab;
        Stack<Transform> pool = new Stack<Transform>();
        public GameObject GetObject(int index)
        {
            if (pool.Count == 0)
            {
                return Instantiate(itemPrefab);
            }
            var candidate = pool.Pop();
            candidate.gameObject.SetActive(true);
            return candidate.gameObject;
        }

        public void ReturnObject(Transform trans)
        {
            trans.gameObject.SetActive(false);
            trans.SetParent(transform, false);
            pool.Push(trans);
        }

        public void ProvideData(Transform trans, int index)
        {
            var item = trans.GetComponent<ChatBox>();
            if (index < ChatSession.ChatData.Count)
            {
                item.role.text = ChatSession.ChatData[index].UserId;
                item.text.Source = ChatSession.ChatData[index].Content;
                item.time.text = ChatSession.ChatData[index].Time;
                if (ChatSession.ChatData[index].UserId == "User")
                    item.background.color = Color.blue;
                else
                    item.background.color = Color.red;
            }
        }
    }
}