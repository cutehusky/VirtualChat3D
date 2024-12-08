using Core.OnlineRuntimeModule.InputModule.Model;
using QFramework;
using UnityEngine;

namespace Core.InputModule.Component.Sensor
{
    /// <summary>
    /// Gyroscope Input process
    /// </summary>
    public class GyroscopeInput : MonoBehaviour, IController
    {
        [SerializeField] private string triggerName;
        private Gyroscope _gyroscope;
        private void Awake()
        {
            _gyroscope = Input.gyro;
        }

        private void OnDisable()
        {
            _gyroscope.enabled = false;
        }

        private void Update()
        {
            if (!_gyroscope.enabled)
                _gyroscope.enabled = true;
            var angle = _gyroscope.rotationRateUnbiased;
            this.GetModel<PlayerInputAction>().TriggerEvent(triggerName, new Vector2(angle.y, angle.x));
        }

        public IArchitecture GetArchitecture()
        {
            return CoreArchitecture.Interface;
        }
    }
}
