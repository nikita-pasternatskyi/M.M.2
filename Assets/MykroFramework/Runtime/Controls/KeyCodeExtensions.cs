namespace MykroFramework.Runtime.Controls
{
    public static class KeyCodeExtensions
    {
        public static bool IsGamepadKey(this MykroKeyCode me)
        {
            return (int)me >= (int)MykroKeyCode.JoystickButton0 && (int)me < (int)MykroKeyCode.MouseMovementXPositive;
        }

        public static bool IsAxisInput(this MykroKeyCode me)
        {
            return (int)me > (int)MykroKeyCode.Joystick8Button19;
        }

        public static bool IsMouseMovement(this MykroKeyCode me)
        {
            return (int)me >= (int)MykroKeyCode.MouseMovementXPositive && (int)me <= (int)MykroKeyCode.MouseMovementYNegative;
        }

        public static bool IsMouse(this MykroKeyCode me)
        {
            switch (me)
            {
                case MykroKeyCode.MouseLeft:
                    return true;
                case MykroKeyCode.MouseRight:
                    return true;
                case MykroKeyCode.MouseMiddle:
                    return true;
                default:
                    return IsMouseMovement(me);
            }
        }
    }
}