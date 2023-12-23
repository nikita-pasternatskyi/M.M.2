using System;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.InteractingWithEnvironment.Raycast
{
    [Serializable]
    public class CenterScreenRay : EnvironmentRaycaster
    {
        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] private float _maxDistance;
        [SerializeField] private LayerMask _layerMask;

        private static Vector3 _point = new Vector3(0.5f, 0.5f, 0);

        public override void DrawDebug()
        {
        }

        public override bool FindHit(out EnvironmentQueryResult environmentQueryResult, float length = -1)
        {
            if (length == -1)
                length = _maxDistance;
            environmentQueryResult = default;
            var ray = _camera.ViewportPointToRay(_point);
            if (Physics.Raycast(ray, out RaycastHit hit, length, _layerMask))
            {
                environmentQueryResult = new EnvironmentQueryResult() { Direction = ray.direction, Hit = hit };
                return true;
            }
            return false;
        }
    }
}
