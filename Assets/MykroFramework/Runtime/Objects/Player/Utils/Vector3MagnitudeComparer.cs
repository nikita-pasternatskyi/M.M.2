using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player.Utils
{
    [System.Serializable]
    public class Vector3MagnitudeComparer : ValueComparer<float, Vector3>
    {
        override public bool Compare(Vector3 value)
        {
            var magnitude = value.magnitude;
            switch (Operator)
            {
                case ComparisonOperator.More:
                    return magnitude > ReferenceValue;
                case ComparisonOperator.Less:
                    return magnitude < ReferenceValue;
                case ComparisonOperator.Equals:
                    return magnitude == ReferenceValue;
                case ComparisonOperator.MoreEquals:
                    return magnitude >= ReferenceValue;
                case ComparisonOperator.LessEquals:
                    return magnitude <= ReferenceValue;
            }
            return false;
        }
    }
}