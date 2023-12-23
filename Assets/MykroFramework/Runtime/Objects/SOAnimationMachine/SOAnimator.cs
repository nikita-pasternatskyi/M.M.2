using Animancer;
using MykroFramework.Runtime.Objects.SOStateMachine;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.SOAnimationMachine
{
    public class SOAnimator : MonoBehaviour
    {
        [SerializeField] private SOStateMachine.SOStateMachine _stateMachineToGrabReferenceFrom;
        public StateContainer PreviousAnimationState { get; private set; }
        public AnimationClip PreviousAnimationClip { get; private set; }
        public AnimancerComponent Animancer;

        private void OnEnable() => _stateMachineToGrabReferenceFrom.OldStateExited += OnOldStateExited;

        private void OnDisable() => _stateMachineToGrabReferenceFrom.OldStateExited -= OnOldStateExited;
        private void OnOldStateExited(StateContainer obj) => PreviousAnimationState = obj;

        private void Update()
        {
            if (Animancer.States.Current != null)
            {
                PreviousAnimationClip = Animancer.States.Current.Clip;
            }
        }

        public AnimancerState PlayClip(ITransition clip)
        {
            var c = Animancer.Play(clip);
            PreviousAnimationClip = c.Clip;
            return c;
        }

    }
}
