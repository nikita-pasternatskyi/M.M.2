using System;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.SOStateMachine
{
    [Serializable]
    public class TestCondition : StateTransitionCondition
    {
        [SerializeField] private bool _transition;

        public override bool Check() => _transition;
    }
}