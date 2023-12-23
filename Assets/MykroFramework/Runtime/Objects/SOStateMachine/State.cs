using System.Collections.Generic;

namespace MykroFramework.Runtime.Objects.SOStateMachine
{
    public sealed class State
    {
        private StateTransition[] _stateTransitions;
        private StateAction[] _stateActions;
        private Dictionary<System.Type, StateAction> _stateActionDictionary;

        public State(StateTransition[] stateTransitions, StateAction[] stateActions, SOStateMachine stateMachine) 
        {
            _stateActionDictionary = new Dictionary<System.Type, StateAction>(stateActions.Length);
            _stateActions = stateActions;
            _stateTransitions = stateTransitions;

            //call Init() for all actions, transitions, conditions
            for (int i = 0; i < _stateActions.Length; i++)
            {
                _stateActions[i].Init(stateMachine);
                _stateActionDictionary.Add(_stateActions[i].GetType(), _stateActions[i]);
            }

            for (int i = 0; i < _stateTransitions.Length; i++)
            {
                _stateTransitions[i].Init(stateMachine);
            }
        }

        public bool TryGetStateAction<T>(out T stateAction) where T : StateAction
        {
            bool value = _stateActionDictionary.TryGetValue(typeof(T), out StateAction action);
            stateAction = (T)action;
            return value;
        }

        public bool CheckTransitions(out StateContainer newStateContainer)
        {
            newStateContainer = null;
            foreach (var transition in _stateTransitions)
            {
                newStateContainer = transition.CheckTransition();
                if (newStateContainer != null)
                    return true;
            }
            return false;
        }

        public void Act()
        {
            for (int i = 0; i < _stateActions.Length; i++)
            {
                _stateActions[i].Act();
            }
        }

        public void PhysicsAct()
        {
            for (int i = 0; i < _stateActions.Length; i++)
            {
                _stateActions[i].PhysicsAct();
            }
        }

        public void Enter()
        {
            for (int i = 0; i < _stateActions.Length; i++)
            {
                _stateActions[i].Enter();
            }

            for (int i = 0; i < _stateTransitions.Length; i++)
            {
                _stateTransitions[i].Enter();
            }
        }

        public void Exit()
        {
            for (int i = 0; i < _stateActions.Length; i++)
            {
                _stateActions[i].Exit();
            }

            for (int i = 0; i < _stateTransitions.Length; i++)
            {
                _stateTransitions[i].Exit();
            }
        }
    }
}