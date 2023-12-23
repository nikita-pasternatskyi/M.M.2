using System.Collections.Generic;
using UnityEngine;

namespace MykroFramework.Runtime.Objects.Weaponry
{
    public class ClassicGun2D : Weapon
    {
        [SerializeField] private ClassicGun2DProjectile _normalShot;
        [SerializeField] private ClassicGun2DProjectile _middleChargeShot;
        [SerializeField] private ClassicGun2DProjectile _maxChargeShot;
        [SerializeField] private int _maxProjectileCount;
        [SerializeField] private float _chargeAwakeTime;
        [SerializeField] private float _timeUntilMaxCharge;

        private List<GameObject> _spawnedProjectiles;
        private float _timer;

        private void Awake()
        {
            if(_maxProjectileCount > 0)
            _spawnedProjectiles = new List<GameObject>(_maxProjectileCount);
        }

        public override void HoldingFire()
        {
            _timer = Mathf.Clamp(_timer + Time.deltaTime, 0, _timeUntilMaxCharge + _chargeAwakeTime);
        }

        public override void EndFire()
        {
            if (_timer >= _chargeAwakeTime + _timeUntilMaxCharge)
            {
                ShootAProjectile(_maxChargeShot);
            }
            else if (_timer >= _chargeAwakeTime)
            {
                ShootAProjectile(_middleChargeShot);
            }
        }

        private void ShootAProjectile(Projectile projectile)
        {
            if (_maxProjectileCount > 0)
                if (_spawnedProjectiles.Count >= _maxProjectileCount)
                    return;
            _timer = 0;
            var obj = Instantiate(projectile, transform.position, transform.rotation, null);
            obj.Destroyed += OnProjectileDestroyed;

            if (_maxProjectileCount < 0)
                return;
            _spawnedProjectiles.Add(obj.gameObject);
        }

        public override void Fire()
        {
            ShootAProjectile(_normalShot);
        }

        private void OnProjectileDestroyed(Projectile obj)
        {
            obj.Destroyed -= OnProjectileDestroyed;
            Destroy(obj.gameObject);
            if (_maxProjectileCount > 0)
                _spawnedProjectiles.Remove(obj.gameObject);
        }
    } 
}
