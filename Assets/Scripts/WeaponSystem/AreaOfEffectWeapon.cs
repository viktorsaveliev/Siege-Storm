using SiegeStorm.PoolSystem;
using SiegeStorm.WeaponSystem.ProjectileSystem;
using UnityEngine;

namespace SiegeStorm.WeaponSystem
{
    public class AreaOfEffectWeapon : Weapon
    {
        private ObjectPool<Projectile> _projectiles;

        public override void Init()
        {
            base.Init();
            _projectiles = new ObjectPool<Projectile>(Data.Projectile, transform, Data.BulletsInMagazine);
        }

        protected override void Shoot(WeaponShootInfo shootInfo)
        {
            if (shootInfo.TargetPosition == Vector3.zero)
            {
                Debug.LogError("Target position is zero");
                return;
            }

            Projectile projectile = _projectiles.GetInactiveObject();
            projectile.Shoot(StartProjectilePoint.position, shootInfo.TargetPosition);
        }
    }
}