using MykroFramework.Runtime.Extensions;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.InteractingWithEnvironment.Raycast
{
    public class CircleEnvironmentRaycast : EnvironmentRaycast2D
    {
        [SerializeField] private Transform _referenceTransform;
        [SerializeField] private float _distance;
        [SerializeField] private float _angle;
        [SerializeField] private float _radius;
        [SerializeField] private LayerMask _layerMask;

        public override void DrawDebug()
        {
            if (!_referenceTransform)
                return;
            var rotatedVector = Vector2Extensions.CreateRotatedVector(_angle);
            var destination = _referenceTransform.position + new Vector3(rotatedVector.x, rotatedVector.y, 0) * _distance;
            Gizmos.DrawLine(_referenceTransform.transform.position, destination);
            Gizmos.DrawWireSphere(destination, _radius);
        }

        public override bool FindHit(out EnvironmentQueryResult2D environmentQueryResult, float length = -1)
        {
            if (length == -1)
                length = _distance;
            environmentQueryResult = default;
            var direction = Vector2Extensions.CreateRotatedVector(_angle);
            var cast = Physics2D.CircleCast(_referenceTransform.position, _radius, direction, length, _layerMask);
            if (cast)
            {
                environmentQueryResult = new EnvironmentQueryResult2D();
                environmentQueryResult.Direction = direction;
                environmentQueryResult.Hit = cast;
                return true;
            }
            return false;
        }
    }
}
