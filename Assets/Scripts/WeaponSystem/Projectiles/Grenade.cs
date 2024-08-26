using SiegeStorm.Destructibility;
using UnityEngine;

namespace SiegeStorm.WeaponSystem.ProjectileSystem
{
    public class Grenade : FlyingProjectile, IExplodable
    {
        public Explosion Explosion { get; private set; }

        [SerializeField] private ParticleSystem _explode;

        private void Awake()
        {
            Explosion = new(_explode, TargetLayerMask);
            Explosion.Init();
        }

        public virtual void Explode() => Explosion.Explode(transform.position, Data.Radius, Data.Damage);

        protected override void OnFlyingEnded()
        {
            Explode();
            base.OnFlyingEnded();
        }
    }
}