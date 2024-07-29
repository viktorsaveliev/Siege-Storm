using SiegeStorm.WeaponSystem.ProjectileSystem;
using UnityEngine;

namespace SiegeStorm.WeaponSystem
{
    public class SingleTargetWeapon : Weapon
    {
        protected override void Shoot(WeaponShootInfo shootInfo)
        {
            if (shootInfo.Target == null)
            {
                Debug.LogError("Target is null");
                return;
            }

            Projectile projectile = Projectiles.GetInactiveObject();
            projectile.Launch(StartProjectilePoint.position, shootInfo.Target.Position, Data.ProjectileSpeed);
        }
    }
}