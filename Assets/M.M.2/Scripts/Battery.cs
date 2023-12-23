using UnityEngine;
using UnityEngine.Events;

namespace MM2
{
    public class Battery : MonoBehaviour
    {
        [SerializeField] private float _batteryLife;
        public UnityEvent PickedUp;

        public void PickUp(Flashlight flashlight)
        {
            PickedUp?.Invoke();
            flashlight.Restore(_batteryLife);
        }
    }
}
