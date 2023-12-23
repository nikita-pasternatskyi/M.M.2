using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.InteractingWithEnvironment.Raycast
{

    [Serializable]
    public class ConeCaster : EnvironmentRaycaster
    {
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Transform _referenceTransform;
        [SerializeField] private float _startRadius;
        [SerializeField] private float _endRadius;
        [SerializeField] [Range(0, 1)] private float _radiusFactor = 0.5f;
        [SerializeField] private int _rayCount;
        [SerializeField] private float _length;
        [SerializeField] private float _angle = 45;

        [Header("Debug")]
        [SerializeField] private bool _debug;
        [SerializeField] private bool _debugHits = false;
        [SerializeField] private Color _debugColor = Color.green;
        [SerializeField] private Color _debugGoodColor = Color.red;

        private List<(Vector2, Vector2)> _builtPoints;

        public void Init()
        {
            _builtPoints = BuildPoints(_startRadius, _endRadius);
        }

        public override void DrawDebug()
        {
            if (!_debug)
                return;
            var endRadius = Mathf.Lerp(_startRadius, _endRadius, _radiusFactor);
            if(!Application.isPlaying)
                _builtPoints = BuildPoints(_startRadius, endRadius);
            foreach (var point in _builtPoints)
            {
                Gizmos.color = _debugColor;
                var forward = Quaternion.AngleAxis(_angle, _referenceTransform.right) * _referenceTransform.forward * _length;
                var transformedPoint = _referenceTransform.TransformPoint(point.Item1.x + _offset.x, point.Item1.y + _offset.y, _offset.z);
                var transformedPoint2 = _referenceTransform.TransformPoint(point.Item2.x + _offset.x, point.Item2.y + _offset.y, _offset.z) + forward;
                var direction = transformedPoint2 - transformedPoint;
                if (_debugHits)
                {
                    if (Physics.Raycast(transformedPoint, direction.normalized, out RaycastHit hit, _length, _layerMask))
                    {
                        Gizmos.color = _debugGoodColor;
                        direction = direction.normalized * hit.distance;
                    }
                }
                Gizmos.DrawRay(transformedPoint, direction);
            }
        }

        private List<(Vector2, Vector2)> BuildPoints(float radius, float endRadius)
        {
            List<(Vector2, Vector2)> points = new List<(Vector2, Vector2)>();
            for (int i = 0; i < _rayCount; i++)
            {
                float angle = (float)i / _rayCount * 360;
                var x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
                var y = radius * Mathf.Sin(angle * Mathf.Deg2Rad);

                var x1 = endRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
                var y1 = endRadius * Mathf.Sin(angle * Mathf.Deg2Rad);

                points.Add((new Vector2(x, y), new Vector2(x1, y1)));
            }
            return points;
        }


        public override bool FindHit(out EnvironmentQueryResult environmentQueryResult, float length = -1)
        {
            environmentQueryResult = default;
            float factor = 0;
            int steps = 20;
            if (length == -1)
                length = _length;
            for (int i = 0; i < steps; i++)
            {
                factor += 1f / steps;
                float endRadius = Mathf.Lerp(_startRadius, _endRadius, factor);
                foreach (var point in _builtPoints)
                {
                    var forward = Quaternion.AngleAxis(_angle, _referenceTransform.right) * _referenceTransform.forward * length;
                    var transformedPoint = _referenceTransform.TransformPoint(point.Item1.x, point.Item1.y, 0);
                    var transformedPoint2 = _referenceTransform.TransformPoint(point.Item2.x, point.Item2.y, 0) + forward;
                    var direction = transformedPoint2 - transformedPoint;
                    if (Physics.Raycast(transformedPoint, direction.normalized, out RaycastHit hit, length, _layerMask))
                    {
                        environmentQueryResult = new EnvironmentQueryResult { Direction = direction.normalized, Hit = hit };
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
