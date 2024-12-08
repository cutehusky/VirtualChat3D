using System.Collections.Generic;
using Core.MVC;
using Core.OnlineRuntimeModule.EnvironmentCustomize.Model;
using Core.OnlineRuntimeModule.EnvironmentCustomize.View;
using QFramework;
using UnityEngine;

namespace Core.OnlineRuntimeModule.EnvironmentCustomize.Controller
{
    public class EnvironmentObjectManager: ControllerBase
    {
        private EnvironmentEditView _environmentEditView;
      
        public void SaveRoomEnvironmentData()
        {
            
        }

        public void LoadRoomEnvironmentData(string roomId)
        {
            
        }
        
        public override void OnInit(List<ViewBase> view)
        {
            _environmentEditView = view[0] as EnvironmentEditView;
            _environmentEditView.OnPuttingItemSuccess += (go) =>
            {
                this.GetModel<EnvironmentDataModel>().CurrentEditingEnvironmentData.Add(go.ExportData());
                Debug.Log(this.GetModel<EnvironmentDataModel>().CurrentEditingEnvironmentData.Count);
            };
        }
    }
}