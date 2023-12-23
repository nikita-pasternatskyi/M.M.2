using MykroFramework.Runtime.Objects.SOStateMachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.SOStateMachine.Conditions
{
    [System.Serializable]
    public class BufferTrueCondition : StateTransitionCondition
    {
        [SerializeReference] [ListDrawerSettings(AlwaysAddDefaultValue = true, ListElementLabelName = "Name")] private StateTransitionCondition[] _conditions;
        private bool _result;

        public override void Init(SOStateMachine stateMachine)
        {
            foreach (var condition in _conditions)
            {
                condition.Init(stateMachine);
            }
        }

        public override bool Check()
        {
            foreach (var condition in _conditions)
            {
                if (condition.Check() == true && _result == false)
                    _result = true;
            }
            return _result;
        }

        public override void Exit()
        {
            _result = false;
            for (int i = 0; i < _conditions.Length; i++)
            {
                _conditions[i].Exit();
            }
        }

        public override void Enter()
        {
            for (int i = 0; i < _conditions.Length; i++)
            {
                _conditions[i].Enter();
            }
        }
    }
}