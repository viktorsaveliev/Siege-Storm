using SiegeStorm.PoolSystem;
using SiegeStorm.WeaponSystem.ProjectileSystem;
using System.Collections;
using System.Linq.Expressions;
using UnityEngine;

namespace SiegeStorm.WeaponSystem
{
    public abstract class Weapon : MonoBehaviour
    {
        public WeaponData Data => _data;
        public Transform StartProjectilePoint => _startProjectilePoint;

        [SerializeField] private WeaponData _data;
        [SerializeField] private Transform _startProjectilePoint;

        protected ObjectPool<Projectile> Projectiles;

        private int _currentBulletsCount;
        private bool _isReloading;
        private float _lastShootTime;

        public virtual void Init()
        {
            _currentBulletsCount = _data.BulletsInMagazine;
            _isReloading = false;
            _lastShootTime = 0;

            Projectiles = new ObjectPool<Projectile>(Data.Projectile, transform, Data.BulletsInMagazine);
        }

        public bool TryShoot(WeaponShootInfo shootInfo)
        {
            if (CanShoot())
            {
                Shoot(shootInfo);
                OnShoot();
                return true;
            }
            else
            {
                return false;
            }
        }

        protected abstract void Shoot(WeaponShootInfo shootInfo);

        protected virtual void OnShoot()
        {
            _currentBulletsCount--;
            _lastShootTime = Time.time;

            if (_currentBulletsCount <= 0)
            {
                Reload();
            }
        }

        public void Reload()
        {
            if (!_isReloading)
            {
                StartCoroutine(ReloadCoroutine());
            }
        }

        private bool CanShoot()
        {
            return !_isReloading && _currentBulletsCount > 0 && Time.time - _lastShootTime >= Data.ShootDelay;
        }

        private IEnumerator ReloadCoroutine()
        {
            _isReloading = true;

            yield return new WaitForSeconds(_data.ReloadDuration);

            _currentBulletsCount = _data.BulletsInMagazine;
            _isReloading = false;
        }
    }
}