using Sirenix.OdinInspector;
using UnityEngine;

namespace SiegeStorm.WeaponSystem.ProjectileSystem
{
    [CreateAssetMenu(fileName = "(Projectile) ", menuName = "Weapon/ProjectileData")]
    public class ProjectileData : ScriptableObject
    {
        public int Damage => _damage;
        public bool IsRadiusDamage => _radiusDamage;
        public float Radius => _radius;

        [SerializeField, Range(1, 500)] private int _damage;

        [SerializeField] private bool _radiusDamage;
        [ShowIf(nameof(_radiusDamage), true), SerializeField, Range(0.5f, 10)]
        private float _radius;
    }
}