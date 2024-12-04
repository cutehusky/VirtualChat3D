using System;
using System.Collections.Generic;
using Core.AdminModule.Model;
using Core.MVC;
using TMPro;
using UMI;
using UnityEngine;
using UnityEngine.UI;

namespace Core.AdminModule.View
{
    public class UserListView: ViewBase, LoopScrollPrefabSource, LoopScrollDataSource
    {
        public MobileInputField userIdSearch;
        public TMP_InputField TMP_userIdSearch;
        public RectTransform inputRect;
        public RectTransform listRect;
        public LoopScrollRect list;
        public Button searchUserButton;
        public UserAccountDataModel _userAccountDataModel;
        public override void Render(ModelBase model)
        {
            _userAccountDataModel = model as UserAccountDataModel;
            list.prefabSource = this;
            list.dataSource = this;
            if (_userAccountDataModel.UserList == null)
                list.totalCount = 0;
            else
                list.totalCount = _userAccountDataModel.UserList.Count;
            list.RefillCells();
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

        public void RefreshList()
        {
            list.totalCount = _userAccountDataModel.UserList.Count;
            list.RefillCells();
        }

        private float _listOffset;
        private float _inputOffset;
        public override void OnInit()
        {
            _listOffset = listRect.offsetMin.y;
            _inputOffset = inputRect.offsetMin.y;
        }

        public GameObject itemPrefab;
        Stack<Transform> pool = new Stack<Transform>();
        public Sprite lockIcon;
        public Sprite unlockIcon;
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
            var item = trans.GetComponent<UserListItem>();
            item.button1.onClick.RemoveAllListeners();
            item.button2.onClick.RemoveAllListeners();
            trans.gameObject.SetActive(false);
            trans.SetParent(transform, false);
            pool.Push(trans);
        }

        public Action<string> OnRemoveUser;
        public Action<string> OnLockUser;
        public Action<string> OnUnlockUser;

        public void ProvideData(Transform trans, int index)
        {
            var item = trans.GetComponent<UserListItem>();
            if (index < _userAccountDataModel.UserList.Count)
            {
                item.button1.onClick.RemoveAllListeners();
                item.button2.onClick.RemoveAllListeners();
                item.username.text = _userAccountDataModel.UserList[index].Username;
                
                item.button2.onClick.AddListener(() =>
                {
                    OnRemoveUser(_userAccountDataModel.UserList[index].UserId);
                });

                if (_userAccountDataModel.UserList[index].IsLock)
                {
                    item.button1Icon.sprite = unlockIcon;
                    item.button1.onClick.AddListener(() =>
                    {
                        OnUnlockUser(_userAccountDataModel.UserList[index].UserId);
                    });
                } else
                {
                    item.button1Icon.sprite = lockIcon;
                    item.button1.onClick.AddListener(() =>
                    {
                        OnLockUser(_userAccountDataModel.UserList[index].UserId);
                    });
                }
            }
        }

    }
}