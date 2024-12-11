using System;
using System.Collections.Generic;
using System.Linq;
using Core.MessageModule.Model;
using Core.MessageModule.View;
using Core.MVC;
using Core.OnlineRuntimeModule.RoomManagementModule.Model;
using TMPro;
using UMI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Core.OnlineRuntimeModule.RoomManagementModule.View
{
    public class HostRoomView : ViewBase, LoopScrollPrefabSource, LoopScrollDataSource
    {
        public MobileInputField ip;
        public MobileInputField port;
        public TMP_InputField TMP_ip;
        public TMP_InputField TMP_port;
        public Button createRoom;
        public Button host;
        public RoomDataModel RoomDataModel;
        public string currentActiveRoom;
        public Sprite hostingIcon;
        public Sprite selectIcon;

        public override void Render(ModelBase model)
        {
            RoomDataModel = model as RoomDataModel;
            ip.Text = "localhost";
            port.Text = "8888";
#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IOS)
            TMP_ip.text = "localhost";
            TMP_port.text = "8888";
#endif
            list.prefabSource = this;
            list.dataSource = this;
            list.totalCount = RoomDataModel.RoomsData.Count;
            list.RefillCells();
        }

        public override void OnInit()
        {

        }
        public LoopScrollRect list;

        public void RefreshList()
        {
            list.totalCount = RoomDataModel.RoomsData.Count;
            list.RefillCells();
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
            var item = trans.GetComponent<RoomListItem>();
            item.delete.onClick.RemoveAllListeners();
            item.host.onClick.RemoveAllListeners();
            item.editEnvironment.onClick.RemoveAllListeners();
            trans.gameObject.SetActive(false);
            trans.SetParent(transform, false);
            pool.Push(trans);
        }

        public Action<string> OnDeleteRoom;
        public Action<string> OnEditRoom;
        public Action<RoomData> OnHostRoom;
        public void ProvideData(Transform trans, int index)
        {
            var item = trans.GetComponent<RoomListItem>();
            if (index < RoomDataModel.RoomsData.Count)
            {
                item.delete.onClick.RemoveAllListeners();
                item.host.onClick.RemoveAllListeners();
                item.editEnvironment.onClick.RemoveAllListeners();
                item.roomId.text = RoomDataModel.RoomsData.ElementAt(index).RoomId;
                item.hostIcon.sprite = RoomDataModel.RoomsData.ElementAt(index).RoomId == currentActiveRoom ? hostingIcon : selectIcon;
                item.delete.onClick.AddListener(() =>
                {
                    OnDeleteRoom(RoomDataModel.RoomsData.ElementAt(index).RoomId);
                });
                item.host.onClick.AddListener(() =>
                {
                    OnHostRoom(RoomDataModel.RoomsData.ElementAt(index));
                });
                item.editEnvironment.onClick.AddListener(() =>
                {
                    OnEditRoom(RoomDataModel.RoomsData.ElementAt(index).RoomId);
                });
            }
        }
    }
}