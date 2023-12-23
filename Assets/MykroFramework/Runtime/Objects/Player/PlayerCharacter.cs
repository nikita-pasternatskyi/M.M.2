using MykroFramework.Runtime.Extensions;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player
{
    [RequireComponent(typeof(GroundedState))]
    public class PlayerCharacter : MonoBehaviour
    {
        [Header("General")]
        public float Gravity;
        public float MinimumYVelocity;

        [Header("Drag")]
        public float AirDrag;
        public float GroundDrag;

        public event Action Jumped;
        public bool JumpedB;

        public CharacterController CharacterController { get; private set; }

        private GroundedState _grounded;

        [ReadOnly] public Vector3 Velocity;
        [ReadOnly] public float Drag;

        private float _gravity;

        [ReadOnly] public float TopSpeed;

        private void Awake()
        {
            _grounded = GetComponent<GroundedState>();
            CharacterController = GetComponent<CharacterController>();
            _gravity = Gravity;
        }

        private void FixedUpdate()
        {
            if (_grounded.Grounded)
            {
                Drag = GroundDrag;
                Velocity.y = Mathf.Clamp(Velocity.y, 0, float.MaxValue);
            }
            else
            {
                Drag = AirDrag;
                Velocity.y += _gravity * Time.fixedDeltaTime * Time.fixedDeltaTime;
                Velocity.y = Mathf.Clamp(Velocity.y, MinimumYVelocity, float.MaxValue);
            }
            CharacterController.Move(Velocity);
        }


        private void LateUpdate()
        {
            JumpedB = false;
        }

        public void Move(float topSpeed, float acceleration, Vector3 direction)
        {
            var flatVelocity = Velocity.XZ();
            flatVelocity = Vector3Extensions.FramelessLerp(flatVelocity, topSpeed * direction, acceleration, Time.deltaTime);
            Velocity.x = flatVelocity.x;
            Velocity.z = flatVelocity.z;
        }

        public void DisableCharacterController()
        {
            CharacterController.enabled = false;
        }
        public void EnableCharacterController()
        {
            CharacterController.enabled = true;
        }

        public void Jump(Vector3 jumpVector, Vector3 forward)
        {
            JumpedB = true;
            Drag = AirDrag;
            float jumpHeight = Mathf.Sqrt(-2 * (_gravity * Time.fixedDeltaTime * Time.fixedDeltaTime) * jumpVector.y);
            Vector3 forwardForce = Vector3.zero;

            if (jumpVector.z != 0)
                forwardForce = jumpVector.z * forward;

            Velocity.y = jumpHeight;
            Jumped?.Invoke();
            if (jumpVector.z == 0)
                return;
            Velocity.x = forwardForce.x;
            Velocity.z = forwardForce.z;
        }

        public void Jump(Vector3 jumpVector, bool calculateYHeight = true)
        {
            JumpedB = true;
            Drag = AirDrag;
            float jumpHeight = Mathf.Sqrt(-2 * (_gravity * Time.fixedDeltaTime * Time.fixedDeltaTime) * jumpVector.y);
            if (!calculateYHeight)
                jumpHeight = jumpVector.y;
            Vector3 forwardForce = Vector3.zero;

            Velocity.y = jumpHeight;
            Jumped?.Invoke();
            if (jumpVector.z == 0)
                return;
            Velocity.x = jumpVector.x;
            Velocity.z = jumpVector.z;
        }

        public void SetGravity(float gravity)
        {
            _gravity = gravity;
        }

        public void ResetGravity()
        {
            _gravity = Gravity;
        }
    }
}