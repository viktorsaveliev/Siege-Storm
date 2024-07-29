using SiegeStorm.WeaponSystem.ProjectileSystem;
using UnityEngine;

namespace SiegeStorm.WeaponSystem
{
    public class AreaOfEffectWeapon : Weapon
    {
        protected override void Shoot(WeaponShootInfo shootInfo)
        {
            if (shootInfo.TargetPosition == Vector3.zero)
            {
                Debug.LogError("Target position is zero");
                return;
            }

            Projectile projectile = Projectiles.GetInactiveObject();
            projectile.Launch(StartProjectilePoint.position, shootInfo.TargetPosition, Data.ProjectileSpeed);
        }
    }
}