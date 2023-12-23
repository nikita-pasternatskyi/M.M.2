using System;
using UnityEngine;
using UnityEngine.Events;

namespace MykroFramework.Runtime.UI
{
    public class Selectable : MonoBehaviour
    {
        public UnityEvent Unity_Event_Selected;
        public UnityEvent Unity_Event_Deselected;
        public UnityEvent Unity_Event_Pressed;
        public event Action Selected;
        public event Action Deselected;
        public event Action Pressed;


        private void OnEnable()
        {
            Selected += Unity_Event_Selected.Invoke;
            Deselected += Unity_Event_Deselected.Invoke;
            Pressed += Unity_Event_Pressed.Invoke;
        }
        private void OnDisable()
        {
            Selected -= Unity_Event_Selected.Invoke;
            Deselected -= Unity_Event_Deselected.Invoke;
            Pressed -= Unity_Event_Pressed.Invoke;
        }

        public void Select()
        {
            Selected?.Invoke();
        }
        
        public void Deselect()
        {
            Deselected?.Invoke();
        }

        public void Press()
        {
            Pressed?.Invoke();
        }

    }
}
