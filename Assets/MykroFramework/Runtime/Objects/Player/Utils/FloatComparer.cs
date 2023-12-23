namespace MykroFramework.Runtime.Objects.Player.Utils
{

    [System.Serializable]
    public class FloatComparer : ValueComparer<float, float>
    {
        override public bool Compare(float value)
        {
            switch (Operator)
            {
                case ComparisonOperator.More:
                    return value > ReferenceValue;
                case ComparisonOperator.Less:
                    return value < ReferenceValue;
                case ComparisonOperator.Equals:
                    return value == ReferenceValue;
                case ComparisonOperator.MoreEquals:
                    return value >= ReferenceValue;
                case ComparisonOperator.LessEquals:
                    return value <= ReferenceValue;
            }
            return false;
        }
    }
}