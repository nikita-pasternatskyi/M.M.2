using MykroFramework.Runtime.Extensions;
using MykroFramework.Runtime.Objects.Player.Input;
using MykroFramework.Runtime.Objects.SOStateMachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player.Actions
{
    [System.Serializable]
    public class MoveAction : StateAction
    {
        enum TopSpeedBehaviour
        {
            UseCurrentTopSpeed,
            SetCustomTopSpeed,
            UseCurrentAsTop,
            UseCurrentAsTopAndGraduallyTurnToCustomTop,
        }

        [SerializeField] private TopSpeedBehaviour _topSpeedBehaviour;
        [SerializeField, HideIf("@_topSpeedBehaviour == TopSpeedBehaviour.UseCurrentAsTop || _topSpeedBehaviour == TopSpeedBehaviour.UseCurrentTopSpeed")] private float _topSpeed;
        [SerializeField, HideIf("@_topSpeedBehaviour != TopSpeedBehaviour.UseCurrentAsTopAndGraduallyTurnToCustomTop")] private float _mixTime;
        [SerializeField] private AnimationCurve _smoothingFactorFromDot;
        [SerializeField] private float _smoothing;

        private IMovementInputProvider _input;
        private NextPlayerCharacter _player;

        private float _mixTopSpeed;
        private float _mixTimer;

        public override void Init(SOStateMachine.SOStateMachine stateMachine)
        {
            _input = stateMachine.gameObject.GetComponent<IMovementInputProvider>();
            _player = stateMachine.GetComponent<NextPlayerCharacter>();
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
            var dot = Vector3.Dot(_player.Velocity.XZ().normalized, _input.RelativeInput);
            var smoothing = _smoothingFactorFromDot.Evaluate(dot) * _smoothing;
            _player.Move(_player.TopSpeed, smoothing, _input.RelativeInput);
            if (_topSpeedBehaviour != TopSpeedBehaviour.UseCurrentAsTopAndGraduallyTurnToCustomTop)
                return;
            _mixTimer = Mathf.Clamp(_mixTimer + Time.deltaTime, 0, _mixTime);
            _player.TopSpeed = Mathf.Lerp(_mixTopSpeed, _topSpeed, _mixTimer / _mixTime);
        }
    }
}

