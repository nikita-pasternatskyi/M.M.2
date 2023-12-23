using MykroFramework.Runtime.Objects.InteractingWithEnvironment.Raycast;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player2D.Utils
{
    public class GroundedState2D : MonoBehaviour
    {
        [SerializeReference] private EnvironmentRaycast2D _groundCheck;
        [SerializeField] private LayerMask _whatIsGround;
        public bool Grounded;
        public bool JumpReady;
        public Vector2 Normal { get; private set; }
        public Vector2 LastGroundPoint { get; private set; }
        public float Distance { get; private set; }
        public float DistanceToGround { get; private set; }

        private void OnDrawGizmos()
        {
            _groundCheck.DrawDebug();
        }

        private void FixedUpdate()
        {
            var cast = Physics2D.Raycast(transform.position, Vector2.down, 1000, _whatIsGround);
            if (cast.transform != null)
            {
                DistanceToGround = cast.distance;
            }

           
            if (_groundCheck.FindHit(out EnvironmentQueryResult2D result))
            {
                Grounded = true;
                Normal = result.Hit.normal;
                LastGroundPoint = result.Hit.point;
                Distance = result.Hit.distance;
                return;
            }
            Distance = 1000;
            Grounded = false;
            Normal = Vector2.up;
        }
    }
}
