using UnityEngine;
using System;
using System.Linq;
using Sirenix.OdinInspector;

namespace MykroFramework.Runtime.Objects.SOStateMachine
{
    [Serializable]
    public sealed class StateTransition
    {
        [SerializeField] private StateContainer _toState;
        [SerializeReference] [ListDrawerSettings(AlwaysAddDefaultValue = true, ListElementLabelName = "Name")] private StateTransitionCondition[] _conditions;
        public StateContainer ToState => _toState;

        private StateTransitionCondition[] _mandatoryConditions;
        private StateTransitionCondition[] _optionalConditions;

        public string Name => _toState == null ? "Empty Transition" : $"To {_toState.name}";

        public void Init(SOStateMachine soStateMachine)
        {
            foreach (var condition in _conditions)
            {
                condition.Init(soStateMachine);
            }
            _mandatoryConditions = _conditions.Where((c) => c.Mandatory == true).ToArray();
            _optionalConditions = _conditions.Where((c) => c.Mandatory == false).ToArray();
        }

        public void Enter()
        {
            for (int i = 0; i < _conditions.Length; i++)
            {
                _conditions[i].Enter();
            }
        }
        
        public void Exit() 
        {
            for (int i = 0; i < _conditions.Length; i++)
            {
                _conditions[i].Exit();
            }
        }

        public StateContainer CheckTransition()
        {
            for (int i = 0; i < _mandatoryConditions.Length; i++)
            {
                if (_mandatoryConditions[i].Check() == _mandatoryConditions[i].ReverseAnswer)
                    return null;
            }

            if (_optionalConditions.Length == 0)
                return _toState;

            for (int i = 0; i < _optionalConditions.Length; i++)
            {
                if (_optionalConditions[i].Check() == !_optionalConditions[i].ReverseAnswer)
                    return _toState;
            } 

            return null;
        }
    }
}