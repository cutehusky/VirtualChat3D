using System;
using System.Collections.Generic;
using System.Globalization;
using Core.AdminModule.Model;
using Core.AdminModule.View;
using Core.FriendModule.Model;
using Core.FriendModule.View;
using Core.MVC;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Core.AdminModule.View
{
    public class UserListView: ViewBase, LoopScrollPrefabSource, LoopScrollDataSource
    {
        public TMP_InputField userIdSearch;
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

        public void RefreshList()
        {
            list.totalCount = _userAccountDataModel.UserList.Count;
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

        public Action<string> OnRemoveUser;
        public Action<string> OnLockUser;
        public Action<string> OnUnlockUser;

        public void ProvideData(Transform trans, int index)
        {
            var item = trans.GetComponent<UserListItem>();
            if (index < _userAccountDataModel.UserList.Count)
            {
                
                item.username.text = _userAccountDataModel.UserList[index].Username;
                
                item.button2.onClick.AddListener(() =>
                {
                    OnRemoveUser(_userAccountDataModel.UserList[index].UserId);
                });

                if (_userAccountDataModel.UserList[index].IsLock)
                {
                    item.button1.onClick.AddListener(() =>
                    {
                        OnUnlockUser(_userAccountDataModel.UserList[index].UserId);
                    });
                } else
                {
                    item.button1.onClick.AddListener(() =>
                    {
                        OnLockUser(_userAccountDataModel.UserList[index].UserId);
                    });
                }
            }
        }

    }
}