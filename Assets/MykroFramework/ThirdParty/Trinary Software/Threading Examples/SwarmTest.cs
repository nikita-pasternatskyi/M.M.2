using System.Collections.Generic;
using UnityEngine;

namespace MEC
{
    public class SwarmTest : MonoBehaviour
    {
        private static readonly List<Transform> SiblingList = new List<Transform>();

        public Transform Target;

        public float MoveSpeed = 6f;
        public float BufferDistance = 1f;
        public float TurnRate = 1f;

        private void Awake()
        {
            SiblingList.Add(transform);
        }

        private void Start()
        {
            Timing.RunCoroutine(_RunSwarm().CancelWith(gameObject));
        }

        private IEnumerator<float> _RunSwarm()
        {
            Vector3 directionVector = Random.onUnitSphere;
            Vector3[] siblingPositions = new Vector3[SiblingList.Count];

            while(gameObject != null)
            {
                if (!gameObject.activeInHierarchy)
                {
                    yield return Timing.WaitForOneFrame;
                    continue;
                }

                Vector3 targetPos = Target != null ? Target.position : transform.position;
                Vector3 myPos = transform.position;
                float deltaTime = Timing.DeltaTime;
                Vector3 randomDirection = Random.onUnitSphere;

                for (int i = 0;i < siblingPositions.Length;i++)
                    siblingPositions[i] = SiblingList[i].position;

                yield return Threading.SwitchToExternalThread();

                Vector3 siblingAverage = Vector3.zero;
                for (int i = 0;i < siblingPositions.Length;i++)
                {
                    if ((siblingPositions[i] - myPos).sqrMagnitude > BufferDistance)
                        siblingAverage += siblingPositions[i] / siblingPositions.Length;
                    else
                        siblingAverage += (randomDirection * siblingPositions[i].magnitude) / siblingPositions.Length;
                }

                targetPos = (targetPos + siblingAverage) / 2;

                Vector3 directionToTarget = (targetPos - myPos).sqrMagnitude > 0.01f ? (targetPos - myPos).normalized : directionVector;
                directionVector = Vector3.RotateTowards(directionVector, directionToTarget, TurnRate * deltaTime, 1f);
                Quaternion directionQuaternion = Quaternion.Euler((directionVector * Mathf.Rad2Deg) + (Vector3.forward * 90f));

                myPos += directionVector * MoveSpeed * deltaTime;

                yield return Threading.SwitchBackToGUIThread;

                transform.position = myPos;
                transform.rotation = directionQuaternion;
            }
        }
    }
}
