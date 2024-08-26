using SiegeStorm.PlayerController;
using SiegeStorm.PoolSystem;
using SiegeStorm.WeaponSystem.ProjectileSystem;
using UnityEngine;
using Zenject;

namespace SiegeStorm.WeaponSystem
{
    public class ProjectileWeapon : Weapon
    {
        public Transform StartProjectilePoint => _startProjectilePoint;

        [SerializeField] private Transform _startProjectilePoint;

        protected DIObjectPool<Projectile> Projectiles;

        public override void Init(DiContainer diContainer, IInteractHandler interactHandler)
        {
            base.Init(diContainer, interactHandler);

            Projectiles = new DIObjectPool<Projectile>(Data.Projectile, diContainer, transform, Data.BulletsInMagazine);
            Projectiles.CreatePool();
        }
    }
}