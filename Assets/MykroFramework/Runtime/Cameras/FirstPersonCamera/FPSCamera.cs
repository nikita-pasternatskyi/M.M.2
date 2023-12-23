using Sirenix.OdinInspector;
using UnityEngine;

namespace MykroFramework.Runtime.Cameras
{
    public class FPSCamera : SerializedMonoBehaviour
    {
        [SerializeField] private ILookInputProvider _lookInputProvider;
        [SerializeField] private Transform _cameraStand;
        [SerializeField] private Transform _camera;
        [SerializeField] private FloatLimits _verticalAngleLimits;

        Vector2 rotation;
        
        private void Start()
        {
            rotation = Vector2.zero;
        }

        private void Update()
        {
            rotation.x += _lookInputProvider.LookInput.x;
            rotation.y += _lookInputProvider.LookInput.y;
            rotation.y = Mathf.Clamp(rotation.y, _verticalAngleLimits.Min, _verticalAngleLimits.Max);
            var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
            var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

            if (_camera == _cameraStand)
            {
                _camera.localRotation = xQuat * yQuat;
                return;
            }
            _cameraStand.localRotation = xQuat;
            _camera.localRotation = yQuat;

        }
    }
}
