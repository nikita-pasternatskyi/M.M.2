using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace MykroFramework.Runtime.Cameras.Camera2D
{
    public class Lane : MonoBehaviour
    {
        [ListDrawerSettings(CustomAddFunction = "CreatePoint")] public List<LanePoint> Points;
        public Transform Smallest;
        public Transform Largest;

        private void OnDrawGizmos()
        {
            if (Points.Count == 0)
                return;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(Points[0].transform.position, 0.5f);

            if (Smallest != null && Largest != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(Smallest.transform.position, 0.75f);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(Largest.transform.position, 0.75f);
            }
        }

        private LanePoint CreatePoint()
        {
            var go = new GameObject();
            go.transform.parent = transform;
            if (Points.Count != 0)
            {
                Points[Points.Count - 1].NextLanePoint = go.transform;
                go.transform.position = Points[Points.Count - 1].transform.position;
            }
            return go.AddComponent<LanePoint>();
        }

        public Vector2 AlignPositionToLane(Transform transform)
        {
            var limits = new Limits2D();
            Vector2 min = transform.position;
            Vector2 max = transform.position;
            int result = 0;

            var firstPoint = Points[0];
            var lastPoint = Points[Points.Count - 1];
            var previousToLastPoint = Points[Points.Count - 2];
            if (lastPoint.transform.position.x < min.x)
            {
                limits.MinXY = new Vector2(previousToLastPoint.transform.position.x, previousToLastPoint.transform.position.y);
                limits.MaxXY = new Vector2(lastPoint.transform.position.x, lastPoint.transform.position.y);
            }
            if (firstPoint.transform.position.x > min.x)
            {
                limits.MinXY = new Vector2(firstPoint.transform.position.x, firstPoint.transform.position.y);
                Transform nextPoint = firstPoint.transform;
                if (firstPoint.NextLanePoint != null)
                    nextPoint = firstPoint.NextLanePoint;
                limits.MaxXY = new Vector2(nextPoint.transform.position.x, nextPoint.transform.position.y);
            }

            for (int i = 0; i < Points.Count; i++)
            {
                LanePoint item = Points[i];

                if (item.transform.position.x < transform.position.x)
                {
                    result = i;
                    min.x = item.transform.position.x;
                    min.y = item.transform.position.y;
                }
                else if (item.transform.position.x == min.x)
                {
                    if (item.transform.position.y < min.y)
                    {
                        result = i;
                        min.y = item.transform.position.y;
                    }
                }
            }

            limits.MaxXY = limits.MinXY = min;
            Largest = Smallest = Points[result].transform;
            if (Points[result].NextLanePoint != null)
            {
                limits.MaxXY = Points[result].NextLanePoint.transform.position;
                Largest = Points[result].NextLanePoint;
            }
            var direction = limits.MaxXY - limits.MinXY;
            direction.Normalize();
            return Vector3.ProjectOnPlane(transform.position, direction);
        }

        public LanePoint GetClosestPoint(Vector3 position)
        {
            LanePoint lanePoint = null;
            for (int i = 0; i < Points.Count; i++)
            {
                LanePoint item = Points[i];

                if (item.transform.position.x < position.x)
                {
                    lanePoint = Points[i];
                }
                else if (item.transform.position.x == position.x)
                {
                    if (item.transform.position.y < position.y)
                    {
                        lanePoint = Points[i];
                    }
                }
            }
            return lanePoint;
        }

        public (Vector2 up,Vector2 right) GetDirections(Transform transform)
        {
            int result = 0;
            Vector2 up = new Vector2();
            Vector2 right = new Vector2();

            for (int i = 0; i < Points.Count; i++)
            {
                LanePoint item = Points[i];

                if (item.transform.position.x < transform.position.x)
                {
                    result = i;
                }
                else if (item.transform.position.x == transform.position.x)
                {
                    if (item.transform.position.y < transform.position.y)
                    {
                        result = i;
                    }
                }
            }
            up = Vector2.up;
            right = (Points[result].NextLanePoint.transform.position - Points[result].transform.position).normalized;
            return (up, right);
        }

        public Limits2D GetLimits(Transform transform)
        {
            var limits = new Limits2D();
            Vector2 min = transform.position;
            Vector2 max = transform.position;
            int result = 0;

            var firstPoint = Points[0];
            var lastPoint = Points[Points.Count - 1];
            var previousToLastPoint = Points[Points.Count - 2];
            if (lastPoint.transform.position.x < min.x)
            {
                limits.MinXY = new Vector2(previousToLastPoint.transform.position.x, previousToLastPoint.transform.position.y);
                limits.MaxXY = new Vector2(lastPoint.transform.position.x, lastPoint.transform.position.y);
                return limits;
            }
            if (firstPoint.transform.position.x > min.x)
            {
                limits.MinXY = new Vector2(firstPoint.transform.position.x, firstPoint.transform.position.y);
                Transform nextPoint = firstPoint.transform;
                if (firstPoint.NextLanePoint != null)
                    nextPoint = firstPoint.NextLanePoint;
                limits.MaxXY = new Vector2(nextPoint.transform.position.x, nextPoint.transform.position.y);
                return limits;
            }

            for (int i = 0; i < Points.Count; i++)
            {
                LanePoint item = Points[i];
                
                if (item.transform.position.x < transform.position.x)
                {
                    result = i;
                    min.x = item.transform.position.x;
                    min.y = item.transform.position.y;
                }
                else if (item.transform.position.x == min.x)
                {
                    if (item.transform.position.y < min.y)
                    {
                        result = i;
                        min.y = item.transform.position.y;
                    }
                }
            }

            limits.MaxXY = limits.MinXY = min;
            Largest = Smallest = Points[result].transform;
            if (Points[result].NextLanePoint != null)
            {
                limits.MaxXY = Points[result].NextLanePoint.transform.position;
                Largest = Points[result].NextLanePoint;
            }
            return limits;
        }
    }
}
