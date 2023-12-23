using MykroFramework.Runtime.Objects.Player2D.Input;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player2D.Actions
{
    public class JumpAction2D : SOStateMachine.StateAction
    {
        [SerializeField] private float jump;

        private PlayerCharacter2D _character;
        private IMovementInputProvider2D _playerInput;

        public override void Init(SOStateMachine.SOStateMachine stateMachine)
        {
            _playerInput = (IMovementInputProvider2D)stateMachine.GetComponent(typeof(IMovementInputProvider2D));
            _character = stateMachine.GetComponent<PlayerCharacter2D>();
        }

        public override void Enter()
        {
            _character.Jump(new Vector2(0, jump));
        }
    }
}
