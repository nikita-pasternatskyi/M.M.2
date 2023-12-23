using MykroFramework.Runtime.Objects.SOStateMachine;

namespace MykroFramework.Runtime.Objects.Player.Camera.StateActions
{
    public abstract class CameraAction : StateAction
    {
        protected PlayerCamera Camera { get; private set; }

        public sealed override void Init(SOStateMachine.SOStateMachine stateMachine)
        {
            Camera = stateMachine.GetComponent<PlayerCamera>();
            OnInit(stateMachine);
        }

        protected virtual void OnInit(SOStateMachine.SOStateMachine stateMachine) { }
    }
}
