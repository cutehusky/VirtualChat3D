using System;
using QFramework;
using UnityEngine;
using Utilities;

namespace Core.MVC
{
    /// <summary>
    /// Base class of controller
    /// </summary>
    public abstract class MonoSingletonControllerBase: MonoSingleton<MonoSingletonControllerBase>, IController
    {
        protected override void Awake()
        {
            base.Awake();
            OnInit();
        }

        public abstract void OnInit();
        
        public IArchitecture GetArchitecture()
        {
            return CoreArchitecture.Interface;
        }
    }
}