using Core.MVC;
using Core.UserAccountModule.Model;
using TMPro;
using UI.Dates;
using UnityEngine.UI;

namespace Core.UserAccountModule.View
{
    public class UserProfileView: ViewBase
    {
        public TMP_Text userId;
        public TMP_InputField username;
        public TMP_InputField description;
        public Image avatar;
        public DatePicker dateOfBirth;
        public Button resetPassword;
        public override void Render(ModelBase model)
        {
            var userProfileModel = model as UserProfileDataModel;
            //
        }

        public override void OnInit()
        {
            
        }
    }
}