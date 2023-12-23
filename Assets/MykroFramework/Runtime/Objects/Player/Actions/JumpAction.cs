using MykroFramework.Runtime.Objects.Player.Input;
using MykroFramework.Runtime.Objects.SOStateMachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player.Actions
{

    [System.Serializable]
    public class JumpAction : StateAction
    {
        enum ValueSource
        {
            Custom,
            FromInput
        }

        enum JumpType
        {
            World,
            InputBased,
        }

        [SerializeField] private ValueSource _valueSource;
        [SerializeField, HideIf("@_valueSource == ValueSource.FromInput")] private Vector3 Jump;
        [SerializeField] private JumpType _jumpType;

        private IJumpHeightValue _jumpHeightValue;
        private NextPlayerCharacter _player;
        private IMovementInputProvider _playerInput;

        public override void Init(SOStateMachine.SOStateMachine stateMachine)
        {
            _playerInput = (IMovementInputProvider)stateMachine.GetComponent(typeof(IMovementInputProvider));
            _player = stateMachine.GetComponent<NextPlayerCharacter>();
            _jumpHeightValue = stateMachine.GetComponent(typeof(IJumpHeightValue)) as IJumpHeightValue;
        }

        public override void Enter()
        {
            var jump = Jump;
            if (_valueSource == ValueSource.FromInput)
                jump = _jumpHeightValue.JumpHeight;
            switch (_jumpType)
            {
                case JumpType.World:
                    _player.Jump(jump, Vector3.forward);
                    break;
                case JumpType.InputBased:
                    _player.Jump(jump, _playerInput.RelativeInput);
                    break;
            }

        }
    }
}