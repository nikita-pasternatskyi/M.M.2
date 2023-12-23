using MEC;
using MykroFramework.Runtime.Extensions;
using MykroFramework.Runtime.Objects.InteractingWithEnvironment.Raycast;
using System.Collections.Generic;
using UnityEngine;

namespace MM2
{
    public class MonsterFollower : MonoBehaviour
    {
        [SerializeField] private Transform[] _wayPoints;
        [SerializeReference] private EnvironmentRaycaster _raycaster;
        [SerializeField] private float _smoothness;
        [SerializeField] private float _followSpeed;
        [SerializeField] private Transform _target;
        [SerializeField] private float _startWaitTime;
        [SerializeField] private float _waitTime;
        private Vector3 _dampSpeed;

        private void OnDrawGizmos()
        {
            _raycaster.DrawDebug();
        }
        private void Awake()
        {
            Timing.RunCoroutine(Tick());
        }

        private IEnumerator<float> Tick()
        {
            yield return Timing.WaitForSeconds(_startWaitTime);
            while (enabled)
            {
                foreach (var item in _wayPoints)
                {
                    while (transform.position.GetDistance(item.position) > 0.5f)
                    {
                        if (_raycaster.FindHit(out EnvironmentQueryResult res))
                        {
                            if (res.Hit.transform.TryGetComponent(out OneWayDoor door))
                            {
                                door.TakeHit();
                                yield return Timing.WaitForSeconds(_waitTime);
                            }
                        }
                        transform.LookAt(item);
                        transform.position = Vector3.SmoothDamp(transform.position, item.position, ref _dampSpeed, _smoothness, _followSpeed, Time.deltaTime);
                        yield return Timing.WaitForOneFrame;
                    }

                }

            }

        }
    }
}
