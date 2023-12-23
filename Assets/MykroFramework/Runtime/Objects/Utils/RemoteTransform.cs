using Sirenix.OdinInspector;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Utils
{

    public class RemoteTransform : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        [SerializeField] private bool _createOffsetOnAwake = false;
        [SerializeField] private bool _copyPosition = true;
        [SerializeField] private bool _copyRotation;
        [SerializeField] private bool _copyScale;

        [SerializeField] [Range(0, 1)] private float _smoothness = 0;

        [ReadOnly] public Vector3 Offset = Vector3.zero;
        private void Awake()
        {
            if (_createOffsetOnAwake)
                Offset = transform.position - _target.position;
        }

        private void Update()
        {
            if (_copyPosition)
                transform.position = Vector3.Lerp(_target.position + Offset, transform.position, _smoothness * Time.deltaTime);
            if (_copyRotation)
                transform.rotation = Quaternion.Slerp(_target.rotation, transform.rotation, _smoothness * Time.deltaTime);
            if (_copyScale)
                transform.localScale = Vector3.Lerp(_target.localScale, transform.localScale, _smoothness * Time.deltaTime);
        }
    }
}