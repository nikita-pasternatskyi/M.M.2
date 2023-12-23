using UnityEngine;
using UnityEngine.Events;

namespace MM2
{
    public class TriggerOnCollisionEnter : MonoBehaviour
    {
        public UnityEvent OnTriggerEntered;
        
        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEntered?.Invoke();
        }
    }
}
