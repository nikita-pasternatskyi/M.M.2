using UnityEngine;
using UnityEngine.Events;

namespace MM2
{
    public class CallOnUnityEvent : MonoBehaviour
    {
        public UnityEvent OnAwake;
        public UnityEvent OnStart;

        private void Awake()
        {
            OnAwake?.Invoke();
        }

        private void Start()
        {
            OnStart?.Invoke();
        }
    }
}
