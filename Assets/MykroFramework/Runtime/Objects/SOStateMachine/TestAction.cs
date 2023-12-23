using UnityEngine;

namespace MykroFramework.Runtime.Objects.SOStateMachine
{
    [System.Serializable]
    public class TestAction : StateAction
    {
        [SerializeField] private string _message;

        public override void Enter()
        {
            UnityEngine.Debug.Log(_message);
        }
    }
}