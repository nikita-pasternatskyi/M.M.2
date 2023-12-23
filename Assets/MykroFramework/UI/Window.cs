using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MykroFramework.Runtime.UI
{
    public class Window : MonoBehaviour
    {
        public Window PreviousWindow;
        public LayoutGroup NavigationRoot;
        public UnityEvent Opened;
        public UnityEvent Closed;

        private bool _opened;
        
        public void Open() 
        {
            _opened = true;
            Opened?.Invoke(); 
        }

        public void Close() 
        {
            _opened = false;
            Closed?.Invoke(); 
        }

        public void Trigger() 
        {
            if (_opened)
                Close();
            else
                Open();
        }
    }
}
