using System;
using Core.OnlineRuntimeModule.EnvironmentCustomize.Model;
using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Core.OnlineRuntimeModule.EnvironmentCustomize.View
{
    public class ObjectListItem : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler, IController
    {
        public GameObject canvas;
        public Image iconImage;
        public GameObject targetPrefab;
        private bool _isValidPutting;
        private GameObject _currentPuttingObject;
        [SerializeField] private GameObject iconObject;
        [SerializeField] private LayerMask ignore;
        public Action<ItemObject> OnPuttingSuccess;

        public void OnDrag(PointerEventData eventData)
        {
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle((RectTransform)transform,
                    eventData.position, eventData.pressEventCamera, 
                    out var globalMousePos))
                iconObject.transform.position = globalMousePos;
            var curObject = eventData.pointerEnter;
            Debug.Log(curObject);
            if (curObject != null)
            {
                if (curObject.name == "PuttingArea")
                {
                    Camera cam = Camera.main;
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out var hit,1000f, ~ignore))
                    {
                        Debug.Log(hit.point);
                        if (_currentPuttingObject == null)
                        {
                            Debug.Log(targetPrefab);
                            _currentPuttingObject = Instantiate(targetPrefab, hit.point, Quaternion.identity);
                        }
                        else
                        {
                            _currentPuttingObject.transform.position = hit.point;
                        }
                        this.GetModel<EnvironmentDataModel>().IsPlacingItem = true;
                    }
                    iconImage.color = Color.clear;
                    if (!_isValidPutting)
                        _currentPuttingObject.SetActive(true);
                    _isValidPutting = true;
                }
                else
                {
                    if (_isValidPutting && _currentPuttingObject)
                        _currentPuttingObject.SetActive(false);
                    _isValidPutting = false;
                    iconImage.color = Color.white;
                }
            }
        }

        private Vector3 _startPos;
        private int _pointerId;
        public void OnBeginDrag(PointerEventData eventData)
        {
            _isValidPutting = false;
            _startPos = iconObject.transform.position;
            iconObject.transform.SetParent(canvas.transform);
            iconObject.transform.SetAsLastSibling();
            _pointerId = eventData.pointerId;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_pointerId == 0
                || _pointerId != eventData.pointerId)
                return;
            iconObject.transform.SetParent(transform);
            iconObject.transform.position = _startPos;
            _pointerId = 0;
            if (_currentPuttingObject)
            {
                var item = _currentPuttingObject.GetComponent<ItemObject>();
                item.meshRenderer.enabled = false;
                if (!_isValidPutting || !item.isValid)
                {
                    foreach (var obj in item.CollisionObject)
                    {
                        if (obj.TryGetComponent<ItemObject>(out var otherItem))
                            otherItem.CollisionObject.Remove(_currentPuttingObject);
                    }
                    Destroy(_currentPuttingObject);
                }
                else
                {
                    OnPuttingSuccess(item);
                }
            }
            _isValidPutting = false;
            _currentPuttingObject = null;
            iconImage.color = Color.white;
            this.GetModel<EnvironmentDataModel>().IsPlacingItem = false;
        }

        public IArchitecture GetArchitecture()
        {
            return CoreArchitecture.Interface;
        }
    }
}
