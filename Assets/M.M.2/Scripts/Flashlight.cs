using UnityEngine;
using UnityEngine.Events;

namespace MM2
{
    public class Flashlight : MonoBehaviour
    {
        [SerializeField] private float _batteryLife;
        public UnityEvent<float> BatteryLifeChanged;
        public UnityEvent TurnedOn;
        public UnityEvent TurnedOff;

        public float CurrentBatteryLife { get; private set; }
        private bool _turnedOn = false;

        private void Awake()
        {
            CurrentBatteryLife = _batteryLife;
        }

        private void Update()
        {
            if (_turnedOn)
            {
                CurrentBatteryLife -= Time.deltaTime;
                BatteryLifeChanged?.Invoke(CurrentBatteryLife/_batteryLife);
                if (CurrentBatteryLife <= 0)
                    TurnOff();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Battery battery))
            {
                battery.PickUp(this);
            }
        }

        public void Restore(float batteryLife)
        {
            CurrentBatteryLife = Mathf.Clamp(CurrentBatteryLife + batteryLife, 0, _batteryLife);
            BatteryLifeChanged?.Invoke(CurrentBatteryLife / _batteryLife);
        }

        public void TurnOn()
        {
            if (CurrentBatteryLife <= 0)
                return;
            TurnedOn?.Invoke(); 
            _turnedOn = true;
        }

        public void TurnOff() 
        {
            TurnedOff?.Invoke();
            _turnedOn = false;
        }

        public void Trigger()
        {
            _turnedOn = !_turnedOn;
            if (_turnedOn)
                TurnOn();
            else
                TurnOff();
        }
    }
}
