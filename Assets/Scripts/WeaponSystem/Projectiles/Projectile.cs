using UnityEngine;

namespace SiegeStorm.WeaponSystem.ProjectileSystem
{
    public abstract class Projectile : MonoBehaviour
    {
        public ProjectileData Data => _data;

        [SerializeField] private ProjectileData _data;

        public virtual void Shoot(Vector3 startPoint, Vector3 targetPoint)
        {

        }
    }
}