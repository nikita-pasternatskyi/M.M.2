using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MykroFramework.Runtime.Controls
{
    public abstract class ButtonMap : SerializedScriptableObject
    {
        public bool ConstantlyCheckAllKeys = false;
        public bool LockCursor = false;
        public Dictionary<string, ButtonState> Values;
        public Dictionary<string, InputButton> DefaultButtons;
        public Dictionary<string, InputButton> CurrentButtons;
        public string[] ButtonsArray;

        public (InputButton button, ButtonState value) UI_Cancel;
        public (InputButton button, ButtonState value) UI_Confirm;
        public (InputButton button, ButtonState value) UI_MoveUp;
        public (InputButton button, ButtonState value) UI_MoveDown;
        public (InputButton button, ButtonState value) UI_MoveLeft;
        public (InputButton button, ButtonState value) UI_MoveRight;

        public event Action<Dictionary<string, InputButton>> BindingsChanged;

        private string _filePath;
        private string _userProfileName;

        public float CreateAxis(string positive, string negative)
        {
            return Values[positive].Value - Values[negative].Value;
        }

        public void Reset()
        {
            CurrentButtons = DefaultButtons;
            if (File.Exists(_filePath))
                File.Delete(_filePath);
        }


        private void SaveInput(Dictionary<string, InputButton> profile)
        {
            if (File.Exists(_filePath))
                File.Delete(_filePath);
            byte[] bytes = SerializationUtility.SerializeValue(profile, DataFormat.Binary);
            File.WriteAllBytes(_filePath, bytes);
        }

        public void LoadInput()
        {
            _userProfileName = $"/{name}.inputSettings";
            _filePath = Application.persistentDataPath + _userProfileName;
            if (!File.Exists(_filePath))
            {
                Debug.Log($"No file exists under{_filePath}");
                CurrentButtons = new Dictionary<string, InputButton>(DefaultButtons);
                return;
            }
            byte[] bytes = File.ReadAllBytes(_filePath);
            CurrentButtons = SerializationUtility.DeserializeValue<Dictionary<string, InputButton>>(bytes, DataFormat.Binary);
            BindingsChanged?.Invoke(CurrentButtons);

        }

        public void Remap(string buttonName, MykroKeyCode newKeyCode)
        {
            var btn = CurrentButtons[buttonName];
            if (newKeyCode.IsGamepadKey())
                btn.GamepadKey = newKeyCode;
            else
                btn.KeyboardKey = newKeyCode;
            CurrentButtons[buttonName] = btn;
            SaveInput(CurrentButtons);
            BindingsChanged?.Invoke(CurrentButtons);
        }
    }
}
