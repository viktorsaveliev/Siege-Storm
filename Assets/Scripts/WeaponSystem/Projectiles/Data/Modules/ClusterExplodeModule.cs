using UnityEngine;

namespace SiegeStorm.WeaponSystem.ProjectileSystem
{
    public class ClusterExplodeModule : ProjectileModule
    {
        public Projectile Projectile => _secondaryProjectile;
        public int ClustersCount => _clustersCount;

        [SerializeField] private Projectile _secondaryProjectile;
        [SerializeField, Range(1, 10)] private int _clustersCount;
    }
}