using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player.WebSwinging
{
    public struct SwingPoint
    {
        public Vector3 StartPosition;
        public float StartLength;

        public SwingPoint(Vector3 position, float length)
        {
            StartPosition = position;
            StartLength = length;
        }
    }
}