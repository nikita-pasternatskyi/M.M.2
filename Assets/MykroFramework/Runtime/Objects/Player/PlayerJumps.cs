using MykroFramework.Runtime.Objects.Player.Input;
using MykroFramework.Runtime.Objects.Player.Web;
using MykroFramework.Runtime.Objects.SOStateMachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player
{
    public class PlayerJumps : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private IMovementInputProvider _playerInput;
        [SerializeField, ValueDropdown("@Buttons.BUTTONS_ARRAY")] private string _chargeButton;

        [Header("States")]
        [SerializeField] private SOStateMachine.SOStateMachine _stateMachine;
        [SerializeField] private StateContainer _swingState;
        [SerializeField] private StateContainer _sprintState;

        [Header("Charge Jump")]
        [SerializeField] private float _chargeJumpTime;
        [SerializeField] private Vector2 _minMaxChargeJumpHeight;
        [SerializeField] private Vector2 _minMaxChargeJumpForward;
        [SerializeField] private AnimationCurve _chargeJumpCurve;
        [SerializeField] private AnimationCurve _chargeJumpForwardCurve;

        [Header("Swing Jump")]
        [SerializeField] private Transform _body;
        [SerializeField] private SwingPointFinder _webShooter;
        [SerializeField] private Vector2 _minMaxSwingJumpHeight;
        [SerializeField] private AnimationCurve _swingHeightPowerCurve;

        [ReadOnly] public Vector3 JumpForce;
        private float _chargeJumpTimer;

        enum State
        {
            Swinging, Sprinting, None
        }

        private State _state;

        private void OnEnable()
        {
            _stateMachine.NewStateEntered += OnEntered;
            _stateMachine.OldStateExited += OnExited;
        }

        private void OnDisable()
        {
            _stateMachine.NewStateEntered -= OnEntered;
            _stateMachine.OldStateExited -= OnExited;
        }

        private void Update()
        {
            switch (_state)
            {
                case State.Swinging:
                    //if (_playerInput.Buttons.Jumped.Pressed)
                    //{
                    //    _chargeJumpTimer = Mathf.Clamp(_chargeJumpTimer + Time.deltaTime, 0, _chargeJumpTime);
                    //    JumpForce.y = Mathf.Lerp(_minMaxSwingJumpHeight.x, _minMaxSwingJumpHeight.y, _chargeJumpCurve.Evaluate(_chargeJumpTimer / _chargeJumpTime));
                    //    JumpForce.z = 0;
                    //}
                    break;
                case State.Sprinting:
                    //if (_playerInput.Buttons.Jumped.Pressed)
                    //{
                    //    _chargeJumpTimer = Mathf.Clamp(_chargeJumpTimer + Time.deltaTime, 0, _chargeJumpTime);
                    //    JumpForce.y = Mathf.Lerp(_minMaxChargeJumpHeight.x, _minMaxChargeJumpHeight.y, _chargeJumpCurve.Evaluate(_chargeJumpTimer / _chargeJumpTime));
                    //    JumpForce.z = Mathf.Lerp(_minMaxChargeJumpForward.x, _minMaxChargeJumpForward.y, _chargeJumpForwardCurve.Evaluate(_chargeJumpTimer / _chargeJumpTime));
                    //}
                    break;
                case State.None:
                    _chargeJumpTimer = 0;
                    return;
            }
        }

        private void OnExited(StateContainer obj)
        {
            _state = State.None;
        }

        private void OnEntered(StateContainer obj)
        {
            if (obj == _swingState)
                _state = State.Swinging;
            else if (obj == _sprintState)
                _state = State.Sprinting;
        }

    }
}