using MykroFramework.Runtime.Controls;
using MykroFramework.Runtime.UI;
using UnityEngine;

namespace MM2
{
    public class NavigateWindowsInput : MonoBehaviour
    {
        [SerializeField] private ButtonMap _inputMap;
        [SerializeField] private WindowControl _windowControl;

        private void Update()
        {
            if (_inputMap.UI_MoveUp.value.WasJustPressed)
            {
                _windowControl.Move(0, 1);
            }
            if (_inputMap.UI_MoveDown.value.WasJustPressed)
            {
                _windowControl.Move(0, -1);
            }
            if (_inputMap.UI_MoveRight.value.WasJustPressed)
            {
                _windowControl.Move(1, 0);
            }
            if (_inputMap.UI_MoveLeft.value.WasJustPressed)
            {
                _windowControl.Move(-1, 0);
            }
            if (_inputMap.UI_Confirm.value.WasJustPressed)
            {
                _windowControl.Apply();
            }
            if (_inputMap.UI_Cancel.value.WasJustPressed)
            {
                _windowControl.Cancel();
            }
            if (_inputMap.UI_MoveLeft.value.WasJustPressed)
            {
                _windowControl.Move(-1, 0);
            }
        }
    }
}
