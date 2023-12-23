using MykroFramework.Runtime.Cameras.Camera2D;
using MykroFramework.Runtime.Objects.Player.Input;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MM2
{
    public class FPSBobbing : SerializedMonoBehaviour
    {
        [SerializeField] private IMovementInputProvider _playerInput;
        [SerializeField] private float _bobbingHeight;
        [SerializeField] public float BobbingSpeed;
        [SerializeField] private Vector2Bool _axisToBob;
        [SerializeField] private float _lerpSpeed;
        [SerializeField] private Transform _holder;
        private Vector3 _startLocalPosition;

        private void Start()
        {
            _startLocalPosition = transform.localPosition;
        }

        private Vector3 BobbingMotion()
        {
            Vector3 pos = Vector3.zero;
            pos.y = Mathf.Sin(Time.time * BobbingSpeed) * _bobbingHeight;
            pos.x = Mathf.Cos(Time.time * BobbingSpeed / 2) * _bobbingHeight * 2;
            if (!_axisToBob.X)
                pos.x = 0;
            if (!_axisToBob.Y)
                pos.y = 0;
            return pos;
        }

        private void ResetPosition()
        {
            if (transform.localPosition != _startLocalPosition)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, _startLocalPosition, Time.deltaTime);
            }
        }

        private void Update()
        {
            ResetPosition();
            if (_playerInput.AbsoluteInput == Vector2.zero)
                return;
            transform.localPosition += BobbingMotion();
        }
    }
}
