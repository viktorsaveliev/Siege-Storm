using UnityEngine;

namespace SiegeStorm.WeaponSystem.ProjectileSystem
{
    public class Bullet : Projectile
    {
        private void OnCollisionEnter(Collision collision)
        {
            if(Layer.IsInLayerMask(collision.gameObject.layer, TargetLayerMask))
            {
                if (collision.gameObject.TryGetComponent(out IDamageable target))
                {
                    target.Health.TakeDamage(Data.Damage);
                }
            }
        }
    }
}