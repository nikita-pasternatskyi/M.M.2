using UnityEngine;

namespace MykroFramework.Runtime.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 GetDirection(this Vector3 me, Vector3 to)
        {
            return (to - me).normalized;
        }
        public static float GetDistance(this Vector3 me, Vector3 to)
        {
            return (to - me).magnitude;
        }


        public static Vector3 XZ(this Vector3 me)
        {
            return new Vector3(me.x, 0, me.z);
        }
        public static Vector3 FramelessLerp(Vector3 v1, Vector3 v2, float smoothing, float delta)
        {
            float t = 1.0f - Mathf.Pow(smoothing, delta);
            return Vector3.Lerp(v1, v2, t);
        }
    }
}
