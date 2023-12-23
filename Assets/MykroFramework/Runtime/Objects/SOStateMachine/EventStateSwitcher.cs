using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.SOStateMachine
{
    public enum StateEvent
    {
        Enter, Exit,
    }
    public class EventStateSwitcher : SerializedMonoBehaviour
    {
        [SerializeField] private SOStateMachine StateMachine;
        [SerializeField] private SOStateMachine StateMachineToFollow;

        [SerializeField] private Dictionary<StateContainer, StateContainer> _onEnterRules = new Dictionary<StateContainer, StateContainer>();
        [SerializeField] private Dictionary<StateContainer, StateContainer> _onExitRules = new Dictionary<StateContainer, StateContainer>();

        private void OnValidate()
        {
            if(_onEnterRules == null)
                _onEnterRules = new Dictionary<StateContainer, StateContainer>();
            if (_onExitRules == null)
                _onEnterRules = new Dictionary<StateContainer, StateContainer>();
        }

        private void OnEnable()
        {
            StateMachineToFollow.NewStateEntered += OnNewStateEntered;
            StateMachineToFollow.OldStateExited += OldStateExited;
        }

        private void OnDisable()
        {
            StateMachineToFollow.NewStateEntered -= OnNewStateEntered;
            StateMachineToFollow.OldStateExited -= OldStateExited;
        }

        private void OldStateExited(StateContainer obj)
        {
            if (obj == null)
                return;
            if (_onExitRules.TryGetValue(obj, out StateContainer newState))
            {
                StateMachine.ChangeState(newState);
            }
        }

        private void OnNewStateEntered(StateContainer obj)
        {
            if (obj == null)
                return;

            if (_onEnterRules.TryGetValue(obj, out StateContainer newState))
            {
                StateMachine.ChangeState(newState);
            }
        }
    }

}
