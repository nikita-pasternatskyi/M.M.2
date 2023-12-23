using System.Collections;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Utils
{
    public class UnparentOnAwake : MonoBehaviour
    {
        private void Awake()
        {
            transform.parent = null;         
        }
    }
}