using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.AdminModule.View
{
    public class UserListItem: MonoBehaviour
    {
        public TMP_Text userId;
        public TMP_Text username;
        public TMP_Text description;
        public Image avatar;
        public TMP_Text dateOfBirth;
        public Button button1; // lock/unlock
        public Button button2; // remove
        public Image button1Icon; 
    }
}