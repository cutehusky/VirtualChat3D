using System.Collections.Generic;
using Core.MessageModule.Model;
using Core.MessageModule.View;
using Core.MVC;
using TMPro;
using UMI;
using UnityEngine;
using UnityEngine.UI;

namespace Core.OnlineRuntimeModule.MessageModule
{
    public class MessageView: ViewBase, LoopScrollPrefabSource, LoopScrollDataSource
    {
        public MobileInputField chatInput;
        public TMP_InputField TMP_chatInput;
        public Button send;
        public LoopScrollRect list;
        public ChatSession ChatSession;
        public Button back;
        public RectTransform inputRect;
        public RectTransform listRect;
        public string myUserId;

        public override void Render(ModelBase model)
        {
            list.prefabSource = this;
            list.dataSource = this;
            list.totalCount = ChatSession.ChatData.Count;
            list.RefillCells();
        }

        public void Render(ChatSession chatSession, string userId)
        {
            myUserId = userId;
            ChatSession = chatSession;
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
                if (ChatSession.ChatData[index].UserId == myUserId)
                {
                    item.background.color = Color.blue;
                    item.roundedCorners.r = new Vector4(40, 0, 40, 40);
                    item.role.alignment = TextAlignmentOptions.Left;
                }
                else
                {
                    item.background.color = Color.red;
                    item.roundedCorners.r = new Vector4(0, 40, 40, 40);
                    item.role.alignment = TextAlignmentOptions.Right;
                }
                item.roundedCorners.Refresh();
            }
        }

    }
}