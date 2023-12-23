using MykroFramework.Runtime.Extensions;
using MykroFramework.Runtime.Objects.Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MM2
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(CharacterController))]
    public class FPSPlayer : MonoBehaviour
    {
        [Header("Set-up")]
        [SerializeField, Required] private GroundedState _groundedState;
        [SerializeField, Tooltip("It has to be LESS THAN 0")] private float _gravity;

        [Header("Movement")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _groundSpeedSmoothing;

        [Header("Chase sequence")]
        [SerializeField] private FPSBobbing[] _bobbings;
        [SerializeField] private float _bobbingSpeedMultiplier;
        [SerializeField] private AudioSource _footStepsSource;
        [SerializeField] private AudioClip _runningFootStepsAudio;
        [SerializeField] private AudioSource _heavyBreathing;
        [SerializeField] private float _runSpeed;
        private Vector3 _moveDampVelocity;

        private PlayerInput _playerInput;
        private CharacterController _characterController;
        private Vector3 _gravityVelocity;
        private Vector3 _velocity;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _characterController = GetComponent<CharacterController>();
        }

        public void BeginChase()
        {
            foreach (var item in _bobbings)
            {
                item.BobbingSpeed *= _bobbingSpeedMultiplier;
            }
            _footStepsSource.clip = _runningFootStepsAudio;
            _footStepsSource.Stop();
            _heavyBreathing.Play();
            _moveSpeed = _runSpeed;
        }

        private void Update()
        {
            var smoothing = _groundSpeedSmoothing;
            var groundVelocity = _velocity.XZ();
            groundVelocity = Vector3.SmoothDamp(
                groundVelocity,
                _playerInput.RelativeMovementInput * _moveSpeed,
                ref _moveDampVelocity,
                smoothing
                );
            _velocity.x = groundVelocity.x;
            _velocity.z = groundVelocity.z;
        }

        private void FixedUpdate()
        {
            void AddGravity()
            {
                if (!_groundedState.Grounded)
                {
                    _velocity.y += _gravity * Time.fixedDeltaTime * Time.fixedDeltaTime;
                }
                else if (_groundedState.Grounded && _gravityVelocity.y < 0)
                {
                    _velocity.y = 0;
                }
            }
            AddGravity();
            _characterController.Move(_velocity);
        }
    }
}
