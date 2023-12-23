using UnityEngine;

namespace MykroFramework.Runtime.Objects.Utils
{
    public class ResetRotation : MonoBehaviour
    {
        private void Update()
        {
            transform.rotation = Quaternion.identity;
        }
    }
}