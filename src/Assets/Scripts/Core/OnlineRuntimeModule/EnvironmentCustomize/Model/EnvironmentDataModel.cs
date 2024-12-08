using System.Collections.Generic;
using Core.MVC;
using UnityEngine;

namespace Core.OnlineRuntimeModule.EnvironmentCustomize.Model
{
    public class EnvironmentDataModel: ModelBase
    {
        public string CurrentEditingRoomId;
        public List<EnvironmentItemData> CurrentEditingEnvironmentData;
        public bool IsPlacingItem;

        protected override void OnInit()
        {
            CurrentEditingEnvironmentData = new();
        }
    }
}