using System;
using Core.UI;
using UnityEngine;
using Utilities;

namespace Core.MVC
{
    /// <summary>
    /// Base class of View component
    /// Contain all reference to UI component of a view
    /// </summary>
    public abstract class ViewBase: MonoBehaviour
    {
        public Footer footer;
        public Header header;


        public virtual void MoveUpWhenOpenKeyboard(float height) {}
        
        public abstract void Render(ModelBase model);

        public void Display()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        public abstract void OnInit();

        protected void Awake()
        {
            OnInit();
        }
    }
}