using MykroFramework.Runtime.Extensions;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player
{
    public class NextPlayerCharacter : MonoBehaviour
    {
        [SerializeField] private float _minimumYVelocity;
        [SerializeField] private BoxCollider _collider;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] public float GravityScale;
        [SerializeField] private bool _simulate;

        [ReadOnly] public Vector3 Velocity;
        [ReadOnly] public Vector3 PhysicsPosition;

        private GroundedState _grounded;

        [ReadOnly] public float TopSpeed;
        private Collider[] _results = new Collider[16];

        private void Awake()
        {
            _grounded = GetComponent<GroundedState>();
            PhysicsPosition = transform.position;
        }


        private void FixedUpdate()
        {
            if (!_simulate)
                return;
            if (_grounded.Grounded)
            {
                Velocity.y = Mathf.Clamp(Velocity.y, 0, float.MaxValue);
            }
            else
            {
                Velocity.y += Physics.gravity.y * GravityScale * Time.deltaTime * Time.deltaTime;
                Velocity.y = Mathf.Clamp(Velocity.y, _minimumYVelocity, float.MaxValue);
            }

            var velocity = Velocity * Time.fixedDeltaTime + Velocity * Time.fixedDeltaTime;
            PhysicsPosition += velocity;

            if (Physics.OverlapBoxNonAlloc(PhysicsPosition + velocity, _collider.size * 0.5f, _results, 
                Quaternion.identity, _layerMask, QueryTriggerInteraction.Ignore) != 0)
            {
                foreach (var collider in _results)
                {
                    if (collider == null)
                        continue;
                    if (Physics.ComputePenetration(_collider, PhysicsPosition, transform.rotation, 
                        collider, collider.transform.position, collider.transform.rotation, 
                        out Vector3 direction, out float distance))
                    {
                        PhysicsPosition += direction * distance;
                    }
                }
            }
            transform.position = PhysicsPosition;
        }

        public void Move(float topSpeed, float smoothing, Vector3 direction)
        {
            var flatVelocity = Velocity.XZ();
            flatVelocity = Vector3Extensions.FramelessLerp(flatVelocity, topSpeed * direction, smoothing, Time.deltaTime);
            Velocity.x = flatVelocity.x;
            Velocity.z = flatVelocity.z;
        }

        public void Jump(Vector3 jumpVector, Vector3 forward, bool calculateYHeight = true)
        {
            float jumpHeight = Mathf.Sqrt(-2 * (Physics.gravity.y * GravityScale * Time.fixedDeltaTime) * jumpVector.y);
            if (!calculateYHeight)
                jumpHeight = jumpVector.y;
            Vector3 forwardForce = Vector3.zero;

            if (jumpVector.z != 0)
                forwardForce = jumpVector.z * forward;

            Velocity.y = jumpHeight;
            if (jumpVector.z == 0)
                return;
            Velocity.x = forwardForce.x;
            Velocity.z = forwardForce.z;
        }
    }
}
