using System.Collections.Generic;
using Core.MVC;
using QFramework;
using UnityEngine;

namespace Core.OnlineRuntimeModule.InputModule.Model
{
    /// <summary>
    /// Model of Player Input
    /// </summary>
    public class PlayerInputAction: ModelBase
    {
        /// <summary>
        /// whether current cursor locked
        /// </summary>
        public bool CursorLocked;
        private Dictionary<string, string> _inputGroup; // name - group name
        private Dictionary<string, bool> _inputGroupState; // group name - state (enable/disable)
        private Dictionary<string, EasyEvent<Vector2>> _vector2Event;
        private Dictionary<string, EasyEvent<bool>> _boolEvent;
        private Dictionary<string, EasyEvent> _trigger;
       
        public void SetActive(string groupName, bool active)
        {
            _inputGroupState[groupName] = active;
        }

        public void SetGroup(string target, string groupName, bool state = true)
        {
            _inputGroup[target] = groupName;
            _inputGroupState.TryAdd(groupName, state);
        }

        public void TriggerEvent(string name, Vector2 data)
        {
            if (_inputGroup.TryGetValue(name, out var group) 
                && _inputGroupState.TryGetValue(group, out var active)
                && !active)
                return;
            if ( _vector2Event.TryGetValue(name, out var res))
                res.Trigger(data);
            else
                AddVector2Event(name).Trigger(data);
        }

        public void TriggerEvent(string name, bool data)
        {
            if (_inputGroup.TryGetValue(name, out var group) 
                && _inputGroupState.TryGetValue(group, out var active)
                && !active)
                return;
            if ( _boolEvent.TryGetValue(name, out var res) )
                res.Trigger(data);
            else
                AddBoolEvent(name).Trigger(data);
        }
        public void TriggerEvent(string name)
        {
            if (_inputGroup.TryGetValue(name, out var group) 
                && _inputGroupState.TryGetValue(group, out var active)
                && !active)
                return;
            if ( _trigger.TryGetValue(name, out var res) )
                res.Trigger();
            else
                AddTrigger(name).Trigger();
        }
        
        private EasyEvent<Vector2> AddVector2Event(string name)
        {
            var easyEvent = new EasyEvent<Vector2>();
            _vector2Event.Add(name, easyEvent);
            return easyEvent;
        }
        
        private EasyEvent<bool> AddBoolEvent(string name)
        {
            var easyEvent = new EasyEvent<bool>();
            _boolEvent.Add(name, easyEvent);
            return easyEvent;
        }
        
        private EasyEvent AddTrigger(string name)
        {
            var easyEvent = new EasyEvent();
            _trigger.Add(name, easyEvent);
            return easyEvent;
        }
        
        public EasyEvent GetTrigger(string name)
        {
            return _trigger.TryGetValue(name, out var res) ? res : AddTrigger(name);
        }
        
        public EasyEvent<Vector2> GetVector2Event(string name)
        {
            return _vector2Event.TryGetValue(name, out var res) ? res : AddVector2Event(name);
        }
        
        public EasyEvent<bool> GetBoolEvent(string name)
        {
            return _boolEvent.TryGetValue(name, out var res) ? res : AddBoolEvent(name);
        }

        protected override void OnInit()
        {
            _inputGroupState = new();
            _inputGroup = new();
            _vector2Event = new();
            _boolEvent = new();
            _trigger = new();
        }
    }
}