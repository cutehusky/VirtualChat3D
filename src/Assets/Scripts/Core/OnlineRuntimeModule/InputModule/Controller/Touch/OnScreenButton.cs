using Core.OnlineRuntimeModule.InputModule.Model;
using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.OnlineRuntimeModule.InputModule.Controller.Touch
{
    /// <summary>
    /// Base for Button input module
    /// </summary>
    public class OnScreenButton : OnScreenTouchBase
    {
        [Header("Button settings")]
        [SerializeField] private string onClickTrigger;
        [SerializeField] private string onTouchTrigger;
        [SerializeField] private string onLongClickTrigger;
        [SerializeField] private string onPointerUpTrigger;
        [SerializeField] private string onPointerDownTrigger;
        [SerializeField] private float longClickThreshold = 1.0f;
        private float _onPointerDownTime;
        private bool _clicked;

        private void Update()
        {
            if (!_clicked) 
                return;
            _onPointerDownTime += Time.deltaTime;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (_onPointerDownTime < longClickThreshold) 
                this.GetModel<PlayerInputAction>().TriggerEvent(onClickTrigger);
            else
                this.GetModel<PlayerInputAction>().TriggerEvent(onLongClickTrigger);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            _clicked = true;
            _onPointerDownTime = 0;
            this.GetModel<PlayerInputAction>().TriggerEvent(onPointerDownTrigger);
            this.GetModel<PlayerInputAction>().TriggerEvent(onTouchTrigger, true);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            _clicked = false;
            _onPointerDownTime = 0;
            this.GetModel<PlayerInputAction>().TriggerEvent(onPointerUpTrigger);
            this.GetModel<PlayerInputAction>().TriggerEvent(onTouchTrigger, false);
        }

        private void OnDisable()
        {
            if (_clicked)
            {
                this.GetModel<PlayerInputAction>().TriggerEvent(onPointerUpTrigger);
                this.GetModel<PlayerInputAction>().TriggerEvent(onTouchTrigger, false);
            }
        }
    }
}