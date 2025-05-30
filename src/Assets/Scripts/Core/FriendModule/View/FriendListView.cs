﻿using System;
using System.Collections.Generic;
using System.Linq;
using Core.FriendModule.Model;
using Core.MVC;
using TMPro;
using UMI;
using UnityEngine;
using UnityEngine.UI;

namespace Core.FriendModule.View
{
    public class FriendListView: ViewBase, LoopScrollPrefabSource, LoopScrollDataSource
    {
        public LoopScrollRect list;
        public MobileInputField userIdSearch;
        public TMP_InputField TMP_userIdSearch;
        public Button addFriendButton;
        public FriendDataModel _friendDataModel;
        public RectTransform inputRect;
        public RectTransform listRect;
        public Sprite buttonRemoveIcon;
        public Sprite buttonRefuseIcon;
        public Sprite buttonMessageIcon;
        public Sprite buttonAcceptIcon;

        public override void Render(ModelBase model)
        {
            _friendDataModel = model as FriendDataModel;
            list.prefabSource = this;
            list.dataSource = this;
            list.totalCount = _friendDataModel.FriendList.Count + _friendDataModel.RequestList.Count;
            list.RefillCells();
        }

        public void RefreshList()
        {
            if (_friendDataModel == null)
                return;
            list.totalCount = _friendDataModel.FriendList.Count + _friendDataModel.RequestList.Count;
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
                TMP_userIdSearch.DeactivateInputField();
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
            var item = trans.GetComponent<FriendListItem>();
            item.button1.onClick.RemoveAllListeners();
            item.button2.onClick.RemoveAllListeners();
            trans.gameObject.SetActive(false);
            trans.SetParent(transform, false);
            pool.Push(trans);
        }

        public Action<string, string> OpenMessageView;
        public Action<string> OnRemoveFriend;
        public Action<string> OnRequestAccept;
        public Action<string> OnRequestRefuse;

        public void ProvideData(Transform trans, int index)
        {
            var item = trans.GetComponent<FriendListItem>();
            item.button1.onClick.RemoveAllListeners();
            item.button2.onClick.RemoveAllListeners();
            if (index < _friendDataModel.FriendList.Count)
            {
                //item.userId.text = _friendDataModel.FriendList[index].UserId;
                item.username.text = _friendDataModel.FriendList.ElementAt(index).Value.Username;
                //item.description.text = _friendDataModel.FriendList[index].Description;
                //item.dateOfBirth.text = _friendDataModel.FriendList[index].DateOfBirth.ToString(CultureInfo.InvariantCulture);
                item.button1Icon.sprite = buttonRemoveIcon;
                item.button2Icon.sprite = buttonMessageIcon;
                item.button2.onClick.AddListener(() =>
                {
                    OpenMessageView(
                        _friendDataModel.FriendList.ElementAt(index).Value.ChatSessionId,
                        _friendDataModel.FriendList.ElementAt(index).Value.UserId);
                });
                item.button1.onClick.AddListener(() =>
                {
                    OnRemoveFriend(_friendDataModel.FriendList.ElementAt(index).Value.UserId);
                });
            }
            else
            {
                item.button1Icon.sprite = buttonAcceptIcon;
                item.button2Icon.sprite = buttonRefuseIcon;
                item.username.text = _friendDataModel.RequestList.ElementAt(index - _friendDataModel.FriendList.Count).Value.Username;
                item.button2.onClick.AddListener(() =>
                {
                    OnRequestRefuse(_friendDataModel.RequestList.ElementAt(index - _friendDataModel.FriendList.Count).Value.UserId);
                });
                item.button1.onClick.AddListener(() =>
                {
                    OnRequestAccept(_friendDataModel.RequestList.ElementAt(index - _friendDataModel.FriendList.Count).Value.UserId);
                });
            }
        }
     }
}