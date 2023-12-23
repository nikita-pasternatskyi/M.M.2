using MykroFramework.Runtime.Extensions;
using System;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player.Camera.StateActions
{
    [Serializable]
    public struct AnimatedFloat
    {
        public float TargetValue;
        [Range(0, 1)] public float Smoothness;

        public float GetValue(float startValue, float current)
        {
            if (TargetValue != startValue)
                return FloatExtensions.FrameInpendentLerp(current, TargetValue, Smoothness, Time.deltaTime);
            return startValue;
        }
    }
}
