using UnityEngine;

namespace MykroFramework.Runtime.Cameras.Camera2D
{
    [System.Serializable]
    public struct Vector2Bool
    {
        public bool X;
        public bool Y;
    }

    public class LanePoint : MonoBehaviour
    {
        public Vector2Bool WhatToLock;
        public Transform NextLanePoint;

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, 0.25f);
            if (NextLanePoint == null)
                return;
            Gizmos.DrawLine(transform.position, NextLanePoint.transform.position);
        }
    }
}
