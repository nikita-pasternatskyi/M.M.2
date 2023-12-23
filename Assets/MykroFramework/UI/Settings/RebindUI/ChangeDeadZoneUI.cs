using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace MykroFramework.Runtime.Controls.RebindUI
{
    public class ChangeDeadZoneUI : MonoBehaviour
    {
        [SerializeField, Required] private Slider _leftStick;
        [SerializeField, Required] private Slider _rightStick;
        [SerializeField, Required] private Slider _leftTrigger;
        [SerializeField, Required] private Slider _rightTrigger;
        [SerializeField, Required] private Slider _dpad;

        private void Start()
        {
            Debug.Log(InputRouter.Instance.LeftTriggerDeadZone);
            _leftStick.value = InputRouter.Instance.LeftStickDeadZone;
            _rightStick.value = InputRouter.Instance.RightStickDeadZone;
            _leftTrigger.value = InputRouter.Instance.LeftTriggerDeadZone;
            _rightTrigger.value = InputRouter.Instance.RightTriggerDeadZone;
            _dpad.value = InputRouter.Instance.DpadDeadZone;
        }

        private void OnEnable()
        {
            _leftStick.onValueChanged.AddListener(SetLeftStickDeadZone);
            _rightStick.onValueChanged.AddListener(SetRightStickDeadZone);
            _leftTrigger.onValueChanged.AddListener(SetLeftTriggerDeadZone);
            _rightTrigger.onValueChanged.AddListener(SetRightTriggerDeadZone);
            _dpad.onValueChanged.AddListener(SetDpadDeadZone);
        }

        private void OnDisable()
        {
            _leftStick.onValueChanged.RemoveListener(SetLeftStickDeadZone);
            _rightStick.onValueChanged.RemoveListener(SetRightStickDeadZone);
            _leftTrigger.onValueChanged.RemoveListener(SetLeftTriggerDeadZone);
            _rightTrigger.onValueChanged.RemoveListener(SetRightTriggerDeadZone);
            _dpad.onValueChanged.RemoveListener(SetDpadDeadZone);
        }

        public void SetLeftStickDeadZone(float newValue)
        {
            InputRouter.Instance.LeftStickDeadZone = newValue;
        }
        public void SetRightStickDeadZone(float newValue)
        {
            InputRouter.Instance.RightStickDeadZone = newValue;
        }
        public void SetRightTriggerDeadZone(float newValue)
        {
            InputRouter.Instance.RightTriggerDeadZone = newValue;
        }
        public void SetLeftTriggerDeadZone(float newValue)
        {
            InputRouter.Instance.LeftTriggerDeadZone = newValue;
        }
        public void SetDpadDeadZone(float newValue)
        {
            InputRouter.Instance.DpadDeadZone = newValue;
        }
    }
}
