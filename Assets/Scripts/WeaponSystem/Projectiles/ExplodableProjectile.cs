using UnityEngine;

namespace SiegeStorm.WeaponSystem.ProjectileSystem
{
    public class ExplodableProjectile : Projectile
    {
        private readonly Collider[] _targets = new Collider[10];

        protected override void OnFlyingEnded()
        {
            base.OnFlyingEnded();
            Explode();
        }

        private void Explode()
        {
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, Data.Radius, _targets, TargetLayerMask);

            for (int i = 0; i < numColliders; i++)
            {
                IDamageable target = _targets[i].GetComponent<IDamageable>();
                target.Health.TakeDamage(Data.Damage);
            }
        }
    }
}