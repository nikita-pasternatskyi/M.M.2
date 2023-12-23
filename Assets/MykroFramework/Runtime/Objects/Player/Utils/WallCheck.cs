using MykroFramework.Runtime.Objects.Player.Input;
using MykroFramework.Runtime.Objects.InteractingWithEnvironment.Raycast;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player.Utils
{
    public class WallCheck : MonoBehaviour
    {
        [SerializeField] public Transform WallClimbRotation;
        [SerializeField] private float _rayLength;
        [SerializeField] private LayerMask _whatIsWall;
        [SerializeField] private IMovementInputProvider _playerInput;
        [SerializeReference] private EnvironmentRaycaster _wallProximityCheck;
        [SerializeReference] private EnvironmentRaycaster _isOnWallCheck;
        private IMovementInputProvider _input;

        [ReadOnly] public Vector3 DynamicWallForward { get; private set; }
        [ReadOnly] public Vector3 DynamicWallRight { get; private set; }
        [ReadOnly] public Vector3 WallForward { get; private set; }
        [ReadOnly] public Vector3 WallRight { get; private set; }
        [ReadOnly] public Vector3 WallNormal { get; private set; }

        private Vector3[] _directions = new Vector3[5]
           {
                Vector3.forward, Vector3.right, -Vector3.right, -Vector3.forward, Vector3.up,
           };

        [Button]
        private void RebuildDirections()
        {
            _directions = new Vector3[5]
            {
                Vector3.forward, Vector3.right, -Vector3.right, -Vector3.forward, Vector3.up,
            };
        }

        public bool IsWallInFront(out Vector3 normal, out Vector3 point)
        {
            normal = default;
            point = default;
            if (_wallProximityCheck.FindHit(out EnvironmentQueryResult query))
            {
                normal = query.Hit.normal;
                point = query.Hit.point;
                return true;
            }
            return false;
        }

        private void Awake()
        {
            //_input = GetComponent<PlayerInput>();
            RebuildDirections();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            foreach (var dir in _directions)
            {
                Gizmos.DrawRay(transform.position, dir * _rayLength);
            }

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, DynamicWallForward * 3);
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, WallNormal * 3);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, DynamicWallRight * 3);

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, WallForward * 15);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, WallRight * 7);

            _wallProximityCheck?.DrawDebug();
            _isOnWallCheck?.DrawDebug();
        }

        private void Update()
        {
            //DynamicWallRight = Vector3.Cross(WallNormal, _playerInput.CameraForward).normalized;

            var crossRight = -Vector3.Cross(WallNormal, DynamicWallRight);
            DynamicWallForward = crossRight;
            WallNormal.Normalize();
        }

        public bool HasWall(out Vector3 wallNormal, WallCheckSide checkSide, out Vector3 point, bool local = false, bool skipInput = false)
        {
            wallNormal = Vector3.zero;
            point = Vector3.zero;
            var rc = Physics.SphereCast(transform.position, 0.5f, _input.RelativeInput, out RaycastHit hit, _rayLength, _whatIsWall);
            var dir = _directions[(int)checkSide];
            if (rc == false || skipInput)
            {
                if (local == true)
                    dir = WallClimbRotation.TransformDirection(dir);
                rc = Physics.SphereCast(transform.position, 0.5f, dir, out hit, _rayLength, _whatIsWall);
            }
            if (rc)
            {
                wallNormal = hit.normal;
                point = hit.point;
                return true;
            }
            return false;
        }

        public bool HasWall(out Vector3 wallNormal, out Vector3 point)
        {
            wallNormal = Vector3.zero;
            point = Vector3.zero;
            var rc = Physics.SphereCast(transform.position, 0.5f, _input.RelativeInput, out RaycastHit hit, _rayLength, _whatIsWall);
            if (rc == false)
            {
                foreach (var direction in _directions)
                {
                    rc = Physics.SphereCast(transform.position, 0.5f, direction, out hit, _rayLength, _whatIsWall);
                    if (rc)
                        break;
                }
            }
            if (rc)
            {
                wallNormal = hit.normal;
                point = hit.point;
                return true;
            }
            return false;
        }

        public bool HasWallUnderneath(out Vector3 wallNormal, out Vector3 point)
        {
            wallNormal = Vector3.zero;
            point = Vector3.zero;
            if (_isOnWallCheck.FindHit(out EnvironmentQueryResult result))
            {
                wallNormal = result.Hit.normal;
                point = result.Hit.point;
                return true;
            }
            return false;
        }

        public void ChangeWallNormal(Vector3 normal)
        {
            WallNormal = normal.normalized;
            var sign = _playerInput.AbsoluteInput.y < 0 ? -1 : 1;
            WallForward = -Vector3.Cross(WallNormal, WallClimbRotation.right) * sign;
            WallRight = WallClimbRotation.right;
        }
    }
}