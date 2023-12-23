using MykroFramework.Runtime.Objects.SOStateMachine;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.SOAnimationMachine
{
    [System.Serializable]
    public class AnimationPreviousStateCondition : StateTransitionCondition
    {
        [SerializeField] private StateContainer _referenceState;
        private SOAnimator _soAnimator;

        public override void Init(SOStateMachine.SOStateMachine stateMachine)
        {
            _soAnimator = stateMachine.GetComponent<SOAnimator>();
        }

        public override bool Check()
        {
            if (_soAnimator.PreviousAnimationState == _referenceState)
                return true;
            return false;
        }
    }
}
