using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.OnlineRuntimeModule.EnvironmentCustomize.View
{
    public class ObjectPuttingArea: MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public Action<GameObject> OnObjectMoved;
        public Action<GameObject> OnObjectRotated;
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        public void OnDrag(PointerEventData eventData)
        {
        }
    }
}