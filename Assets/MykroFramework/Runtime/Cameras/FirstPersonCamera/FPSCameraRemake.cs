using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace MykroFramework.Runtime.Cameras
{
    public class FPSCameraRemake : SerializedMonoBehaviour
    {
            [SerializeField] private ILookInputProvider _lookInput;

            [SerializeField] private Transform _camera;
            [SerializeField] private Transform[] _bodiesToRotateY;
            [SerializeField] private Transform[] _bodiesToRotateX;

            [SerializeField] private float _sensitivityX = 15F;
            [SerializeField] private float _sensitivityY = 15F;

            [SerializeField] private float _minimumX = -360F;
            [SerializeField] private float _maximumX = 360F;

            [SerializeField] private float _minimumY = -60F;
            [SerializeField] private float _maximumY = 60F;

            [SerializeField] private float _frameCounter = 20;

            private float _rotationX = 0F;
            private float _rotationY = 0F;

            private List<float> _rotationArrayX = new List<float>();
            private float _averageRotationX = 0F;

            private List<float> _rotationArrayY = new List<float>();
            private float _averageRotationY = 0F;

            private Quaternion _originalRotation;

            private void Update()
            {
                Quaternion yQuaternion, xQuaternion;
                CalculateRotations(out yQuaternion, out xQuaternion);
                ApplyRotations(yQuaternion, xQuaternion);
            }

            private void ApplyRotations(Quaternion yQuaternion, Quaternion xQuaternion)
            {
                foreach (var item in _bodiesToRotateY)
                {
                    item.localRotation = _originalRotation * yQuaternion;
                }
                foreach (var item in _bodiesToRotateX)
                {
                    item.localRotation = _originalRotation * xQuaternion;
                }
            }

            private void CalculateRotations(out Quaternion yQuaternion, out Quaternion xQuaternion)
            {
                _averageRotationY = 0f;
                _averageRotationX = 0f;

                _rotationY += Input.GetAxisRaw("Mouse Y") * _sensitivityY;
                _rotationX += Input.GetAxisRaw("Mouse X") * _sensitivityX;

                _rotationArrayY.Add(_rotationY);
                _rotationArrayX.Add(_rotationX);

                if (_rotationArrayY.Count >= _frameCounter)
                {
                    _rotationArrayY.RemoveAt(0);
                }
                if (_rotationArrayX.Count >= _frameCounter)
                {
                    _rotationArrayX.RemoveAt(0);
                }

                for (int j = 0; j < _rotationArrayY.Count; j++)
                {
                    _averageRotationY += _rotationArrayY[j];
                }
                for (int i = 0; i < _rotationArrayX.Count; i++)
                {
                    _averageRotationX += _rotationArrayX[i];
                }

                _averageRotationY /= _rotationArrayY.Count;
                _averageRotationX /= _rotationArrayX.Count;

                _averageRotationY = ClampAngle(_averageRotationY, _minimumY, _maximumY);
                _averageRotationX = ClampAngle(_averageRotationX, _minimumX, _maximumX);

                yQuaternion = Quaternion.AngleAxis(_averageRotationY, Vector3.left);
                xQuaternion = Quaternion.AngleAxis(_averageRotationX, Vector3.up);
            }

            private void Start()
            {
                _originalRotation = transform.localRotation;
            }

            private static float ClampAngle(float angle, float min, float max)
            {
                angle = angle % 360;
                if ((angle >= -360F) && (angle <= 360F))
                {
                    if (angle < -360F)
                    {
                        angle += 360F;
                    }
                    if (angle > 360F)
                    {
                        angle -= 360F;
                    }
                }
                return Mathf.Clamp(angle, min, max);
            }

    }
}
