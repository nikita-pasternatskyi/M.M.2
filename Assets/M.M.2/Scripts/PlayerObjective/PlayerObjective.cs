using System;
using UnityEngine;

namespace MM2
{
    [CreateAssetMenu(menuName ="MM2/Player Objective")]
    public class PlayerObjective : ScriptableObject
    {
        public string Title;
        public string Description;
        public bool IsCompleted;
        public int AmountForCompletion;
        public event Action<PlayerObjective> Completed;
        public event Action AmountAdded;

        public void Complete()
        {
            Completed?.Invoke(this);
        }

        public void AddOneToAmount()
        {
            AmountAdded?.Invoke();
        }
    }
}
