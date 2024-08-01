using SiegeStorm.PoolSystem;
using SiegeStorm.WeaponSystem.ProjectileSystem;
using System;
using System.Collections;
using UnityEngine;

namespace SiegeStorm.WeaponSystem
{
    public abstract class Weapon : MonoBehaviour
    {
        public WeaponData Data => _data;
        public Transform StartProjectilePoint => _startProjectilePoint;
        public int CurrentBulletsCount => _currentBulletsCount;

        public event Action OnReloadStart;
        public event Action OnReloadEnd;
        public event Action OnFired;

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
            Projectiles.CreatePool();
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

            OnFired?.Invoke();
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
            OnReloadStart?.Invoke();

            _isReloading = true;

            yield return new WaitForSeconds(_data.ReloadDuration);

            _currentBulletsCount = _data.BulletsInMagazine;
            _isReloading = false;

            OnReloadEnd?.Invoke();
        }
    }
}