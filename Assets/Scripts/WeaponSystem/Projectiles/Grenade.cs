using SiegeStorm.Destructibility;
using UnityEngine;

namespace SiegeStorm.WeaponSystem.ProjectileSystem
{
    public class Grenade : FlyingProjectile, IExplodable
    {
        public ExplosionHandler ExplosionHandler { get; private set; }

        [SerializeField] private ParticleSystem _explode;

        private void Awake()
        {
            ExplosionHandler = new(_explode, Data, TargetLayerMask);
        }

        public virtual void Explode() => ExplosionHandler.Explode(transform.position);

        protected override void OnFlyingEnded()
        {
            Explode();
            base.OnFlyingEnded();
        }
    }
}