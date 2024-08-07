using SiegeStorm.Destructibility;
using UnityEngine;
using Zenject;

namespace SiegeStorm.WeaponSystem.ProjectileSystem
{
    public class ClusterProjectile : FlyingProjectile, IExplodable
    {
        public ExplosionHandler ExplosionHandler { get; private set; }

        [SerializeField] private ParticleSystem _explode;
        [Inject] private readonly DiContainer _diContainer;

        private Projectile[] _secondaryProjectiles;
        private Rigidbody[] _secondaryProjectilesRigidbodies;

        private const float _initialForce = 3f;

        private void Awake()
        {
            ExplosionHandler = new(_explode, Data, TargetLayerMask);

            if (Data.TryGetModule(out ClusterExplodeModule clusterExplodeModule))
            {
                _secondaryProjectiles = new Projectile[clusterExplodeModule.ClustersCount];
                _secondaryProjectilesRigidbodies = new Rigidbody[clusterExplodeModule.ClustersCount];

                for (int i = 0; i < clusterExplodeModule.ClustersCount; i++)
                {
                    Vector3 randomPosition = GetRandomPositionAround(transform.position, clusterExplodeModule.Projectile.Data.Radius);
                    Projectile projectile = _diContainer.InstantiatePrefabForComponent<Projectile>(clusterExplodeModule.Projectile, randomPosition, Quaternion.identity, null);
                    
                    _secondaryProjectiles[i] = projectile;
                    _secondaryProjectilesRigidbodies[i] = projectile.GetComponent<Rigidbody>();
                    
                    gameObject.SetActive(false);
                }
            }
        }

        public void Explode()
        {
            ExplosionHandler.Explode(transform.position);
            CreateSecondaryExplosions();
        }

        protected override void OnFlyingEnded()
        {
            Explode();
            base.OnFlyingEnded();
        }

        private void CreateSecondaryExplosions()
        {
            foreach (var projectile in _secondaryProjectiles)
            {
                projectile.transform.position = GetRandomPositionAround(transform.position, 1f);
                projectile.Launch(projectile.transform.position + Vector3.up, Vector3.zero, 0);
            }

            foreach (var rb in _secondaryProjectilesRigidbodies)
            {
                Vector3 direction = (rb.transform.position - transform.position).normalized;
                rb.AddForce(direction * _initialForce, ForceMode.Impulse);
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