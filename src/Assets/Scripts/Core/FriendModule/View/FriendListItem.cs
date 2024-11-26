using Core.MVC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.FriendModule.View
{
    public class FriendListItem: MonoBehaviour
    {
        public TMP_Text userId;
        public TMP_Text username;
        public TMP_Text description;
        public Image avatar;
        public TMP_Text dateOfBirth;
        public Button button1; // remove friend/remove friend request   
        public Button button2; // for send message/accept friend request
    }
}