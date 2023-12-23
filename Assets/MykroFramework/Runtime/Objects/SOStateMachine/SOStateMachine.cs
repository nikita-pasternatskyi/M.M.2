using MykroFramework.Runtime.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.SOStateMachine
{
    public class SOStateMachine : GetCachedComponent
    {
        [SerializeField] private StateContainer _startStateContainer;
        [SerializeField] private bool _createStateInstances = true;
        [SerializeField] private bool _initAllStatesOnStart;

        public StateContainer CurrentState { get; private set; }

        public event Action<StateContainer> NewStateEntered;
        public event Action<StateContainer> OldStateExited;

        private Dictionary<StateContainer, StateContainer> _states = new Dictionary<StateContainer, StateContainer>();

        private void OnApplicationQuit()
        {
            if (!_createStateInstances)
                return;
            foreach (var item in _states.Values)
            {
                Destroy(item);
            }
        }

        private void InitAllStates(StateContainer state)
        {
            StateContainer stateInstance = null;
            if (_createStateInstances)
            {
                stateInstance = Instantiate(state);
                stateInstance.Init(this);
                _states.Add(state, stateInstance);
            }
            else
            {
                state.Init(this);
                _states.Add(state, state);
            }
            foreach (var transitionState in state.Transitions)
            {
                if (transitionState.ToState == state || _states.ContainsKey(transitionState.ToState))
                    continue;
                InitAllStates(transitionState.ToState);
            }
            return;
        }

        private void Start()
        {
            if (_initAllStatesOnStart)
                InitAllStates(_startStateContainer);
            ChangeState(_startStateContainer);
        }

        public bool TryGetStateContainer(StateContainer container, out StateContainer stateContainer)
        {
            return _states.TryGetValue(container, out stateContainer);
        }

        private void Update()
        {
            CurrentState.Act();
            if (CurrentState.CheckTransitions(out StateContainer newStateContainer))
                ChangeState(newStateContainer);
        }

        private void FixedUpdate()
        {
            CurrentState.PhysicsAct();
        }

        public StateContainer ChangeState(StateContainer newStateContainer)
        {
            StateContainer newState = null;

            if (newStateContainer == null)
                return CurrentState;

            if (newStateContainer == CurrentState && !newStateContainer.CanTransitionToSelf)
                return CurrentState;

            if (!_states.TryGetValue(newStateContainer, out newState))
            {
                if (_createStateInstances)
                {
                    newState = Instantiate(newStateContainer);
                    newState.Init(this);
                    _states.Add(newStateContainer, newState);
                }
                else
                {
                    newStateContainer.Init(this);
                    newState = newStateContainer;
                    _states.Add(newStateContainer, newState);
                }
            }

            OldStateExited?.Invoke(CurrentState);
            CurrentState?.Exit();

            CurrentState = newState;

            NewStateEntered?.Invoke(CurrentState);
            CurrentState.Enter();
            return CurrentState;

        }
    }
}