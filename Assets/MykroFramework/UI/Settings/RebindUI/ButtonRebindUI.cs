using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MEC;
using UnityEngine.Events;

namespace MykroFramework.Runtime.Controls.RebindUI
{

    public class ButtonRebindUI : MonoBehaviour
    {
        public Controller Controller;
        [Required] public ButtonMap Map;
        [ValueDropdown("@Map.ButtonsArray")] public string Button;

        [SerializeField] private TextMeshProUGUI _buttonLabel;
        [SerializeField] private TextMeshProUGUI _keyLabel;
        [SerializeField] private GameObject _button;
        [SerializeField] public UnityEvent RebindFinished;
        private CoroutineHandle _coroutineHandle;

        private void OnValidate()
        {
            UpdateUI();
        }

        private void Awake()
        {
            Map.LoadInput();
            UpdateUI();
        }

        private void OnEnable()
        {
            Map.BindingsChanged += OnBindingsChanged;
        }

        private void OnDisable()
        {
            Map.BindingsChanged -= OnBindingsChanged;
        }

        public void UpdateUI()
        {
            _buttonLabel.text = Button;
            switch (Controller)
            {
                case Controller.Keyboard:
                    _keyLabel.text = Map.CurrentButtons[Button].KeyboardKey.ToString();
                    break;
                case Controller.Gamepad:
                    _keyLabel.text = Map.CurrentButtons[Button].GamepadKey.ToString();
                    break;
            }
        }

        private void OnBindingsChanged(Dictionary<string, InputButton> obj)
        {
            UpdateUI();
        }

        private IEnumerator<float> Rebind()
        {
            float rebindTimer = 0;
            float rebindTime = 5;

            float startWaitTimer = 0;
            float startWaitTime = 0.3f;

            float coolDownTimer = 0;
            float coolDownTime = 0.25f;

            if(_button != null)
                _button.SetActive(false);

            var wasPressed = InputRouter.Instance.TryGetCurrentlyPressedKey(out MykroKeyCode startKey);

            if (wasPressed)
            {
                while (startWaitTimer < startWaitTime)
                {
                    startWaitTimer += Time.unscaledDeltaTime;

                    if (InputRouter.Instance.TryGetCurrentlyPressedKey(out MykroKeyCode newKey))
                    {
                        if (newKey != startKey)
                        {
                            break;
                        }
                    }
                    yield return Timing.WaitForOneFrame;
                }
            }
                
            while (rebindTimer < rebindTime)
            {
                rebindTimer += Time.unscaledDeltaTime;
                if (InputRouter.Instance.TryGetCurrentlyPressedKey(out MykroKeyCode key))
                {
                    RebindKey(key);
                    break;
                }
                yield return Timing.WaitForOneFrame;
            }
            while (coolDownTimer < coolDownTime)
            {
                coolDownTimer += Time.unscaledDeltaTime; 
                yield return Timing.WaitForOneFrame;
            }
            RebindFinished?.Invoke();
            if (_button != null)
                _button.SetActive(true);
            yield break;

        }

        public void Press()
        {
            if (_coroutineHandle.IsRunning)
                return;
            _coroutineHandle = Timing.RunCoroutine(Rebind());
        }
        private void RebindKey(MykroKeyCode key)
        {
            if (Controller == Controller.Gamepad && key.IsGamepadKey())
                Map.Remap(Button, key);
            if (Controller != Controller.Gamepad && !key.IsGamepadKey())
                Map.Remap(Button, key);
        }

    }
}
