using UnityEngine;
using UnityEngine.Events;

namespace Assets.MykroFramework.Runtime.Controls.Misc
{
    public class InvokeFunction : MonoBehaviour
    {
        public UnityEvent Event;

        public void Invoke() 
        {
            Event?.Invoke();
        }
    }
}
