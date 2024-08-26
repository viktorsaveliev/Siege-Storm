using SiegeStorm.Destructibility;
using SiegeStorm.PoolSystem;
using UnityEngine;
using Zenject;

namespace SiegeStorm.WeaponSystem.ProjectileSystem
{
    public class ClusterProjectile : FlyingProjectile, IExplodable
    {
        public Explosion Explosion { get; private set; }

        [SerializeField] private ParticleSystem _explode;
        [Inject] private readonly DiContainer _diContainer;

        private DIObjectPool<Projectile> _projectiles;

        private const float _initialForce = 3f;

        private void Awake()
        {
            Explosion = new(_explode, TargetLayerMask);
            Explosion.Init();

            CreatePool();
        }

        public void Explode()
        {
            Explosion.Explode(transform.position, Data.Radius, Data.Damage);
            SpawnSecondaryProjectiles();
        }

        protected override void OnFlyingEnded()
        {
            Explode();
            base.OnFlyingEnded();
        }

        private void CreatePool()
        {
            if (Data.TryGetModule(out ClusterExplodeModule clusterExplodeModule))
            {
                _projectiles = new(clusterExplodeModule.Projectile, _diContainer, null, clusterExplodeModule.ClustersCount);
                _projectiles.CreatePool();
            }
        }

        private void SpawnSecondaryProjectiles()
        {
            foreach (var projectile in _projectiles.PoolList)
            {
                projectile.transform.position = GetRandomPositionAround(transform.position, 1f);
                projectile.Launch(projectile.transform.position + Vector3.up, transform.position, _initialForce);
            }
        }

        private Vector3 GetRandomPositionAround(Vector3 center, float radius)
        {
            float angle = Random.Range(0f, 360f);
            float distance = Random.Range(0f, radius);
            return center + new Vector3(Mathf.Cos(angle) * distance, 0f, Mathf.Sin(angle) * distance);
        }
    }
}