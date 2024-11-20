using Core.MVC;
using TMPro;
using UnityEngine.UI;

namespace Core.OnlineRuntimeModule.ActivitiesModule.Voting
{
    public class VotingView: ViewBase
    {
        public Button yes;
        public Button no;
        public TMP_Text time;
        public TMP_Text note;
        public TMP_Text result;
        
        public override void Render(ModelBase model)
        {
            
        }

        public override void OnInit()
        {
        }
    }
}