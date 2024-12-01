
using System.Collections.Generic;
using Core.MessageModule.Model;
using Core.MVC;
using TMPro;
using UMI;
using UnityEngine;
using UnityEngine.UI;

namespace Core.MessageModule.View
{
    public class MessageView: ViewBase, LoopScrollPrefabSource, LoopScrollDataSource
    {
        public MobileInputField chatInput;
        public TMP_InputField TMP_chatInput;
        public Button send;
        public LoopScrollRect list;
        public ChatSession ChatSession;
        public string friendId;
        public Button back;
        public TMP_Text friendName;
        public RectTransform inputRect;
        public RectTransform listRect;
        
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
        
        private float _listOffset;
        private float _inputOffset;
        public override void OnInit()
        {
            _listOffset = listRect.offsetMin.y;
            _inputOffset = inputRect.offsetMin.y;
        }
        
        public override void MoveUpWhenOpenKeyboard(float height)
        {
            var inputOffset = (height > 0) ? height : _inputOffset;
            inputRect.offsetMax = new Vector2(inputRect.offsetMax.x, inputOffset + inputRect.sizeDelta.y);
            inputRect.offsetMin = new Vector2(inputRect.offsetMin.x, inputOffset);
            var listOffset = (height > 0) ? inputRect.sizeDelta.y + height : _listOffset;
            listRect.offsetMin = new Vector2(listRect.offsetMin.x, listOffset);
            if (height == 0)
                TMP_chatInput.DeactivateInputField();
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