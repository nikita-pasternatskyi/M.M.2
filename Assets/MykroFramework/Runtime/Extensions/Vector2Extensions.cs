using UnityEngine;

namespace MykroFramework.Runtime.Extensions
{
    public static class Vector2Extensions
    {
        public static Vector2 GetDirection(this Vector2 me, Vector2 to)
        {
            return (to - me).normalized;
        }

        public static Vector2 CreateRotatedVector(float degrees)
        {
            degrees = degrees * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(degrees), Mathf.Sin(degrees));
        }

        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            float tx = v.x;
            float ty = v.y;
            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);
            return v;
        }
    }
}
