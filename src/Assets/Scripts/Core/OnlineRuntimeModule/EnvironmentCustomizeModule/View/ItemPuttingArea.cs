using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.OnlineRuntimeModule.EnvironmentCustomizeModule.View
{
    public class ItemPuttingArea: MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
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