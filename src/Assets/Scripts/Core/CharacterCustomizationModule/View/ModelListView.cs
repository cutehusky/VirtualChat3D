using System;
using System.Collections.Generic;
using Core.CharacterCustomizationModule.Model;
using Core.MVC;
using UIS;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Core.CharacterCustomizationModule.View
{
    public class ModelListView: ViewBase
    {
        public Scroller list;
        public Button import;
        public Transform previewModelPoint;
        private CharacterModelDataModel _dataModel;
        public Action<int> OnModelPreviewChange;
        public Action<int> OnSelectChatBotModel;
        public Action<int> OnSelectCharacterModel;
        public override void Render(ModelBase model)
        {
            _dataModel = model as CharacterModelDataModel;
            list.OnFill += OnFillItem;
            list.OnHeight += OnHeightItem;
            list.InitData( _dataModel.ModelId.Count);
        }   
        
        int OnHeightItem(int index) {
            return 100;
        }

        void OnFillItem(int index, GameObject item) {
            item.GetComponent<ModelListItem>().modelId.text = _dataModel.ModelId[index];
            item.GetComponent<ModelListItem>().preview.onClick.RemoveAllListeners();
            item.GetComponent<ModelListItem>().preview.onClick.AddListener(() =>
            {
                OnModelPreviewChange(index);
            });
            
            item.GetComponent<ModelListItem>().selectAsCharacter.onClick.RemoveAllListeners();
            item.GetComponent<ModelListItem>().selectAsCharacter.onClick.AddListener(() =>
            {
                OnSelectCharacterModel(index);
            });
            
            item.GetComponent<ModelListItem>().selectAsChatBot.onClick.RemoveAllListeners();
            item.GetComponent<ModelListItem>().selectAsChatBot.onClick.AddListener(() =>
            {
                OnSelectChatBotModel(index);
            });
        }
        
        public override void OnInit()
        {
        }
    }
}