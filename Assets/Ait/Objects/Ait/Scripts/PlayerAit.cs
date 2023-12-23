using MEC;
using MykroFramework.Runtime.Extensions;
using MykroFramework.Runtime.Objects.Player;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ait
{
    public enum AitLocomotionState
    {
        Normal, GrappleSwing, GrappleZip, Slide, WallRun
    }

    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerAit : MonoBehaviour
    {
        [Header("Set-up")]
        [SerializeField, Required] private GroundedState _groundedState;
        [SerializeField, Tooltip("It has to be LESS THAN 0")] private float _gravity;

        [Header("Movement")]
        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _groundSpeedSmoothing;
        [SerializeField] private float _airSpeedSmoothing;
        private Vector3 _moveDampVelocity;

        [Header("Dash")]
        [SerializeField] private float _dashPower;
        [SerializeField] private float _dashCooldown;
        private float _dashCooldownTimer;

        [Header("Slide")]
        [SerializeField] private float _lowSpeedThreshold;
        [SerializeField] private float _slideHeight;
        [SerializeField] private float _slideSmoothing;
        private Vector3 _slideDampVelocity;
        private float _startHeight;

        [Header("Grappling Hook")]
        [SerializeField] private float _hookBreakTreshold;
        [SerializeField] private float _hookJumpTreshold;
        [SerializeField] private float _hookSpeed;
        [SerializeField] private float _hookJumpHeight;
        [SerializeField] private float _hookSpeedSmoothing;
        public event Action<Vector3> BeganZip;
        public event Action StoppedZip;
        private Vector3 _hookDampVelocity;
        private CoroutineHandle _jumpRoutineHandle;

        [Header("Swing")]
        [SerializeField] private float _swingDamp;
        [SerializeField] private float _swingSpeed;
        private Vector3 _anchorPoint;
        public event Action<Vector3> BeganSwing;
        public event Action StoppedSwing;

        [Header("WallRun")]
        [SerializeField] private LayerMask _whatIsWall;
        [SerializeField] private float _wallCheckDistance;
        [SerializeField] private float _minimumDistanceFromGroundForWallRun;
        [SerializeField] private float _wallRunGravity;
        [SerializeField] private float _wallJumpHeightForce;
        [SerializeField] private float _wallJumpNormalForce;
        [SerializeField] private float _wallJumpForwardForce;
        private Vector3 _wallRunInputDirection;
        private Vector3 _wallRunNormal;

        private PlayerInput _playerInput;
        private CharacterController _characterController;
        private Vector3 _gravityVelocity;
        private Vector3 _velocity;
        private AitLocomotionState _currentLocomotionState;

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(transform.position, transform.forward * _wallCheckDistance);
            Gizmos.DrawRay(transform.position, -transform.up * _minimumDistanceFromGroundForWallRun);
        }

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _characterController = GetComponent<CharacterController>();
            _startHeight = _characterController.height;
        }

        private void Update()
        {
            switch (_currentLocomotionState)
            {
                case AitLocomotionState.Normal:
                    if (CanWallRun(_playerInput.RelativeMovementInput, out RaycastHit wallHit))
                        StartWallRun(_playerInput.RelativeMovementInput, wallHit);

                    _dashCooldownTimer = Mathf.Clamp(_dashCooldownTimer - Time.deltaTime, 0, _dashCooldownTimer);
                    var smoothing = _groundedState.Grounded ? _groundSpeedSmoothing : _airSpeedSmoothing;
                    var groundVelocity = _velocity.XZ();
                    groundVelocity = Vector3.SmoothDamp(
                        groundVelocity,
                        _playerInput.RelativeMovementInput * _moveSpeed,
                        ref _moveDampVelocity,
                        smoothing
                        );
                    _velocity.x = groundVelocity.x;
                    _velocity.z = groundVelocity.z;

                    if (groundVelocity.magnitude > _lowSpeedThreshold)
                    {
                        if (_playerInput.GameplayMap.GetDashState().WasJustPressed && _dashCooldownTimer == 0)
                        {
                            _velocity += _playerInput.RelativeMovementInput * _dashPower;
                            _dashCooldownTimer = _dashCooldown;
                        }

                        if (_playerInput.GameplayMap.GetSlideState().WasJustPressed)
                            _currentLocomotionState = AitLocomotionState.Slide;
                    }

                    if (_playerInput.GameplayMap.GetJumpState().WasJustPressed && _groundedState.Grounded)
                    {
                        _velocity.y = _jumpHeight;
                    }
                    break;

                case AitLocomotionState.GrappleSwing:
                    _velocity += _playerInput.RelativeMovementInput * _swingSpeed * Time.deltaTime;
                    break;

                case AitLocomotionState.GrappleZip:
                    if (_playerInput.GameplayMap.GetJumpState().WasJustPressed)
                    {
                        var zipLength = transform.position.GetDistance(_anchorPoint);
                        if (zipLength < _hookJumpTreshold && !_jumpRoutineHandle.IsRunning)
                        {
                            _jumpRoutineHandle = Timing.RunCoroutine(JumpFromZipping());
                        }
                    }
                    break;

                case AitLocomotionState.Slide:
                    void ExitSlide()
                    {
                        _currentLocomotionState = AitLocomotionState.Normal;
                        _characterController.height = _startHeight;
                    }
                    _characterController.Move(new Vector3(0, -(_startHeight - _slideHeight), 0));
                    _characterController.height = _slideHeight;
                    var slideVelocity = _velocity.XZ();
                    slideVelocity = Vector3.SmoothDamp(
                        slideVelocity,
                        Vector3.zero,
                        ref _slideDampVelocity,
                        _slideSmoothing
                        );
                    _velocity.x = slideVelocity.x;
                    _velocity.z = slideVelocity.z;

                    if (_playerInput.GameplayMap.GetJumpState().WasJustPressed)
                    {
                        _velocity.y = _jumpHeight;
                        ExitSlide();
                    }
                    else if (!_groundedState.Grounded || _playerInput.GameplayMap.GetSlideState().WasJustReleased)
                    {
                        ExitSlide();
                    }
                    break;

                case AitLocomotionState.WallRun:
                    if (_playerInput.GameplayMap.GetJumpState().WasJustPressed)
                    {
                        _velocity.y = 0;
                        _currentLocomotionState = AitLocomotionState.Normal;
                        _velocity = JumpFromWallForce();
                    }
                    break;
            }
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
            switch (_currentLocomotionState)
            {
                default:
                    AddGravity();
                    break;
                case AitLocomotionState.WallRun:
                    AddGravity();
                    if (Physics.Raycast(transform.position, _wallRunInputDirection, 1f, _whatIsWall))
                    {
                        Vector3 wallRunVelocity = Vector3.zero;
                        if (_velocity.y < 0)
                            wallRunVelocity.y = _wallRunGravity * Time.fixedDeltaTime;
                        _velocity += wallRunVelocity;
                    }
                    else
                    {
                        _currentLocomotionState = AitLocomotionState.Normal;
                    }
                    break;

                case AitLocomotionState.GrappleSwing:
                    AddGravity();
                    var direction = transform.position.GetDirection(_anchorPoint);
                    _velocity = Vector3.ProjectOnPlane(_velocity, direction);
                    _velocity += Vector3.Project(Vector3.up, direction) * -_gravity * Time.fixedDeltaTime * Time.fixedDeltaTime;
                    _velocity = Vector3Extensions.FramelessLerp(_velocity, Vector3.zero, _swingDamp, Time.deltaTime);
                    break;
                case AitLocomotionState.GrappleZip:
                    var zipLength = transform.position.GetDistance(_anchorPoint);
                    var zipDirection = transform.position.GetDirection(_anchorPoint);
                    _velocity = Vector3.SmoothDamp(
                        _velocity,
                        zipDirection * _hookSpeed,
                        ref _hookDampVelocity,
                        _hookSpeedSmoothing
                        );
                    if (zipLength < _hookBreakTreshold)
                        StopZip();
                    break;
            }
            _characterController.Move(_velocity);
        }

        private IEnumerator<float> JumpFromZipping()
        {
            StopZip();
            var velocity = _velocity;
            _velocity = Vector3.zero;
            yield return Timing.WaitForSeconds(0.1f);
            _velocity = velocity;
            _velocity.y = _hookJumpHeight;

        }

        public void AttachToAnchorPoint(Vector3 point)
        {
            BeganSwing?.Invoke(point);
            _currentLocomotionState = AitLocomotionState.GrappleSwing;
            _anchorPoint = point;
        }

        public void DetachFromAnchorPoint()
        {
            if (_currentLocomotionState == AitLocomotionState.GrappleSwing)
            {
                StoppedSwing?.Invoke();
                _currentLocomotionState = AitLocomotionState.Normal;
            }
        }

        public void ZipToPoint(Vector3 point)
        {
            BeganZip?.Invoke(point);
            _currentLocomotionState = AitLocomotionState.GrappleZip;
            _anchorPoint = point;
        }

        public void StopZip()
        {
            if (_currentLocomotionState == AitLocomotionState.GrappleZip)
            {
                _currentLocomotionState = AitLocomotionState.Normal;
                StoppedZip?.Invoke();
            }
        }

        private bool CanWallRun(in Vector3 checkDirection, out RaycastHit WallHit)
        {
            bool hitWall = Physics.Raycast(transform.position, checkDirection, out WallHit, _wallCheckDistance, _whatIsWall);
            if (HighEnoughAboveTheGround() && hitWall && _currentLocomotionState != AitLocomotionState.WallRun)
            {
                if (CheckSurfaceAngle(WallHit.normal, 90))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        private void StartWallRun(in Vector3 movementInputDirection, in RaycastHit wallHit)
        {
            _wallRunInputDirection = movementInputDirection;
            _wallRunNormal = wallHit.normal;
            _currentLocomotionState = AitLocomotionState.WallRun;

            if (_velocity.y < 0)
                _velocity.y = 0;
        }

        private bool HighEnoughAboveTheGround()
        {
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit groundHit, Mathf.Infinity, _whatIsWall))
            {
                if (groundHit.distance >= _minimumDistanceFromGroundForWallRun)
                    return true;
            }
            else if (!Physics.Raycast(transform.position, -transform.up, _minimumDistanceFromGroundForWallRun, _whatIsWall))
            {
                return true;
            }
            return false;
        }

        private bool CheckSurfaceAngle(Vector3 surface, float targetAngle) => Mathf.Abs(targetAngle - Vector3.Angle(Vector3.up, surface)) < 0.1f;

        private Vector3 JumpFromWallForce()
        {
            var jumpDirection = transform.forward;
            jumpDirection.y = 0;
            Vector3 forceModificators = (Vector3.up * _wallJumpHeightForce + jumpDirection * _wallJumpForwardForce);
            return _wallRunNormal * _wallJumpNormalForce + forceModificators;
        }
    }
}
