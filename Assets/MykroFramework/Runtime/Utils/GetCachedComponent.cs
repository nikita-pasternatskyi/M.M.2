using System.Collections.Generic;
using UnityEngine;

namespace MykroFramework.Runtime.Utils
{
    public class GetCachedComponent : MonoBehaviour
    {
        private Dictionary<System.Type, Component> _components = new Dictionary<System.Type, Component>();

        public new Component GetComponent(System.Type type)
        {
            if (_components.TryGetValue(type, out Component value))
            {
                return value;
            }
            if (base.TryGetComponent(type, out Component component))
            {
                _components.Add(type, component);
                return component;
            }
            return null;
        }

        public new T GetComponent<T>() where T : Component
        {
            if (_components.TryGetValue(typeof(T), out Component value))
            {
                return (T)value;
            }
            if (base.TryGetComponent<T>(out T component))
            {
                _components.Add(typeof(T), component);
                return component;
            }
            return null;
        }
    }
}