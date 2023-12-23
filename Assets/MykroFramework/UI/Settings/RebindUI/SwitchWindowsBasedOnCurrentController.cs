using System.Collections.Generic;
using UnityEngine;
using MykroFramework.Runtime.UI;
using Sirenix.OdinInspector;
using MEC;

namespace MykroFramework.Runtime.Controls.RebindUI
{
    public class SwitchWindowsBasedOnCurrentController : SerializedMonoBehaviour
    {
        [SerializeField] private Dictionary<Controller, Window> _windows;
        [SerializeField] private bool _hideAllWindowsOnAwake;
        [SerializeField] private float _checkFrequency;
        private Window _currentWindow;

        private CoroutineHandle _routine;
        private WaitForSeconds _delay;

        private void Awake()
        {
            _delay = new WaitForSeconds(1 / _checkFrequency);
            if (_hideAllWindowsOnAwake)
            {
                foreach (var item in _windows.Values)
                {
                    item.Close();
                }
            }
        }

        private void OnEnable()
        {
            _routine = Timing.RunCoroutine(CheckController());
        }

        private void OnDisable()
        {
            if (_routine.IsRunning)
                Timing.KillCoroutines(_routine);
        }

        private IEnumerator<float> CheckController()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(1f/ _checkFrequency);
                
                if (_windows.TryGetValue(InputRouter.Instance.Controller, out Window window))
                {
                    ChangeWindow(window);
                }
            }
        }

        private void ChangeWindow(Window window)
        {
            _currentWindow?.Close();
            _currentWindow = window;
            _currentWindow.Open();
        }
    }
}
