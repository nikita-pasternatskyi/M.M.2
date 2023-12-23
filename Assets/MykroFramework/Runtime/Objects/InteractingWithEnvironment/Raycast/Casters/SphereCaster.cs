using System;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.InteractingWithEnvironment.Raycast
{
    [Serializable]
    public class SphereCaster : EnvironmentRaycaster
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _referenceTransform;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private Vector3 _originOffset;
        [SerializeField] private float _radius;
        [SerializeField] private float _angle;
        [SerializeField] private float _length;
        [SerializeField] private bool _shootInCenter;

        [Header("Debug")]
        [SerializeField] private bool _debug;
        [SerializeField] private bool _debugHits = false;
        [SerializeField] private Color _debugColor = Color.green;
        [SerializeField] private Color _debugGoodColor = Color.red;

        public override void DrawDebug()
        {
            if (!_debug)
                return;
            var forward = Quaternion.AngleAxis(_angle, _referenceTransform.right) * _referenceTransform.forward * _length;
            var origin = _referenceTransform.TransformPoint(_originOffset.x, _originOffset.y, _originOffset.z);
            var pointA = origin;
            var pointB = origin + forward;
            if(_shootInCenter)
                pointB = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, _length));
            var direction = pointB - pointA;

            Gizmos.color = _debugColor;
            Gizmos.DrawWireSphere(origin, _radius);
            if (_debugHits)
            {
                if (Physics.SphereCast(pointA, _radius, direction.normalized, out RaycastHit hit, _length, _layerMask))
                {
                    Gizmos.color = _debugGoodColor;
                    direction = direction.normalized * hit.distance;
                }
            }
            
            Gizmos.DrawWireSphere(origin, _radius);
            Gizmos.DrawRay(origin, direction);
            Gizmos.DrawWireSphere(origin + direction, _radius);
        }

        public override bool FindHit(out EnvironmentQueryResult environmentQueryResult, float length = -1)
        {
            if (length == -1)
                length = _length;
            environmentQueryResult = default;
            var forward = Quaternion.AngleAxis(_angle, _referenceTransform.right) * _referenceTransform.forward * length;
            var origin = _referenceTransform.TransformPoint(_originOffset.x, _originOffset.y, _originOffset.z);
            var pointA = origin;
            var pointB = origin + forward; 
            if (_shootInCenter)
                pointB = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, _length));
            var direction = pointB - pointA;
            if (Physics.SphereCast(pointA, _radius, direction.normalized, out RaycastHit hit, length, _layerMask))
            {
                environmentQueryResult = new EnvironmentQueryResult { Direction = direction.normalized, Hit = hit };
                return true;
            }
            return false;
        }
    }
}