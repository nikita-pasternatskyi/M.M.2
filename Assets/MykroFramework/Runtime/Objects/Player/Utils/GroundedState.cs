using System.Collections;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player
{

    public class GroundedState : MonoBehaviour
    {
        [SerializeField] private LayerMask _whatIsGround;
        [SerializeField] private Vector3 _groundCheckOffset;
        [SerializeField] private float _groundCheckLength;
        [SerializeField] private Vector3 _groundCheckExtents;
        [SerializeField] private float _maxGroundAngle = -1;
        public Vector3 GroundNormal { get; private set; }

        public bool Grounded { get; private set; }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawLine(transform.position, GetGroundRayEndPosition());
            Gizmos.DrawWireCube(GetGroundRayEndPosition(), _groundCheckExtents);
        }

        private Vector3 GetGroundRayEndPosition() => transform.position + _groundCheckOffset - transform.up * _groundCheckLength;
#endif

        public float GetDistanceToGround(float yOffset)
        {
            var pos = transform.position;
            pos += transform.up * yOffset;
            if (Physics.Raycast(pos, -transform.up, out RaycastHit hit, float.MaxValue, _whatIsGround))
            {
                return hit.distance;
            }
            return float.MaxValue;
        }

        public bool CheckGround(float length)
        {
            if (Physics.BoxCast(transform.position, _groundCheckExtents, -transform.up, out RaycastHit hit, Quaternion.identity, length, _whatIsGround))
            {
                if (Vector3.Dot(hit.normal, transform.up) > _maxGroundAngle)
                {
                    GroundNormal = hit.normal;
                    return true;
                }
            }
            return false;
        }

        private void FixedUpdate()
        {
            Grounded = CheckGround(_groundCheckLength);
            if (!Grounded)
                GroundNormal = Vector3.up;
        }
    }
}