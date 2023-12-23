using MykroFramework.Runtime.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.InteractingWithEnvironment.Raycast
{
    public class RectangleEnvironmentRaycast : EnvironmentRaycast2D
    {
        [SerializeField] private Transform _referenceTransform;
        [SerializeField] private float _distance;
        [SerializeField] private float _angle;
        [SerializeField] private Vector2 _extents;
        [SerializeField] private ContactFilter2D _contactFilter;
        private List<RaycastHit2D> _results = new List<RaycastHit2D>(8);

        public override void DrawDebug()
        {
            if (!_referenceTransform)
                return;
            var rotatedVector = Vector2Extensions.CreateRotatedVector(_angle);
            var destination = _referenceTransform.position + new Vector3(rotatedVector.x, rotatedVector.y, 0) * _distance;
            Gizmos.DrawLine(_referenceTransform.transform.position, destination);
            Gizmos.DrawWireCube(destination, _extents);
        }

        public override bool FindHit(out EnvironmentQueryResult2D environmentQueryResult, float length = -1)
        {
            environmentQueryResult = default;
            var direction = Vector2Extensions.CreateRotatedVector(_angle);
            if (length == -1)
                length = _distance;
            var cast = Physics2D.BoxCast(_referenceTransform.position, _extents, 0, direction, _contactFilter, _results, length);
            if (cast != 0)
            {
                foreach (var item in _results)
                {
                    if (item.collider == null)
                        continue;
                    environmentQueryResult = new EnvironmentQueryResult2D();
                    environmentQueryResult.Direction = direction;
                    environmentQueryResult.Hit = item;

                }
                return true;
            }
            return false;
        }
    }
}
