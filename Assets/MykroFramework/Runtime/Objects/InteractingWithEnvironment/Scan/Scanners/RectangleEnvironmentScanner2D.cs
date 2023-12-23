using UnityEngine;

namespace MykroFramework.Runtime.Objects.InteractingWithEnvironment.Scan.Scanners
{
    public class RectangleEnvironmentScanner2D : EnvironmentScanner2D
    {
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private Vector2 _size;
        [SerializeField] private Color _debugColor;
        [SerializeField] private Vector3 _offset;

        public override void DrawDebug()
        {
            Gizmos.color = _debugColor;
            var position = Transform.position + _offset;
            Gizmos.DrawWireCube(position, _size);
        }

        public override Collider2D[] GetHits(Vector3 position)
        {
            return Physics2D.OverlapBoxAll(position, _size, 0f, _layerMask);
        }
    }
}
