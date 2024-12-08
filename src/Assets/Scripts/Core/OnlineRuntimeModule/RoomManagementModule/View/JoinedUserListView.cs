using System;
using System.Collections.Generic;
using System.Linq;
using Core.MVC;
using Core.OnlineRuntimeModule.RoomManagementModule.Model;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Core.OnlineRuntimeModule.RoomManagementModule.View
{
    public class JoinedUserListView: ViewBase, LoopScrollPrefabSource, LoopScrollDataSource
    {
        public LoopScrollRect list;
        public GameObject itemPrefab;
        Stack<Transform> pool = new Stack<Transform>();
        private RoomDataModel _dataModel;
        public Action<ulong> OnKick;
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
            var item = trans.GetComponent<JoinedUserListItem>();
            item.kick.onClick.RemoveAllListeners();
            trans.gameObject.SetActive(false);
            trans.SetParent(transform, false);
            pool.Push(trans);
        }

        public void ProvideData(Transform trans, int index)
        {
            var item = trans.GetComponent<JoinedUserListItem>();
            var data = _dataModel.CurrentHostRoomJoinedUser.ElementAt(index);
            item.kick.onClick.RemoveAllListeners();
            item.username.text = data.Value.Username;
            //item.userId.text = _dataModel.CurrentHostRoomJoinedUser.ElementAt(index).Value.UserId;
            if (data.Key != NetworkManager.Singleton.LocalClientId)
            {
                item.kick.onClick.AddListener(() => { OnKick(data.Key); });
                item.kick.gameObject.SetActive(true);
            }
            else
            {
                item.kick.gameObject.SetActive(false);
            }
        }
         
        
        public void RefreshList()
        {
            list.totalCount = _dataModel.CurrentHostRoomJoinedUser.Count;
            list.RefillCells(); 
        }
        
        public override void Render(ModelBase model)
        {
            _dataModel = model as RoomDataModel;
            list.prefabSource = this;
            list.dataSource = this;
            list.totalCount = _dataModel.CurrentHostRoomJoinedUser.Count;
            list.RefillCells();
        }

        public override void OnInit()
        {
        }
    }
}