using System;
using System.Collections.Generic;
using Core.CharacterCustomizationModule.Model;
using Core.MVC;
using UnityEngine;
using UnityEngine.UI;

namespace Core.CharacterCustomizationModule.View
{
    public class ModelListView: ViewBase,  LoopScrollPrefabSource, LoopScrollDataSource
    {
        public Button import;
        public Transform previewModelPoint;
        private CharacterModelDataModel _dataModel;
        public Action<int> OnModelPreviewChange;
        public Action<int> OnSelectChatBotModel;
        public Action<int> OnSelectCharacterModel;
        public LoopScrollRect list;
        public override void Render(ModelBase model)
        {
            _dataModel = model as CharacterModelDataModel;
            
            list.prefabSource = this;
            list.dataSource = this;
            list.totalCount = _dataModel.ModelId.Count;
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
            var item = trans.GetComponent<ModelListItem>();
            item.selectAsCharacter.onClick.RemoveAllListeners();
            item.selectAsChatBot.onClick.RemoveAllListeners();
            item.preview.onClick.RemoveAllListeners();
            trans.gameObject.SetActive(false);
            trans.SetParent(transform, false);
            pool.Push(trans);
        }

        public void ProvideData(Transform trans, int index)
        {
            var item = trans.GetComponent<ModelListItem>();
            item.modelId.text = _dataModel.ModelId[index];
            item.preview.onClick.AddListener(() =>
            {
                OnModelPreviewChange(index);
            });
            
            item.selectAsCharacter.onClick.AddListener(() =>
            {
                OnSelectCharacterModel(index);
            });
            
            item.selectAsChatBot.onClick.AddListener(() =>
            {
                OnSelectChatBotModel(index);
            });
        }
    }
}