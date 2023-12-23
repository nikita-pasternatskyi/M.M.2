using UnityEngine;
using UnityEngine.Events;

namespace MM2
{
    public class BasicInteractable : MonoBehaviour, IInteractable
    {
        public UnityEvent Interacted;
        public void Interact()
        {
            Interacted?.Invoke();
        }

        public void LockInteractivity() => Destroy(this);
    }
}
