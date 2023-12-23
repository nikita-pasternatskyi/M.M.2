using MykroFramework.Runtime.Objects.InteractingWithEnvironment.Raycast;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Player.Web
{
    public class SwingPointFinder : MonoBehaviour
    {
        [SerializeField] private CenterScreenRay _centerScreenCheck;
        [SerializeField] private ConeCaster _coneScanner;
        [field: SerializeField, ReadOnly] public Vector3 LastSwingHit { get; private set; }

        private void Awake()
        {
            _coneScanner.Init();
        }

        private void OnDrawGizmos()
        {
            _coneScanner.DrawDebug();
        }

        public bool CanSwing(out RaycastHit webHit)
        {
            webHit = default;
            if (_coneScanner.FindHit(out EnvironmentQueryResult environmentQueryResult))
            {
                webHit = environmentQueryResult.Hit;
                LastSwingHit = environmentQueryResult.Hit.point;
                return true;
            }
            return false;
        }

        public bool CanSwingMouseCenter(out RaycastHit webHit)
        {
            webHit = default;
            var res = (_centerScreenCheck.FindHit(out EnvironmentQueryResult environmentQueryResult));
            Debug.Log(res);
            if(res)
            {
                webHit = environmentQueryResult.Hit;
                LastSwingHit = environmentQueryResult.Hit.point;
                return true;
            }
            return false;
        }
    }
}
