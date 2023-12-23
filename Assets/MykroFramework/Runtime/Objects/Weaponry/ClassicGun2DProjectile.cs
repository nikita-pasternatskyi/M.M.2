using System;
using UnityEngine;
using UnityEngine.Events;

namespace MykroFramework.Runtime.Objects.Weaponry
{
    public class ClassicGun2DProjectile : Projectile
    {
        [SerializeField] private float _flightSpeed;
        [SerializeField] private bool _destroyIfOffscreen;
        [SerializeField] private int _damage;
        [SerializeReference] private InteractingWithEnvironment.Scan.EnvironmentScanner2D _scanner;
        public override event Action<Projectile> Destroyed;

        private void OnDrawGizmos()
        {
            _scanner.DrawDebug();
        }

        private void OnBecameInvisible()
        {
            if (_destroyIfOffscreen)
                Destroyed?.Invoke(this);
        }

        private void FixedUpdate()
        {
            transform.position += transform.right * _flightSpeed * Time.fixedDeltaTime;
            var hits = _scanner.GetHits(transform.position);
            foreach (var hit in hits)
            {
                if (hit.isTrigger == false)
                    continue;
                if (hit.gameObject.TryGetComponent(out IDamageable damageable))
                {
                    if (damageable.CurrentHealth - _damage >= _damage)
                    {
                        Destroyed?.Invoke(this);
                    }
                    damageable.TakeDamage(_damage, transform.right);
                }
                else
                {
                    Destroyed?.Invoke(this);
                }
            }
        }
    }
}
