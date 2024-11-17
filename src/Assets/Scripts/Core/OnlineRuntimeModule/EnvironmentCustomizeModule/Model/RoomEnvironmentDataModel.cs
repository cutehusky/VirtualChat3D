using System.Collections.Generic;
using Core.MVC;
using UnityEngine;

namespace Core.OnlineRuntimeModule.EnvironmentCustomizeModule.Model
{
    public class RoomEnvironmentDataModel: ModelBase
    {
        public string CurrentEditingRoomId;
        public List<RoomEnvironmentItemData> CurrentEditingEnvironmentData = new();
        public GameObject CurrentSelectObject;
        public List<GameObject> ObjectInScene = new();

        protected override void OnInit()
        {
            
        }
    }
}