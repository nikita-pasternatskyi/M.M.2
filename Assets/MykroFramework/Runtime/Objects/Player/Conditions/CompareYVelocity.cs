using MykroFramework.Runtime.Objects.Player.Utils;
using MykroFramework.Runtime.Objects.SOStateMachine;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player.Conditions
{
    [System.Serializable]
    public class CompareYVelocity : StateTransitionCondition
    {
        [SerializeField] private FloatComparer _comparer;

        private NextPlayerCharacter _player;

        public override void Init(SOStateMachine.SOStateMachine stateMachine)
        {
            _player = stateMachine.GetComponent<NextPlayerCharacter>();
        }
        public override bool Check()
        {
            return _comparer.Compare(_player.Velocity.y);
        }
    }
}