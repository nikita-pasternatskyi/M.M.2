using UnityEngine;

namespace MykroFramework.Runtime.Utils
{
    class SetDontDestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}
