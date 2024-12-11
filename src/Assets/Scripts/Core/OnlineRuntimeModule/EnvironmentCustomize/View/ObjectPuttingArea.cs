using System;
using Core.OnlineRuntimeModule.EnvironmentCustomize.Model;
using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.OnlineRuntimeModule.EnvironmentCustomize.View
{
    public class ObjectPuttingArea:  MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler, IController
    {
        public LayerMask ignore;
        private Vector3 _originMousePos;
        private Vector3 _originPos;
        private float _originRot;
        public ItemObject rotSelected;
        public ItemObject moveSelected;
        public Action<ItemObject> OnModified;


        public bool isRotate;

        public void Awake()
        {
            gameObject.name = "PuttingArea";
        }

        public void SetRotate(bool rotate)
        {
            isRotate = rotate;
            RestoreMove();
            RestoreRot();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Camera cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (this.GetModel<EnvironmentDataModel>().IsPlacingItem)
                return;
            RaycastHit hit;
            if (isRotate)
            {
                if (moveSelected != null)
                    return;
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log(hit.transform.gameObject);
                    if (hit.transform.TryGetComponent<ItemObject>(out var target))
                    {
                        rotSelected = target;
                        rotSelected.meshRenderer.enabled = true;
                        Debug.Log(target);
                        _originMousePos = Input.mousePosition;
                        _originRot = rotSelected.transform.rotation.eulerAngles.y;
                    }
                }
                return;
            }
       
            if (rotSelected != null)
                return;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.gameObject);
                if (hit.transform.TryGetComponent<ItemObject>(out var target))
                {
                    moveSelected = target;
                    moveSelected.meshRenderer.enabled = true;
                    Debug.Log(target);
                    _originPos = moveSelected.transform.position;
                }
            }
        }

        public void RestoreMove()
        {
            if (moveSelected)
            {
                moveSelected.meshRenderer.enabled = false;
                if (!moveSelected.isValid)
                    moveSelected.transform.position = _originPos;
                else
                    OnModified(moveSelected);
                moveSelected = null;
            }
        }
        
        public void RestoreRot()
        {
            if (rotSelected)
            {
                rotSelected.meshRenderer.enabled = false;
                if (!rotSelected.isValid)
                    rotSelected.transform.rotation = Quaternion.Euler(0,_originRot,0);
                else
                    OnModified(rotSelected);
                rotSelected = null;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (moveSelected)
            {
                Camera cam = Camera.main;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit, 1000f, ~ignore))
                {
                    moveSelected.transform.position = hit.point;
                }
            }
            if (rotSelected)
            {
                rotSelected.transform.rotation = Quaternion.Euler(0,
                    _originRot +  (_originMousePos.x - Input.mousePosition.x),0);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            RestoreMove();
            RestoreRot();
        }

        public IArchitecture GetArchitecture()
        {
            return CoreArchitecture.Interface;
        }
    }
}