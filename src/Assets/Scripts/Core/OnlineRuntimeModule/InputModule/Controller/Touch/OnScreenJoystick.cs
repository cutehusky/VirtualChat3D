using Core.OnlineRuntimeModule.InputModule.Model;
using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.OnlineRuntimeModule.InputModule.Controller.Touch
{
    /// <summary>
    /// Base for Joystick input module 
    /// </summary>
    public class OnScreenJoystick : OnScreenTouchBase
    {
        [Header("Joystick settings")]
        [SerializeField] private string onValueChangeTrigger;
        [SerializeField] private float movementRange = 100;
        [SerializeField] protected GameObject gJoyStick;
        [SerializeField] private GameObject gJoyStickBackground;
        [SerializeField] private bool idleHide = true;
        private Vector2 _joyStickBackgroundStartPos;
        private Vector2 _pointerCurrentPos;
        private int _pointerId;

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (_pointerId != 0)
                return;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Parent,
                eventData.position, eventData.pressEventCamera, 
                out _pointerCurrentPos);
            _pointerId = eventData.pointerId;
            gJoyStickBackground.SetActive(true);
            ((RectTransform)gJoyStickBackground.transform).position = eventData.position;
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            if (eventData.pointerId != _pointerId)
                return;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Parent,
                eventData.position, eventData.pressEventCamera, 
                out var vec);
            vec = Vector2.ClampMagnitude(vec - _pointerCurrentPos, movementRange);
            ((RectTransform)gJoyStick.transform).anchoredPosition = vec;
            this.GetModel<PlayerInputAction>().TriggerEvent(onValueChangeTrigger,vec / movementRange);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            if (_pointerId != eventData.pointerId)
                return;
            ((RectTransform)gJoyStick.transform).anchoredPosition = Vector2.zero;
            this.GetModel<PlayerInputAction>().TriggerEvent(onValueChangeTrigger, Vector2.zero);
            _pointerId = 0;
            ((RectTransform)gJoyStickBackground.transform).position = _joyStickBackgroundStartPos;
            if (idleHide)
                gJoyStickBackground.SetActive(false);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _pointerId = 0;  
            gJoyStickBackground.SetActive(!idleHide);
            _joyStickBackgroundStartPos = ((RectTransform)gJoyStickBackground.transform).position;
        }

        private void OnDisable()
        {
            this.GetModel<PlayerInputAction>().TriggerEvent(onValueChangeTrigger, Vector2.zero);
            _pointerId = 0;
        }
    }
}
