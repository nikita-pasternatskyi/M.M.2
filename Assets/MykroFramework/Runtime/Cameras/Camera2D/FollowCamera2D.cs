using MEC;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MykroFramework.Runtime.Cameras.Camera2D
{
    public struct Limits2D
    {
        public Vector2 MinXY;
        public Vector2 MaxXY;
    }

    public class FollowCamera2D : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _defaultBoundsChangeTime;
        [SerializeField] private Transform _startX;
        [SerializeField] private Transform _endX;
        public Transform Target;
        private CoroutineHandle _changeBoundsHandle;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_startX.position, 0.25f);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_endX.position, 0.25f);
        }
#endif

        private void Update()
        {
            var height = 2 * _camera.orthographicSize;
            var halfWidth = height * _camera.aspect * 0.5f;

            if (Target == null)
                return;
            var targetX = Mathf.Clamp(Target.position.x, _startX.position.x + halfWidth, _endX.position.x - halfWidth);
            if (transform.position.x > targetX)
                return;
            transform.position = new Vector3(targetX, transform.position.y, transform.position.z);

        }
    }
}
