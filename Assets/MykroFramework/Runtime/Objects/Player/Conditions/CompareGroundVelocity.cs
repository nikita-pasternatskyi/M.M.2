using MykroFramework.Runtime.Objects.Player.Utils;
using MykroFramework.Runtime.Objects.SOStateMachine;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player.Conditions
{
    [System.Serializable]
    public class CompareGroundVelocity : StateTransitionCondition
    {
        [SerializeField] private Vector3MagnitudeComparer _comparer;

        private PlayerCharacter _spiderMan;

        public override void Init(SOStateMachine.SOStateMachine stateMachine)
        {
            _spiderMan = stateMachine.GetComponent<PlayerCharacter>();
        }
        public override bool Check()
        {
            return _comparer.Compare(_spiderMan.Velocity);
        }
    }
}