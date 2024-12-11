using System.Collections.Generic;
using Core.MVC;
using Core.OnlineRuntimeModule.EnvironmentCustomize.Model;
using Core.OnlineRuntimeModule.EnvironmentCustomize.View;
using Core.UserAccountModule.Model;
using QFramework;
using UnityEngine;

namespace Core.OnlineRuntimeModule.EnvironmentCustomize.Controller
{
    public class EnvironmentObjectManager: ControllerBase
    {
        private EnvironmentEditView _environmentEditView;
      
        public void SaveRoomEnvironmentData()
        {
            this.GetModel<EnvironmentDataModel>().SaveRoomEnvironmentData(
                this.GetModel<UserProfileDataModel>().UserProfileData.UserId);
        }

        public void LoadRoomEnvironmentData(string roomId)
        {
            this.GetModel<EnvironmentDataModel>().CurrentEditingRoomId = roomId;
            this.GetModel<EnvironmentDataModel>()
                .FetchRoomsEnvironment(this.GetModel<UserProfileDataModel>().UserProfileData.UserId
                , () =>
                {
                    _environmentEditView.LoadItem();
                });
        }
        

        public ViewBase OpenEnvironmentEditView(string roomId)
        {
            AppMain.Instance.CloseCurrentView();
            LoadRoomEnvironmentData(roomId);
            _environmentEditView.Display();
            _environmentEditView.Render(this.GetModel<EnvironmentDataModel>());
            return _environmentEditView;
        }
        
        public override void OnInit(List<ViewBase> view)
        {
            _environmentEditView = view[0] as EnvironmentEditView;
            _environmentEditView.OnPuttingItemSuccess += (go) =>
            {
                var data = go.ExportData();
                this.GetModel<EnvironmentDataModel>().CurrentEditingEnvironmentData.Add(data);
                this.GetModel<EnvironmentDataModel>().InSceneObject.Add(go.gameObject, data);
            };
            _environmentEditView.objectPuttingArea.OnModified += (item) =>
            {
                this.GetModel<EnvironmentDataModel>().InSceneObject[item.gameObject].Copy(item.ExportData());
            };
            _environmentEditView.back.onClick.AddListener(() =>
            {
                AppMain.Instance.OpenHostRoomView();
            });
            _environmentEditView.save.onClick.AddListener(SaveRoomEnvironmentData);
        }
    }
}