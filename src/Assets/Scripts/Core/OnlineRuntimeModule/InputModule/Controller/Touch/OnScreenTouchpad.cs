using System.Collections.Generic;
using Core.OnlineRuntimeModule.InputModule.Model;
using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.OnlineRuntimeModule.InputModule.Controller.Touch
{
    /// <summary>
    /// Touchpad input module
    /// </summary>
    public class OnScreenTouchpad : OnScreenTouchBase
    {
        private int _pointerId;
        private Vector2 _pointerCurrentPos;
        [SerializeField] private string onValueChangeTrigger;

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (_pointerId != 0)
                return;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Parent
                , eventData.position, eventData.pressEventCamera, 
                out var vec);
            _pointerId = eventData.pointerId;
            _pointerCurrentPos = vec;
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            if (_pointerId == 0)
                return;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Parent,
                eventData.position, eventData.pressEventCamera,
                out var position);
            var vec = position - _pointerCurrentPos;
            vec.y *= -1;
            this.GetModel<PlayerInputAction>().TriggerEvent(onValueChangeTrigger, vec);
            _pointerCurrentPos = position;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            _pointerId = 0;
            this.GetModel<PlayerInputAction>().TriggerEvent(onValueChangeTrigger, Vector2.zero);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _pointerId = 0;
        }

        private void OnDisable()
        {
            this.GetModel<PlayerInputAction>().TriggerEvent(onValueChangeTrigger, Vector2.zero);
            _pointerId = 0;
        }
    }
}
