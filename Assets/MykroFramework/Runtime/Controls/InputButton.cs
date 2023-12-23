using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace MykroFramework.Runtime.Controls
{
    [Serializable]
    public struct InputButton
    {
        public MykroKeyCode KeyboardKey;
        public MykroKeyCode GamepadKey;
        public bool IsFloat;
        [ShowIf("IsFloat")] public bool Snap;
        [ShowIf("IsFloat")] public bool InvertKeyboard;
        [ShowIf("IsFloat")] public bool InvertGamepad;
    }
}