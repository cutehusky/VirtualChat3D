using Core.MVC;
using UnityEngine;
using UnityEngine.UI;

namespace Core.OnlineRuntimeModule.CharacterControl
{
    public class CharacterControlView: ViewBase
    {
        public Button outRoom;
        public Button openUserList;
        public Button openChat;
        public Button openEmotion;
        public Button openAnimation;
        public EmotionList emotionList;
        public AnimationList animationList; 
      
        public override void Render(ModelBase model)
        {
           
        }
        
        public override void OnInit()
        {
            
        }
    }
}