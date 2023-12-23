using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MykroFramework.Runtime.Controls
{
    public class InputRouter : SerializedMonoBehaviour
    {
        [SerializeField] public float LeftStickDeadZone = 0.3f;
        [SerializeField] public float RightStickDeadZone = 0.3f;
        [SerializeField] public float LeftTriggerDeadZone = 0.3f;
        [SerializeField] public float RightTriggerDeadZone = 0.3f;
        [SerializeField] public float DpadDeadZone = 0.1f;
        [SerializeField] private bool _initOnAwake;
        [SerializeField] public Controller Controller { get; private set; }

        public ButtonMap CurrentButtonMap => _buttonMap;
        [SerializeField] private ButtonMap _buttonMap;
        public static InputRouter Instance;

        private bool _initialized;

        public event EventHandler<ControllerChangedArgs> ControllerChanged;
        public class ControllerChangedArgs : EventArgs
        {
            public Controller Controller;
            public MykroKeyCode KeyReason;
            public string ButtonReason;

            public ControllerChangedArgs(Controller controller, MykroKeyCode keyReason, string buttonReason)
            {
                Controller = controller;
                KeyReason = keyReason;
                ButtonReason = buttonReason;
            }
        }

        //workaround for getting WasKeyPressed on axis inputs
        private Dictionary<MykroKeyCode, int> _trackedAxisKeys = new Dictionary<MykroKeyCode, int>(10);

        private WaitForEndOfFrame _delay = new WaitForEndOfFrame();

        #region ADDITIONAL_BUTTONS_INPUT_MANAGER_NAMES
        private static string JoystickDPADX = "DPAD X"; //6th axis
        private static string JoystickDPADY = "DPAD Y"; //7th axis

        private static string JoystickLeftStickX = "Left Joy X"; //X
        private static string JoystickLeftStickY = "Left Joy Y"; //Y

        private static string JoystickRightStickX = "Right Joy X"; //5th
        private static string JoystickRightStickY = "Right Joy Y"; //4th

        private static string JoystickRightTrigger = "Right Trigger"; //10th
        private static string JoystickLeftTrigger = "Left Trigger"; //9th

        private static string MouseMovementX = "Mouse X";
        private static string MouseMovementY = "Mouse Y";

        private int[] _keys = (int[])System.Enum.GetValues(typeof(MykroKeyCode));
        #endregion

        public void ChangeButtonMap(ButtonMap newMap)
        {
            if (_buttonMap == newMap)
                return;
            _buttonMap = newMap;
            Init();
        }
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(this);
            }

            if (_initOnAwake)
                Init();
        }

        private void Init()
        {
            _initialized = true;
            RestoreDeadZones();

            if (_buttonMap.LockCursor)
                LockCursor();
            else
                UnlockCursor();

            _buttonMap.LoadInput();
            if (_buttonMap.Values == null)
                _buttonMap.Values = new System.Collections.Generic.Dictionary<string, ButtonState>();
            foreach (var item in _buttonMap.CurrentButtons.Keys)
            {
                if (!_buttonMap.Values.ContainsKey(item))
                    _buttonMap.Values.Add(item, new ButtonState());
            }
        }


        private void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void Update()
        {
            ButtonState FillButtonValue(InputButton button, string buttonName = null)
            {
                var keyboardState = CreateButtonState(button.KeyboardKey, button.Snap, button.InvertKeyboard);
                var gamepadState = CreateButtonState(button.GamepadKey, button.Snap, button.InvertGamepad);

                if (gamepadState.Value != 0 && Controller != Controller.Gamepad)
                {
                    Controller = Controller.Gamepad;
                    ControllerChanged?.Invoke(this, new ControllerChangedArgs(Controller, button.GamepadKey, buttonName));
                }
                else if (keyboardState.Value != 0 && Controller != Controller.Keyboard)
                {
                    Controller = Controller.Keyboard;
                    ControllerChanged?.Invoke(this, new ControllerChangedArgs(Controller, button.KeyboardKey, buttonName));
                }
                return Controller == Controller.Keyboard ? keyboardState : gamepadState;
            }

            if (!_initialized)
                return;

            _buttonMap.UI_Cancel.value = FillButtonValue(_buttonMap.UI_Cancel.button);
            _buttonMap.UI_Confirm.value = FillButtonValue(_buttonMap.UI_Confirm.button);
            _buttonMap.UI_MoveUp.value = FillButtonValue(_buttonMap.UI_MoveUp.button);
            _buttonMap.UI_MoveDown.value = FillButtonValue(_buttonMap.UI_MoveDown.button);
            _buttonMap.UI_MoveLeft.value = FillButtonValue(_buttonMap.UI_MoveLeft.button);
            _buttonMap.UI_MoveRight.value = FillButtonValue(_buttonMap.UI_MoveRight.button);

            if (_buttonMap.ConstantlyCheckAllKeys)
            {
                foreach (var key in _keys)
                {
                    var keycode = (MykroKeyCode)key;
                    if (keycode.IsMouse())
                        continue;
                    var value = IsKeyPressed(keycode);
                    if (value)
                    {
                        if (keycode.IsGamepadKey() && Controller != Controller.Gamepad)
                        {
                            Controller = Controller.Gamepad;
                            ControllerChanged?.Invoke(this, new ControllerChangedArgs(Controller, keycode, null));
                        }
                        else
                        {
                            Controller = Controller.Keyboard;
                            ControllerChanged?.Invoke(this, new ControllerChangedArgs(Controller, keycode, null));
                        }
                    }
                }
            }

            foreach (var buttonName in _buttonMap.CurrentButtons.Keys)
            {
                _buttonMap.CurrentButtons.TryGetValue(buttonName, out InputButton button);
                _buttonMap.Values[buttonName] = FillButtonValue(button);

            }
        }

        private void OnApplicationQuit()
        {
            SaveDeadZones();
        }

        private void RestoreDeadZones()
        {
            LeftStickDeadZone = PlayerPrefs.GetFloat(nameof(LeftStickDeadZone), 0.3f);
            RightStickDeadZone = PlayerPrefs.GetFloat(nameof(RightStickDeadZone), 0.3f);
            LeftTriggerDeadZone = PlayerPrefs.GetFloat(nameof(LeftTriggerDeadZone), 0.3f);
            RightTriggerDeadZone = PlayerPrefs.GetFloat(nameof(RightTriggerDeadZone), 0.3f);
        }

        private void SaveDeadZones()
        {
            PlayerPrefs.SetFloat(nameof(LeftStickDeadZone), LeftStickDeadZone);
            PlayerPrefs.SetFloat(nameof(RightStickDeadZone), RightStickDeadZone);
            PlayerPrefs.SetFloat(nameof(LeftTriggerDeadZone), LeftTriggerDeadZone);
            PlayerPrefs.SetFloat(nameof(RightTriggerDeadZone), RightTriggerDeadZone);
            PlayerPrefs.Save();
        }

        private ButtonState CreateButtonState(MykroKeyCode keyCode, bool snap, bool invert)
        {
            ButtonState buttonState = new ButtonState()
            {
                Value = Mathf.Abs(GetValueFromKeyCode(keyCode, snap, invert)),
                WasJustPressed = WasKeyJustPressed(keyCode),
                WasJustReleased = WasKeyJustReleased(keyCode),
                IsPressed = IsKeyPressed(keyCode),
                IsReleased = IsKeyReleased(keyCode)
            };
            return buttonState;
        }

        public bool WasKeyJustPressed(MykroKeyCode code)
        {
            int value = Input.GetKeyDown((KeyCode)code) ? 1 : 0;
            if (!code.IsAxisInput())
            {
                return value == 1;
            }
            float fValue = GetValueFromKeyCode(code);
            if (_trackedAxisKeys.TryGetValue(code, out int key))
            {
                value = fValue == 1 ? 1 : key;
                if (key != value)
                {
                    _trackedAxisKeys[code] = value;
                    return true;
                }
            }
            else
            {
                _trackedAxisKeys.Add(code, value);
                return value == 1;
            }
            return false;
        }

        public bool WasKeyJustReleased(MykroKeyCode code)
        {
            int value = Input.GetKeyUp((KeyCode)code) ? -1 : 0;
            if (!code.IsAxisInput())
            {
                return value == -1;
            }

            float fValue = GetValueFromKeyCode(code);
            if (_trackedAxisKeys.TryGetValue(code, out int key))
            {
                value = fValue == 0 ? -1 : key;
                if (key != value)
                {
                    _trackedAxisKeys[code] = value;
                    return true;
                }
            }
            else
            {
                _trackedAxisKeys.Add(code, value);
                return value == -1;
            }
            return false;
        }

        public bool IsKeyPressed(MykroKeyCode code)
        {
            return GetValueFromKeyCode(code, false) != 0f;
        }

        public bool IsKeyReleased(MykroKeyCode code) => !IsKeyPressed(code);

        public bool TryGetCurrentlyPressedKey(out MykroKeyCode keyCode)
        {
            keyCode = default;
            for (int i = 0; i < _keys.Length; i++)
            {
                int key = _keys[i];
                MykroKeyCode customKeyCode = (MykroKeyCode)key;
                if (IsKeyPressed(customKeyCode))
                {
                    keyCode = customKeyCode;
                    return true;
                }
            }
            return false;
        }

        private float GetValueFromKeyCode(MykroKeyCode keyCode, bool snap = false, bool invert = false)
        {
            float value = 0;
            float positiveMin = 0;
            float positiveMax = 1;

            float negativeMin = -1;
            float negativeMax = 0;

            float deadzone = 0;

            if (invert)
            {
                positiveMin = -1;
                positiveMax = 0;

                negativeMin = 0;
                negativeMax = 1;
            }
            switch (keyCode)
            {
                default:
                    value = Input.GetKey((UnityEngine.KeyCode)keyCode) == true ? 1 : 0;
                    break;

                case MykroKeyCode.JoystickLeftTrigger:
                    value = Input.GetAxis(JoystickLeftTrigger);
                    deadzone = LeftTriggerDeadZone;
                    break;
                case MykroKeyCode.JoystickRightTrigger:
                    value = Input.GetAxis(JoystickRightTrigger);
                    deadzone = RightTriggerDeadZone;
                    break;

                case MykroKeyCode.MouseWheelScrollPositive:
                    value = Mathf.Clamp(Input.mouseScrollDelta.y, positiveMin, positiveMax);
                    break;
                case MykroKeyCode.MouseWheelScrollNegative:
                    value = Mathf.Clamp(Input.mouseScrollDelta.y, negativeMin, negativeMax);
                    break;

                case MykroKeyCode.MouseMovementYPositive:
                    value = Mathf.Clamp(Input.GetAxisRaw(MouseMovementY), positiveMin, float.MaxValue);
                    break;
                case MykroKeyCode.MouseMovementYNegative:
                    value = Mathf.Clamp(Input.GetAxisRaw(MouseMovementY), float.MinValue, negativeMax);
                    break;

                case MykroKeyCode.MouseMovementXPositive:
                    value = Mathf.Clamp(Input.GetAxisRaw(MouseMovementX), positiveMin, float.MaxValue);
                    break;
                case MykroKeyCode.MouseMovementXNegative:
                    value = Mathf.Clamp(Input.GetAxisRaw(MouseMovementX), float.MinValue, negativeMax);
                    break;

                case MykroKeyCode.JoystickDPADXPositive:
                    value = Mathf.Clamp(Input.GetAxis(JoystickDPADX), positiveMin, positiveMax);
                    deadzone = DpadDeadZone;
                    break;
                case MykroKeyCode.JoystickDPADXNegative:
                    value = Mathf.Clamp(Input.GetAxis(JoystickDPADX), negativeMin, negativeMax);
                    deadzone = DpadDeadZone;
                    break;

                case MykroKeyCode.JoystickDPADYPositive:
                    value = Mathf.Clamp(Input.GetAxis(JoystickDPADY), positiveMin, positiveMax);
                    deadzone = DpadDeadZone;
                    break;
                case MykroKeyCode.JoystickDPADYNegative:
                    value = Mathf.Clamp(Input.GetAxis(JoystickDPADY), negativeMin, negativeMax);
                    deadzone = DpadDeadZone;
                    break;

                case MykroKeyCode.JoystickLeftStickXPositive:
                    value = Mathf.Clamp(Input.GetAxis(JoystickLeftStickX), positiveMin, positiveMax);
                    deadzone = LeftStickDeadZone;
                    break;
                case MykroKeyCode.JoystickLeftStickXNegative:
                    value = Mathf.Clamp(Input.GetAxis(JoystickLeftStickX), negativeMin, negativeMax);
                    deadzone = LeftStickDeadZone;
                    break;

                case MykroKeyCode.JoystickLeftStickYPositive:
                    value = Mathf.Clamp(Input.GetAxis(JoystickLeftStickY), positiveMin, positiveMax);
                    deadzone = LeftStickDeadZone;
                    break;
                case MykroKeyCode.JoystickLeftStickYNegative:
                    value = Mathf.Clamp(Input.GetAxis(JoystickLeftStickY), negativeMin, negativeMax);
                    deadzone = LeftStickDeadZone;
                    break;

                case MykroKeyCode.JoystickRightStickXPositive:
                    value = Mathf.Clamp(Input.GetAxis(JoystickRightStickX), positiveMin, positiveMax);
                    deadzone = RightStickDeadZone;
                    break;
                case MykroKeyCode.JoystickRightStickXNegative:
                    value = Mathf.Clamp(Input.GetAxis(JoystickRightStickX), negativeMin, negativeMax);
                    deadzone = RightStickDeadZone;
                    break;

                case MykroKeyCode.JoystickRightStickYPositive:
                    value = Mathf.Clamp(Input.GetAxis(JoystickRightStickY), positiveMin, positiveMax);
                    deadzone = RightStickDeadZone;
                    break;
                case MykroKeyCode.JoystickRightStickYNegative:
                    value = Mathf.Clamp(Input.GetAxis(JoystickRightStickY), negativeMin, negativeMax);
                    deadzone = RightStickDeadZone;
                    break;
            }

            if (deadzone != 0)
            {
                var abs = Mathf.Abs(value);
                if (abs < deadzone)
                {
                    value = 0;
                }
            }

            if (snap)
            {
                if (value < -deadzone)
                    value = -1;
                if (value > deadzone)
                    value = 1;
                if (value < deadzone && value > -deadzone)
                {
                    value = 0;
                }
            }

            return value;
        }

    }
}
