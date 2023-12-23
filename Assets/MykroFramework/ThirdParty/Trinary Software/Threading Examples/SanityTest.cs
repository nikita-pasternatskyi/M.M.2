using System.Collections.Generic;
using UnityEngine;

namespace MEC
{
    public class SanityTest : MonoBehaviour
    {
        private CoroutineHandle handle;

        public float min = -20f;
        public float max = 20f;
        public float timeToReach = 6f;

        private void OnEnable()
        {
            handle = Timing.RunCoroutine(_MoveBackAndForth());
        }

        private void OnDisable()
        {
            Timing.KillCoroutines(handle);
        }

        private IEnumerator<float> _MoveBackAndForth()
        {
            while(gameObject != null)
            {
                Vector3 start = transform.position;
                float startTime = Timing.LocalTime;

                while(transform.position.x < max)
                {
                    transform.position = Vector3.Lerp(start, new Vector3(max, transform.position.y, transform.position.z),
                        (Timing.LocalTime - startTime) / timeToReach);
                    yield return Timing.WaitForOneFrame;
                }

                start = transform.position;
                startTime = Timing.LocalTime;
                while(transform.position.x > min)
                {
                    transform.position = Vector3.Lerp(start, new Vector3(min, transform.position.y, transform.position.z),
                        (Timing.LocalTime - startTime) / timeToReach);
                    yield return Timing.WaitForOneFrame;
                }
            }
        }
    }
}
