using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.OnlineRuntimeModule.RoomManagementModule.View
{
    public class RoomListItem: MonoBehaviour
    {
        public TMP_Text roomId;
        public Toggle roomAccessType;
        public Button delete;
        public Button host;
        public Button editEnvironment;
        public Image hostIcon;
    }
}