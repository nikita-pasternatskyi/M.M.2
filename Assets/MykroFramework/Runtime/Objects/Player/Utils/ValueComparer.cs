using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player.Utils
{
    public abstract class ValueComparer<T, T2>
    {
        protected enum ComparisonOperator
        {
            More,
            Less,
            Equals,
            MoreEquals,
            LessEquals,
        }

        [SerializeField] protected T ReferenceValue;
        [SerializeField] protected ComparisonOperator Operator;

        public abstract bool Compare(T2 value);
    }
}