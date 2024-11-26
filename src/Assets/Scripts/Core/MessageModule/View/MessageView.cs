
using System.Collections.Generic;
using Core.MessageModule.Model;
using Core.MVC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.MessageModule.View
{
    public class MessageView: ViewBase, LoopScrollPrefabSource, LoopScrollDataSource
    {
        public TMP_InputField chatInput;
        public Button send;
        public LoopScrollRect list;
        public ChatSession ChatSession;
        public string friendId;
        public Button back;
        public TMP_Text friendName;
        
        public override void Render(ModelBase model)
        {
            var messageDataModel = model as MessageDataModel;
            ChatSession = messageDataModel.CurrentChatSession;
            friendId = messageDataModel.CurrentChatSession.FriendId;
            friendName.text = friendId;
            list.prefabSource = this;
            list.dataSource = this;
            if (messageDataModel.CurrentChatSession == null)
                list.totalCount = 0;
            else
                list.totalCount = messageDataModel.CurrentChatSession.ChatData.Count;
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
                if (ChatSession.ChatData[index].UserId != friendId) {
                    item.background.color = Color.blue;
                    item.roundedCorners.r = new Vector4(40,0,40,40);
                }else {
                    item.background.color = Color.red;
                    item.roundedCorners.r = new Vector4(0,40,40,40);
                } 
                item.roundedCorners.Refresh();
            }
        }
    }
}