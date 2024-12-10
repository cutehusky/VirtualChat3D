using System;
using System.Collections.Generic;
using Core.AdminModule.Model;
using Core.MVC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.AdminModule.View
{
    public class SystemMonitorView : ViewBase, LoopScrollPrefabSource, LoopScrollDataSource
    {
        public TMP_Text OnlineUserCount;
        public TMP_Text CpuSpeed;
        public TMP_Text Cpu;
        public TMP_Text Ram;
        public LoopScrollRect list;
        private SystemInfoDataModel _systemInfoDataModel;

        public override void Render(ModelBase model)
        {
            _systemInfoDataModel = model as SystemInfoDataModel;
            OnlineUserCount.text = _systemInfoDataModel.OnlineUserCount.ToString();
            Cpu.text = _systemInfoDataModel.Cpu;
            CpuSpeed.text = _systemInfoDataModel.CpuSpeed.ToString();
            Ram.text = _systemInfoDataModel.Ram.ToString();
            list.prefabSource = this;
            list.dataSource = this;
            if (_systemInfoDataModel.Countries == null)
                list.totalCount = 0;
            else
                list.totalCount = _systemInfoDataModel.Countries.Count;
            list.RefillCells();
        }

        public override void OnInit()
        {

        }

        public void RefreshList()
        {
            list.totalCount = _systemInfoDataModel.Countries.Count;
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
            trans.gameObject.SetActive(false);
            trans.SetParent(transform, false);
            pool.Push(trans);
        }

        public void ProvideData(Transform trans, int index)
        {
            var item = trans.GetComponent<AnalyticsListItem>();
            if (index < _systemInfoDataModel.Countries.Count)
            {
                item.activeUser.text = _systemInfoDataModel.Countries[index].activeUsers.ToString();
                item.countryName.text = _systemInfoDataModel.Countries[index].name;
            }
        }
    }
}