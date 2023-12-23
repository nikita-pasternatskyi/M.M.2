using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace MykroFramework.Runtime.Objects.SOStateMachine
{
    [System.Serializable]
    public class StateAction
    {
        public string Name => GetType().Name;
        public virtual void Init(SOStateMachine stateMachine) { }
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Act() { }
        public virtual void PhysicsAct() { }
    }
}