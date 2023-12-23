using System;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.SOStateMachine.Conditions
{
    [Serializable]
    public class TimerCondition : StateTransitionCondition
    {
        [SerializeField] private float _timeToWait;

        private float _timer;

        public override void Exit()
        {
            _timer = 0;
        }

        public override bool Check()
        {
            _timer = Mathf.Clamp(_timer + Time.deltaTime, 0, _timeToWait);
            if (_timer >= _timeToWait)
                return true;
            return false;
        }
    }
}