using MykroFramework.Runtime.Controls;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.SOStateMachine.Conditions
{
    [System.Serializable]
    public class InputCondition : StateTransitionCondition
    {
        private enum ButtonState
        {
            Held,
            Released,
            JustPressed,
            JustReleased,
        }

        [SerializeField, Required] private ButtonMap _buttonMap;
        [SerializeField, ValueDropdown("@_buttonMap.ButtonsArray")] private string[] _buttons;
        [SerializeField] private bool _allButtons;
        [SerializeField] private ButtonState _buttonState;

        public override bool Check()
        {
            if (_allButtons == true)
            {
                return CheckButtons(false);
            }

            return CheckButtons(true);
        }

        private bool CheckButtons(bool awaitedResult)
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                string button = _buttons[i];
                if (ProcessButton(button) == awaitedResult)
                    return awaitedResult;
            }
            return !awaitedResult;
        }

        private bool ProcessButton(string button)
        {
            switch (_buttonState)
            {
                case ButtonState.Held:
                    return _buttonMap.Values[button].IsPressed;
                case ButtonState.Released:
                    return _buttonMap.Values[button].IsReleased;
                case ButtonState.JustPressed:
                    return _buttonMap.Values[button].WasJustPressed;
                case ButtonState.JustReleased:
                    return _buttonMap.Values[button].WasJustReleased;
            }
            return false;
        }
    }
}