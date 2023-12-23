using System;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.InteractingWithEnvironment.Raycast
{
    [Serializable]
    public class RayCaster : EnvironmentRaycaster
    {
        [SerializeField] private Transform _referenceTransform;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private Vector3 _originOffset;
        [SerializeField] private float _angle;
        [SerializeField] private float _length;

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
            var direction = pointB - pointA;

            Gizmos.color = _debugColor;
            if (_debugHits)
            {
                if (Physics.Raycast(pointA, direction.normalized, out RaycastHit hit, _length, _layerMask))
                {
                    Gizmos.color = _debugGoodColor;
                    direction = direction.normalized * hit.distance;
                }
            }

            Gizmos.DrawRay(origin, direction);
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
            var direction = pointB - pointA;
            if (Physics.Raycast(pointA, direction.normalized, out RaycastHit hit, length, _layerMask))
            {
                environmentQueryResult = new EnvironmentQueryResult { Direction = direction.normalized, Hit = hit };
                return true;
            }
            return false;
        }
    }
}