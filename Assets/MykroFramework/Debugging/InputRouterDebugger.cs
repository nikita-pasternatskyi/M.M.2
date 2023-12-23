using MykroFramework.Runtime.Controls;
using UnityEngine;

namespace MykroFramework.Debugging
{

    public class InputRouterDebugger : IDebuggable
    {
        private DebugTextBlock _textBlock;

        private const string DEBUG_NAME = "==INPUT ROUTER DEBUG==";
        private const string CURRENT_MAP_FIELD = "CURRENT INPUT MAP:";
        
        private const string LEFT_STICK_DEADZONE = "LEFT STICK DEAD ZONE: ";
        private const string RIGHT_STICK_DEADZONE = "RIGHT STICK DEAD ZONE: ";
        private const string DPAD_DEADZONE = "DPAD DEAD ZONE: ";
        private const string LEFT_TRIGGER_DEADZONE = "LEFT TRIGGER DEAD ZONE: ";
        private const string RIGHT_TRIGGER_DEADZONE = "RIGHT TRIGGER DEAD ZONE: ";

        private const string CURRENT_CONTROLLER = "CURRENT CONTROLLER: ";
        private const string BUTTON_REASON_CONTROLLER_CHANGED = "BUTTON WHY CONTROLLER CHANGED: ";
        private const string KEY_REASON_CONTROLLER_CHANGED = "KEY WHY CONTROLLER CHANGED: ";

        private InputRouter.ControllerChangedArgs _controllerChangeReason;

        public string Name => "InputRouter";

        public string Description => "Debugs current input router";

        private void OnControllerChanged(object sender, InputRouter.ControllerChangedArgs e)
        {
            _controllerChangeReason = e;
        }

        public void DoDebug()
        {
            void AddDebugInfo(string title, object value = null, string titleColor = null, string valueColor = null)
            {
                titleColor = titleColor ?? DebugColors.COLOR_WHITE;
                valueColor = valueColor ?? DebugColors.COLOR_GREEN;
                _textBlock.AddText(title, titleColor);
                if (value != null)
                {
                    _textBlock.AddText(value.ToString(), valueColor, true);
                }
                _textBlock.AddLine();
            }
            _textBlock.AddText(DEBUG_NAME, DebugColors.COLOR_CYAN, true, true);
            AddDebugInfo(CURRENT_MAP_FIELD, InputRouter.Instance.CurrentButtonMap);
            AddDebugInfo(CURRENT_CONTROLLER, InputRouter.Instance.Controller);
            if (_controllerChangeReason != null)
            {
                AddDebugInfo(BUTTON_REASON_CONTROLLER_CHANGED, _controllerChangeReason.ButtonReason);
                AddDebugInfo(KEY_REASON_CONTROLLER_CHANGED, _controllerChangeReason.KeyReason);
            }
            AddDebugInfo(DPAD_DEADZONE, InputRouter.Instance.DpadDeadZone);
            AddDebugInfo(LEFT_TRIGGER_DEADZONE, InputRouter.Instance.LeftTriggerDeadZone);
            AddDebugInfo(RIGHT_TRIGGER_DEADZONE, InputRouter.Instance.RightTriggerDeadZone);
            AddDebugInfo(RIGHT_STICK_DEADZONE, InputRouter.Instance.RightStickDeadZone);
            AddDebugInfo(LEFT_STICK_DEADZONE, InputRouter.Instance.LeftStickDeadZone);
        }

        public void Open(DebugTextBlock textBlock)
        {
            InputRouter.Instance.ControllerChanged += OnControllerChanged;
            _textBlock = textBlock;
        }

        public void Close()
        {
            InputRouter.Instance.ControllerChanged -= OnControllerChanged;
        }
    }
}
