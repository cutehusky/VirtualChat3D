using Core.OnlineRuntimeModule.InputModule.Model;
using QFramework;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Core.OnlineRuntimeModule.CharacterControl
{
    public class AnimationList: MonoBehaviour, IController
    {
        public Button laughing;
        public Button applause;
        public Button crying;
        public Button waves;

        public void Awake()
        {
            laughing.onClick.AddListener(() =>
            {
                this.GetModel<PlayerInputAction>().GetTrigger("Laughing").Trigger();
            });
            applause.onClick.AddListener(() =>
            {
                this.GetModel<PlayerInputAction>().GetTrigger("Applause").Trigger();
            });
            crying.onClick.AddListener(() =>
            {
                this.GetModel<PlayerInputAction>().GetTrigger("Crying").Trigger();
            });
            waves.onClick.AddListener(() =>
            {
                this.GetModel<PlayerInputAction>().GetTrigger("Waves").Trigger();
            });
        }


        public IArchitecture GetArchitecture()
        {
            return CoreArchitecture.Interface;
        }
    }
}