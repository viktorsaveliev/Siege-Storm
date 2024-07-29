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

            shootInfo.Target.Health.TakeDamage(Data.Damage);
        }
    }
}