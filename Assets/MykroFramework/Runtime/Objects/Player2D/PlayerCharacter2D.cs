using MykroFramework.Runtime.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace MykroFramework.Runtime.Objects.Player2D
{
    public class PlayerCharacter2D : MonoBehaviour
    {
        [Header("Collision")]
        [SerializeField] private int _maxGroundAngle;
        [SerializeField] private ContactFilter2D _collidable;
        [SerializeField] private int _freeColliderIterations;
        private Rigidbody2D _rigidbody2D;

        [Header("General")]
        public float MinimumYVelocity;
        public float GravityScale;

        public UnityEvent Jumped;
        public UnityEvent Fell;
        public UnityEvent Landed;

        public UnityEvent StartedMoving;
        public UnityEvent StoppedMoving;

        [ReadOnly] public Vector2 Velocity;
        [ReadOnly] public Vector2 FrameVelocity;
        [ReadOnly] public float Drag;

        private float _originalGravityScale;

        [ReadOnly] public float TopSpeed;
        private Vector2 _defaultNormal = new Vector2(0, 1);
        private Vector2 _lastPosition;

        public bool Grounded { get; private set; }
        public Vector2 GroundNormal { get; private set; }

        private ContactPoint2D[] _contacts = new ContactPoint2D[8];

        private void Awake()
        {
            TryGetComponent(out _rigidbody2D);
            _originalGravityScale = GravityScale;
            _lastPosition = transform.position;
        }

        private void Update()
        {
            var currentPosition = new Vector2(transform.position.x, transform.position.y);
            var velocity = (currentPosition - _lastPosition) / Time.deltaTime;
            FrameVelocity = velocity;
            _lastPosition = currentPosition;

            var grounded = false;
            int numContacts = _rigidbody2D.GetContacts(_collidable, _contacts);
            for (int i = 0; i < numContacts; i++)
            {
                var item = _contacts[i];
                if (Vector2.Angle(item.normal, transform.up) <= _maxGroundAngle)
                {
                    GroundNormal = item.normal;
                    grounded = true;
                }
            }

            if (grounded)
            {
                if (Velocity.x != 0 && velocity.x == 0)
                    StartedMoving?.Invoke();

                if (Velocity.x == 0 && velocity.x != 0)
                    StoppedMoving?.Invoke();
            }

            if (!Grounded && grounded)
                Landed?.Invoke();
            else if (Grounded && !grounded && Velocity.y <= 0)
                Fell?.Invoke();

            Grounded = grounded;
            if (grounded)
            {
                if (Velocity.y < 0)
                    Velocity.y = 0;
            }
            else
                Velocity.y = Mathf.Clamp(Velocity.y + Physics2D.gravity.y * GravityScale * Time.deltaTime, MinimumYVelocity, float.MaxValue);

            _rigidbody2D.velocity = Velocity;
            Velocity.x = 0;
        }

        public void Move(float topSpeed, float acceleration, Vector2 direction, bool stickToFloor = false)
        {
                Velocity.x = topSpeed * direction.x;
        }

        public void Jump(Vector2 jumpVector, bool calculateYJump = true)
        {
            float jumpHeight = calculateYJump == true ?
            Mathf.Sqrt(-2 * (GravityScale * Physics2D.gravity.y) * jumpVector.y)
            : jumpVector.y;
            Velocity.y = jumpHeight;
            Jumped?.Invoke();
            if (jumpVector.x == 0)
                return;
            Velocity.x = jumpVector.x;
        }

        public void SetGravity(float gravity)
        {
            GravityScale = gravity;
        }

        public void ResetGravity()
        {
            GravityScale = _originalGravityScale;
        }
    }
}
