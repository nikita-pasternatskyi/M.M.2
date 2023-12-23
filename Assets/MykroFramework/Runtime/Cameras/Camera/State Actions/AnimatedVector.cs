using System;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player.Camera.StateActions
{
    [Serializable]
    public struct AnimatedVector
    {
        [SerializeField] private AnimatedFloat _x;
        [SerializeField] private AnimatedFloat _y;
        [SerializeField] private AnimatedFloat _z;

        public Vector3 GetValue(Vector3 startValue, Vector3 current)
        {
            Vector3 result = new Vector3(
                _x.GetValue(startValue.x, current.x),
                _y.GetValue(startValue.y, current.y),
                _z.GetValue(startValue.z, current.z));
            return result;
        }
    }
}
