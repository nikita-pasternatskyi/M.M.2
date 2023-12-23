using MykroFramework.Runtime.Extensions;
using MykroFramework.Runtime.Objects.Player.Input;
using MykroFramework.Runtime.Objects.Player.Web;
using MykroFramework.Runtime.Objects.SOStateMachine;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player.Actions
{
    public interface ISwingPointProvider
    {
        public Vector3 SwingPoint { get; }
        public bool CanSwing { get; }
    }

    [System.Serializable]
    public class SwingAction : StateAction
    {
        [SerializeField] private AnimationCurve _yVelocityRemainderCurve;
        [SerializeField] private AnimationCurve _springForceFromAngle;
        [SerializeField] private float _additionalGravity;
        [SerializeField] private float _swingControl;
        [SerializeField] private float _perfectPointTime;
        [SerializeField] private float _damp;
        [SerializeField] private float _spring;

        private Vector3 _swingPoint;
        private Vector3 _realisticSwingPoint;
        private Vector3 _perfectSwingPoint;

        private IMovementInputProvider _input;
        private NextPlayerCharacter _player;
        public Vector3 SwingPoint;

        private float _distance;
        private float _timer;

        public override void Init(SOStateMachine.SOStateMachine stateMachine)
        {
            _player = stateMachine.GetComponent<NextPlayerCharacter>();
             _input = (IMovementInputProvider)stateMachine.GetComponent(typeof(IMovementInputProvider));
        }

        public override void Enter()
        {
            _player.PhysicsPosition = _player.transform.position;
            _distance = Vector3.Distance(_player.transform.position, SwingPoint);
            _timer = 0.1f;
        }


        public override void Act()
        {
            var connection = _player.PhysicsPosition - SwingPoint;
            var distanceDiscrepancy = _distance - connection.magnitude;

            _player.PhysicsPosition += distanceDiscrepancy * connection.normalized;

            var direction = (_swingPoint - _player.transform.position).normalized;
            var angle = Vector3.Angle(Vector3.up, direction);

            var gravity = Physics.gravity * (_spring * _springForceFromAngle.Evaluate(angle / 90));

            var inputControl = Vector3.ProjectOnPlane(_input.RelativeInput, direction) * _swingControl;
            var velocityTarget = connection + _player.Velocity + gravity + inputControl;
            var projectOnConnection = Vector3.Project(velocityTarget, connection);
           
            _player.Velocity = (velocityTarget - projectOnConnection) / (1 + _damp * Time.deltaTime);
        }

        public override void Exit()
        {
            _distance = 0;
            SwingPoint = Vector3.zero;
            var direction = (_swingPoint - _player.transform.position).normalized;
            var angle = Vector3.Angle(Vector3.up, direction);
            _player.Velocity.y = _yVelocityRemainderCurve.Evaluate(angle / 90) * _player.Velocity.y;
        }
    }
}