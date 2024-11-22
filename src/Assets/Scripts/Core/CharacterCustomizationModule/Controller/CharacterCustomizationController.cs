using System;
using System.Collections.Generic;
using System.IO;
using Core.CharacterCustomizationModule.Model;
using Core.CharacterCustomizationModule.View;
using Core.MVC;
using JimmysUnityUtilities;
using QFramework;
using UIS;
using UnityEngine;

namespace Core.CharacterCustomizationModule.Controller
{
    public class CharacterCustomizationController: ControllerBase
    {
        private ModelListView _modelListView;
        public void LoadModelFromExternal()
        {
            NativeFilePicker.PickFile((path =>
            {
                if (path == null)
                    Debug.LogError("Fail");
                else
                {
                    byte[] bytes = File.ReadAllBytes(path);
                    File.WriteAllBytes(Application.persistentDataPath 
                                       + CharacterModelDataModel.ModelPath 
                                       + Path.GetFileName(path), bytes);
                    OpenModelListView();
                }
            }));
        }

        public void SelectChatRoomModel(string id)
        {
            this.GetModel<CharacterModelDataModel>().ChatRoomSelectModelId = id;
        }
        
        public void SelectChatBotModel(string id)
        {
            this.GetModel<CharacterModelDataModel>().ChatBotSelectModelId = id;
        }

        public void PreviewModel(string id)
        {
            this.GetModel<CharacterModelDataModel>().CreateCharacter(id,(model =>
            {
                _modelListView.previewModelPoint.DestroyAllChildren();
                model.transform.SetParent(_modelListView.previewModelPoint, false);
            }));
        }
        
        public override void OnInit(List<ViewBase> view)
        {
            _modelListView = view[0] as ModelListView;
            _modelListView.import.onClick.AddListener((LoadModelFromExternal));
            _modelListView.OnModelPreviewChange += (i) =>
            {
                PreviewModel(this.GetModel<CharacterModelDataModel>().ModelId[i]);
            };
            _modelListView.OnSelectCharacterModel += (i) =>
            {
                SelectChatRoomModel(this.GetModel<CharacterModelDataModel>().ModelId[i]);
            };
            _modelListView.OnSelectChatBotModel += (i) =>
            {
                SelectChatBotModel(this.GetModel<CharacterModelDataModel>().ModelId[i]);
            };
        }

        public ViewBase OpenModelListView()
        {
            this.GetModel<CharacterModelDataModel>().LoadModelList();
            _modelListView.Display();
            _modelListView.Render(this.GetModel<CharacterModelDataModel>());
            return null;
        }
    }
}