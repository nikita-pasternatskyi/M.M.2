namespace MykroFramework.Runtime.Objects.SOStateMachine
{
    [System.Serializable]
    public abstract class StateTransitionCondition
    {
        public bool Mandatory;
        public bool ReverseAnswer;

        public string Name => $"{GetType().Name} {(Mandatory? "- Required" : "")}";
        public virtual void Init(SOStateMachine stateMachine) { }

        public virtual void Enter() { }
        public virtual void Exit() { }
        public abstract bool Check();
    }
}