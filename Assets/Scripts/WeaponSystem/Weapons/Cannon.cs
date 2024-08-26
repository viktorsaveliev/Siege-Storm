using SiegeStorm.WeaponSystem.ProjectileSystem;
using UnityEngine;
using static SiegeStorm.WeaponSystem.PlayerWeapon;

namespace SiegeStorm.WeaponSystem
{
    public class Cannon : ProjectileWeapon
    {
        protected override void Shoot(ShootPhase shootPhase)
        {
            if (shootPhase != ShootPhase.Canceled) return;

            Vector3 targetPosition = GetTargetPosition();

            Projectile projectile = Projectiles.GetInactiveObject();
            projectile.Launch(StartProjectilePoint.position, targetPosition, Data.ProjectileSpeed);

            base.Shoot(shootPhase);
        }
    }
}