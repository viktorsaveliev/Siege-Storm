using SiegeStorm.PlayerController;
using System;
using System.Collections;
using UnityEngine;
using Zenject;
using static SiegeStorm.WeaponSystem.PlayerWeapon;

namespace SiegeStorm.WeaponSystem
{
    public abstract class Weapon : MonoBehaviour
    {
        public WeaponData Data => _data;
        public int CurrentBulletsCount { get; protected set; }

        public event Action OnReloadStart;
        public event Action OnReloadEnd;
        public event Action OnFired;

        [SerializeField] private WeaponData _data;

        private IInteractHandler _interactHandler;
        private bool _isReloading;
        private float _lastShootTime;

        public virtual void Init(DiContainer diContainer, IInteractHandler interactHandler)
        {
            _interactHandler = interactHandler;

            CurrentBulletsCount = _data.BulletsInMagazine;
            _isReloading = false;
            _lastShootTime = 0;
        }

        public bool TryShoot(ShootPhase shootStep)
        {
            if (CanShoot())
            {
                Shoot(shootStep);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reload()
        {
            if (!_isReloading)
            {
                StartCoroutine(ReloadCoroutine());
            }
        }

        protected Vector3 GetTargetPosition()
        {
            return _interactHandler.GetTargetPosition();
        }

        protected virtual void Shoot(ShootPhase shootStep)
        {
            OnShoot();
        }

        protected virtual void OnShoot()
        {
            CurrentBulletsCount--;
            _lastShootTime = Time.time;

            if (CurrentBulletsCount <= 0)
            {
                Reload();
            }

            OnFired?.Invoke();
        }

        private bool CanShoot()
        {
            return !_isReloading && CurrentBulletsCount > 0 && Time.time - _lastShootTime >= Data.ShootDelay;
        }

        private IEnumerator ReloadCoroutine()
        {
            OnReloadStart?.Invoke();

            _isReloading = true;

            yield return new WaitForSeconds(_data.ReloadDuration);

            CurrentBulletsCount = _data.BulletsInMagazine;
            _isReloading = false;

            OnReloadEnd?.Invoke();
        }
    }
}