using UnityEngine;

namespace Utilities
{
    public class MonoSingleton<TDerived>: MonoBehaviour 
        where TDerived: MonoBehaviour
    {
        private static TDerived _instance;
        
        /// <summary>
        /// Get Instance's GameObject of singleton
        /// </summary>
        public GameObject InstanceObject => Instance.gameObject;
        
        /// <summary>
        /// Get Instance of singleton
        /// </summary>
        public static TDerived Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                _instance = FindFirstObjectByType(typeof(TDerived)) as TDerived;
                if (_instance == null)
                    _instance = new GameObject(typeof(TDerived).ToString(), typeof(TDerived)).GetComponent<TDerived>();
                return _instance;
            }
        }
        
        /// <summary>
        /// remember call base.Awake() when override
        /// </summary>
        protected virtual void Awake()
        {
            _instance = this as TDerived;
            DontDestroyOnLoad(this);
        }
    }
}