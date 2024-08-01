using SiegeStorm.WeaponSystem.ProjectileSystem;
using UnityEngine;

namespace SiegeStorm.WeaponSystem
{
    public class SingleTargetWeapon : Weapon
    {
        protected override void Shoot(WeaponShootInfo shootInfo)
        {
            Vector3 targetPosition;

            if (shootInfo.Target == null)
            {
                if (shootInfo.TargetPosition == Vector3.zero)
                {
                    Debug.LogError("Target position is zero");
                    return;
                }

                targetPosition = shootInfo.TargetPosition;
            }
            else
            {
                targetPosition = shootInfo.Target.Position;
            }

            Projectile projectile = Projectiles.GetInactiveObject();
            projectile.Launch(StartProjectilePoint.position, targetPosition, Data.ProjectileSpeed);
        }
    }
}