using MykroFramework.Runtime.Objects.Player2D.Input;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player2D.Actions
{

    public class Move2D : SOStateMachine.StateAction
    {
        enum TopSpeedBehaviour
        {
            UseCurrentTopSpeed,
            SetCustomTopSpeed,
            UseCurrentAsTop,
            UseCurrentAsTopAndGraduallyTurnToCustomTop,
        }

        [SerializeField] private bool _stickToFloor = true;
        [SerializeField] private TopSpeedBehaviour _topSpeedBehaviour;
        [SerializeField, HideIf("@_topSpeedBehaviour == TopSpeedBehaviour.UseCurrentAsTop || _topSpeedBehaviour == TopSpeedBehaviour.UseCurrentTopSpeed")] private float _topSpeed;
        [SerializeField, HideIf("@_topSpeedBehaviour != TopSpeedBehaviour.UseCurrentAsTopAndGraduallyTurnToCustomTop")] private float _mixTime;
        [SerializeField] private AnimationCurve _accelerationSmoothnessFactorFromDot;
        [SerializeField] private float _accelerationSmoothness;

        private IMovementInputProvider2D _input;
        private PlayerCharacter2D _player;

        private float _mixTopSpeed;
        private float _mixTimer;

        public override void Init(SOStateMachine.SOStateMachine stateMachine)
        {
            _input = (IMovementInputProvider2D)stateMachine.GetComponent(typeof(IMovementInputProvider2D));
            _player = stateMachine.GetComponent<PlayerCharacter2D>();
        }

        public override void Enter()
        {
            switch (_topSpeedBehaviour)
            {
                case TopSpeedBehaviour.SetCustomTopSpeed:
                    _player.TopSpeed = _topSpeed;
                    break;
                case TopSpeedBehaviour.UseCurrentAsTop:
                    _player.TopSpeed = _player.Velocity.magnitude;
                    break;
                case TopSpeedBehaviour.UseCurrentAsTopAndGraduallyTurnToCustomTop:
                    _mixTopSpeed = _player.TopSpeed = _player.Velocity.magnitude;
                    break;
            }
        }

        public override void Exit()
        {
            _mixTimer = 0;
        }

        public override void PhysicsAct()
        {
            var input = _input.AbsoluteInput;
            input.y = 0;
            var dot = Vector2.Dot(_player.Velocity.normalized, _input.AbsoluteInput);
            var acceleration = _accelerationSmoothnessFactorFromDot.Evaluate(dot) * _accelerationSmoothness;
            _player.Move(_player.TopSpeed, acceleration, _input.AbsoluteInput, _stickToFloor);

            if (_topSpeedBehaviour != TopSpeedBehaviour.UseCurrentAsTopAndGraduallyTurnToCustomTop)
                return;
            _mixTimer = Mathf.Clamp(_mixTimer + Time.deltaTime, 0, _mixTime);
            _player.TopSpeed = Mathf.Lerp(_mixTopSpeed, _topSpeed, _mixTimer / _mixTime);
        }
    }
}
