namespace MykroFramework.Runtime.Objects.SOStateMachine
{
    [System.Serializable]
    public abstract class SimpleTransition
    {
        public virtual void Init(SOStateMachine stateMachine) { }
        public abstract State Check();
    }
}