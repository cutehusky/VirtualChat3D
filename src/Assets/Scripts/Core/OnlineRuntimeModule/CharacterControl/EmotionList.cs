using System;
using Core.OnlineRuntimeModule.InputModule.Model;
using QFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Core.OnlineRuntimeModule.CharacterControl
{
    public class EmotionList: MonoBehaviour, IController
    {
        public Button neutral;
        public Button relax;
        public Button angry;
        public Button surprised;
        public Button sad;
        public Button fun;

        public void Awake()
        {
            neutral.onClick.AddListener(() =>
            {
                this.GetModel<PlayerInputAction>().GetTrigger("Neutral").Trigger();
            });
            relax.onClick.AddListener(() =>
            {
                this.GetModel<PlayerInputAction>().GetTrigger("Relax").Trigger();
            });
            angry.onClick.AddListener(() =>
            {
                this.GetModel<PlayerInputAction>().GetTrigger("Angry").Trigger();
            });
            surprised.onClick.AddListener(() =>
            {
                this.GetModel<PlayerInputAction>().GetTrigger("Surprised").Trigger();
            });
            sad.onClick.AddListener(() =>
            {
                this.GetModel<PlayerInputAction>().GetTrigger("Sad").Trigger();
            });
            fun.onClick.AddListener(() =>
            {
                this.GetModel<PlayerInputAction>().GetTrigger("Fun").Trigger();
            });
        }


        public IArchitecture GetArchitecture()
        {
            return CoreArchitecture.Interface;
        }
    }
}