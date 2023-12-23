using MykroFramework.Runtime.Objects.SOStateMachine;

namespace MykroFramework.Runtime.Objects.Player.Visuals.Skin
{
    public abstract class RotateSkinBase : StateAction
    {
        protected SkinRotator SkinRotator { get; private set; }

        public sealed override void Init(SOStateMachine.SOStateMachine stateMachine)
        {
            SkinRotator = stateMachine.GetComponent<SkinRotator>();
            OnInit(stateMachine);
        }

        protected virtual void OnInit(SOStateMachine.SOStateMachine stateMachine) { }
    }
}