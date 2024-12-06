using System.Collections.Generic;
using System.Linq;
using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.OnlineRuntimeModule.InputModule.Controller.Touch
{
    /// <summary>
    /// Base for all touch component
    /// remember ignore raycast target in all children to prevent infinite loop 
    /// </summary>
    public class OnScreenTouchBase: MonoBehaviour, IController ,
        IPointerClickHandler ,IPointerDownHandler,IPointerUpHandler, IDragHandler
    {
        [SerializeField] private bool raycastGoThrough;
        [SerializeField] private List<GameObject> raycastTarget = new();
        protected RectTransform Parent;
        
        private void PassEvent<T>(PointerEventData data,ExecuteEvents.EventFunction<T> function)
            where T : IEventSystemHandler
        {
            if (!raycastGoThrough)
                return;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, results); 
            foreach (var t in results.Where(t => raycastTarget.Contains(t.gameObject)))
            {
                ExecuteEvents.Execute(t.gameObject, data, function);
            }
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            PassEvent(eventData,ExecuteEvents.submitHandler);
            PassEvent(eventData,ExecuteEvents.pointerClickHandler);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            PassEvent(eventData,ExecuteEvents.pointerDownHandler);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            PassEvent(eventData,ExecuteEvents.pointerUpHandler);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            PassEvent(eventData,ExecuteEvents.dragHandler);
        }

        protected virtual void OnEnable()
        {
            Parent = transform.parent.GetComponentInParent<RectTransform>();
        }

        public IArchitecture GetArchitecture()
        {
            return CoreArchitecture.Interface;
        }
    }
}