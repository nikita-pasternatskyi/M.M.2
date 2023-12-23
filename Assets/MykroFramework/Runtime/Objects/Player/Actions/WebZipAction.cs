using MykroFramework.Runtime.Objects.Player.Input;
using MykroFramework.Runtime.Objects.SOStateMachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player.Actions
{
    [System.Serializable]
    public class WebZipAction : StateAction
    {
        [SerializeField] private Vector3 _force;
        [ReadOnly] public Vector3 Point;
        [ReadOnly] public Vector3 Direction;
        [ReadOnly] public bool UseRealDirection;

        private PlayerCharacter _spiderMan;
        private IMovementInputProvider _input;
        private WebZipRules _rules;

        public override void Init(SOStateMachine.SOStateMachine stateMachine)
        {
            _spiderMan = stateMachine.GetComponent<PlayerCharacter>();
             _input = (IMovementInputProvider)stateMachine.GetComponent(typeof(IMovementInputProvider));
            _rules = stateMachine.GetComponent<WebZipRules>();
        }

        public override void Enter()
        {
            var direction = UseRealDirection ? Direction : _input.RelativeInput;
            _spiderMan.Jump(Vector3.up * _rules.WebZipMultiplier * _force.y, direction);
            _spiderMan.Velocity += direction * _force.z * _rules.WebZipMultiplier;
            UseRealDirection = false;
        }
    }
}