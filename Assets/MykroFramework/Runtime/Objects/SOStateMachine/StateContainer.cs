using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using MykroFramework.Runtime.Extensions;

namespace MykroFramework.Runtime.Objects.SOStateMachine
{
    [CreateAssetMenu(menuName = "StateMachine/State Container")]
    public class StateContainer : SerializedScriptableObject
    {
        [SerializeField] [ListDrawerSettings(AlwaysAddDefaultValue = true, ListElementLabelName = "Name")] public StateTransition[] Transitions;
        [SerializeReference] [ListDrawerSettings(AlwaysAddDefaultValue = true, ListElementLabelName = "Name")] public StateAction[] StateActions;
        public bool CanTransitionToSelf = false;

        private Dictionary<System.Type, StateAction> _stateActionDictionary;

        public StateContainer CreateState(SOStateMachine stateMachine)
        {
            Init(stateMachine);
            var newState = Instantiate(this);
            return newState;
        }

        public void Init(SOStateMachine stateMachine)
        {
            _stateActionDictionary = new Dictionary<System.Type, StateAction>(StateActions.Length);
            foreach (var item in StateActions)
            {
                item.Init(stateMachine);
                _stateActionDictionary.Add(item.GetType(), item);
            }
            for (int i = 0; i < Transitions.Length; i++)
            {
                Transitions[i].Init(stateMachine);
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
            foreach (var transition in Transitions)
            {
                newStateContainer = transition.CheckTransition();
                if (newStateContainer != null)
                    return true;
            }
            return false;
        }

        public void Act()
        {
            for (int i = 0; i < StateActions.Length; i++)
            {
                StateActions[i].Act();
            }
        }

        public void PhysicsAct()
        {
            for (int i = 0; i < StateActions.Length; i++)
            {
                StateActions[i].PhysicsAct();
            }
        }

        public void Enter()
        {
            for (int i = 0; i < StateActions.Length; i++)
            {
                StateActions[i].Enter();
            }

            for (int i = 0; i < Transitions.Length; i++)
            {
                Transitions[i].Enter();
            }
        }

        public void Exit()
        {
            for (int i = 0; i < StateActions.Length; i++)
            {
                StateActions[i].Exit();
            }

            for (int i = 0; i < Transitions.Length; i++)
            {
                Transitions[i].Exit();
            }
        }
    }
}