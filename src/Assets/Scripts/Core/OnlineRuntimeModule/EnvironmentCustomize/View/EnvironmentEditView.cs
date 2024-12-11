using System;
using System.Collections.Generic;
using Core.MVC;
using Core.OnlineRuntimeModule.EnvironmentCustomize.Model;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Core.OnlineRuntimeModule.EnvironmentCustomize.View
{
    public class EnvironmentEditView: ViewBase, LoopScrollPrefabSource, LoopScrollDataSource
    {
        public LoopScrollRect list;
        public GameObject listItemPrefab;
        Stack<Transform> pool = new Stack<Transform>();
        public ObjectPuttingArea objectPuttingArea;
        public List<GameObject> puttingItemPrefab;
        public Action<ItemObject> OnPuttingItemSuccess;
        public Transform objectParent;
        public Button save;
        private EnvironmentDataModel _dataModel;
        public Button back;
        public override void Render(ModelBase model)
        {
            _dataModel = model as EnvironmentDataModel;
            foreach (Transform child in objectParent)
            {
                Destroy(child.gameObject);
            }
            _dataModel.InSceneObject.Clear();
        }
        
        public void LoadItem()
        {
            foreach (var itemData in _dataModel.CurrentEditingEnvironmentData)
            {
                var obj = Instantiate(puttingItemPrefab[itemData.ID], objectParent, true);
                obj.GetComponent<ItemObject>().ImportData(itemData);
                _dataModel.InSceneObject.Add(obj, itemData);
            }
        }

        public void OnEnable()
        {
            objectParent.gameObject.SetActive(true);
        }

        public void OnDisable()
        {
            objectParent.gameObject.SetActive(false);
        }

        public override void OnInit()
        {
            list.dataSource = this;
            list.prefabSource = this;
            list.dataSource = this;
            list.totalCount = puttingItemPrefab.Count;
            list.RefillCells();
        }
        
        public GameObject GetObject(int index)
        {
            if (pool.Count == 0)
            {
                return Instantiate(listItemPrefab);
            }
            var candidate = pool.Pop();
            candidate.gameObject.SetActive(true);
            return candidate.gameObject;
        }

        public void ReturnObject(Transform trans)
        {
            var item = trans.GetComponent<ObjectListItem>();
            item.OnPuttingSuccess = null;
            trans.gameObject.SetActive(false);
            trans.SetParent(transform, false);
            pool.Push(trans);
        }
        
        public void ProvideData(Transform trans, int index)
        {
            var item = trans.GetComponent<ObjectListItem>();
            var data = puttingItemPrefab[index].GetComponent<ItemObject>();
            item.iconImage.sprite = data.icon;
            item.canvas = gameObject;
            item.targetPrefab = puttingItemPrefab[index];
            item.objectParent = objectParent;
            item.OnPuttingSuccess += (obj) =>
            {
                OnPuttingItemSuccess(obj);
            };
        }
    }
}