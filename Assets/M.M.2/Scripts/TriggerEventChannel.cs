using MykroFramework.Runtime.Objects.SOAnimationMachine;
using System;
using UnityEngine;

namespace MM2
{
    [CreateAssetMenu(menuName = "Game/Trigger Event Channel")]
    public class TriggerEventChannel : ScriptableObject
    {
        public event Action Triggered;

        public void Trigger()
        {
            Triggered?.Invoke();
        }
    }
}
