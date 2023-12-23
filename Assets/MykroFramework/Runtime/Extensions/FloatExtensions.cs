using UnityEngine;

namespace MykroFramework.Runtime.Extensions
{
    public static class FloatExtensions
    {
        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static float FrameInpendentLerp(float v1, float v2, float smoothing, float delta)
        {
            float t = 1.0f - Mathf.Pow(smoothing, delta);
            return Mathf.Lerp(v1, v2, t);
        }

        public static float FrameInpendentSquaredLerp(float v1, float v2, float smoothing, float delta)
        {
            return FrameInpendentLerp(v1, v2, smoothing, delta * delta);
        }

        public static float FrameInpendentSqrRootLerp(float v1, float v2, float smoothing, float delta)
        {
            return FrameInpendentLerp(v1, v2, smoothing, Mathf.Sqrt(delta));
        }
    }
}
