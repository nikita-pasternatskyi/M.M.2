using MykroFramework.Runtime.Objects.Player.Utils;
using MykroFramework.Runtime.Objects.SOStateMachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player.Conditions
{

    [System.Serializable]
    public class GroundedCondition : StateTransitionCondition
    {
        [SerializeField] private GroundCheckType _groundCheckType;
        [SerializeField, HideIf("@_groundCheckType != GroundCheckType.Distance")] private FloatComparer _distanceComparer;
        [SerializeField, HideIf("@_groundCheckType != GroundCheckType.Distance")] private float _groundCheckOffset;
        private GroundedState _groundedState;

        public override void Init(SOStateMachine.SOStateMachine stateMachine)
        {
            _groundedState = stateMachine.GetComponent<GroundedState>();
        }

        public override bool Check()
        {
            switch (_groundCheckType)
            {
                case GroundCheckType.Distance:
                    return _distanceComparer.Compare(_groundedState.GetDistanceToGround(_groundCheckOffset));
                case GroundCheckType.Grounded:
                    return _groundedState.Grounded;
                case GroundCheckType.InAir:
                    return !_groundedState.Grounded;
            }
            return false;
        }
    }
}