using MykroFramework.Runtime.Objects.Player.Conditions;
using MykroFramework.Runtime.Objects.Player.Utils;
using MykroFramework.Runtime.Objects.Player2D.Utils;
using MykroFramework.Runtime.Objects.SOStateMachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player2D.Conditions
{
    class Grounded2DCondition : StateTransitionCondition
    {
        [SerializeField] private GroundCheckType _groundCheckType;
        [SerializeField, HideIf("@_groundCheckType != GroundCheckType.Distance")] private FloatComparer _distanceComparer;
        private GroundedState2D _groundedState2D;


        public override void Init(SOStateMachine.SOStateMachine stateMachine)
        {
            _groundedState2D = stateMachine.GetComponent<GroundedState2D>();
        }

        public override bool Check()
        {
            switch (_groundCheckType)
            {
                case GroundCheckType.Distance:
                    return _distanceComparer.Compare(_groundedState2D.DistanceToGround);
                case GroundCheckType.Grounded:
                    return _groundedState2D.Grounded;
                case GroundCheckType.InAir:
                    return !_groundedState2D.Grounded;
            }
            return false;
        }
    }
}
