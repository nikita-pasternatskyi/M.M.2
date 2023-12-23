using MykroFramework.Runtime.Objects.InteractingWithEnvironment.Raycast;
using MykroFramework.Runtime.Objects.Weaponry;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Ait
{
    public class AitGrappleBeam : Weapon
    {
        [SerializeField, Required] private PlayerAit _player;
        [SerializeReference] private EnvironmentRaycaster _raycaster;
        public Grappable CurrentGrappable;


        private void OnDrawGizmos()
        {
            _raycaster.DrawDebug();
        }

        public override void Fire()
        {
            if (_raycaster.FindHit(out EnvironmentQueryResult environmentQueryResult))
            {
                if (environmentQueryResult.Hit.collider.gameObject.TryGetComponent(out Grappable grappable))
                {
                    switch (grappable.Type)
                    {
                        case GrappableType.Normal:
                            _player.ZipToPoint(grappable.transform.position);
                            break;
                        case GrappableType.Swing:
                            _player.AttachToAnchorPoint(grappable.transform.position);
                            break;
                    }
                   
                }
            }
        }

        public override void EndFire()
        {
            _player.DetachFromAnchorPoint();
            _player.StopZip();
        }
    }
}
