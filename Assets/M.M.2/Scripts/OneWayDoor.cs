using UnityEngine;
using UnityEngine.Events;

namespace MM2
{
    public class OneWayDoor : MonoBehaviour, IInteractable
    {
        public int Durability = 3;
        public UnityEvent Hit;
        public UnityEvent Destroyed;
        public UnityEvent Opened;
        public UnityEvent Closed;
        private bool _interactable = true;

        public void PermanentlyLock()
        {
            Destroy(this);
        }

        public void TakeHit()
        {
            Durability = Mathf.Clamp(--Durability, 0, Durability);
            Hit?.Invoke();
            if(Durability == 0)
            {
                Destroyed?.Invoke();
            }
        }

        public void Interact()
        {
            if (!_interactable)
                return;
            Opened?.Invoke();
            _interactable = false;
        }

        public void Open()
        {
            if (!enabled)
                return;
            Opened?.Invoke();
        }

        public void Close()
        {
            if (!enabled)
                return;
            Closed?.Invoke();
            _interactable = false;
        }
    }
}
