using SiegeStorm.WeaponSystem.ProjectileSystem;
using UnityEngine;

namespace SiegeStorm.WeaponSystem
{
    public class AreaOfEffectWeapon : Weapon
    {
        protected override void Shoot(WeaponShootInfo shootInfo)
        {
            Vector3 targetPosition;
            if (shootInfo.TargetPosition == Vector3.zero)
            {
                if (shootInfo.Target == null)
                {
                    Debug.LogError("Target position is zero");
                    return;
                }

                targetPosition = shootInfo.Target.Position;
            }
            else
            {
                targetPosition = shootInfo.TargetPosition;
            }

            Projectile projectile = Projectiles.GetInactiveObject();
            projectile.Launch(StartProjectilePoint.position, targetPosition, Data.ProjectileSpeed);
        }
    }
}