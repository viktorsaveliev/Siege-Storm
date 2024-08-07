using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace SiegeStorm.WeaponSystem.ProjectileSystem
{
    [CreateAssetMenu(fileName = "(Projectile) ", menuName = "Weapon/ProjectileData")]
    public class ProjectileData : ScriptableObject
    {
        public int Damage => _damage;
        public bool IsRadiusDamage => _radiusDamage;
        public float Radius => _radius;

        [Title("BaseData")]
        [SerializeField, Range(1, 500)] private int _damage;

        [SerializeField] private bool _radiusDamage;
        [ShowIf(nameof(_radiusDamage), true), SerializeField, Range(0.5f, 10)]
        private float _radius;

        [Title("Additional")]
        [SerializeReference] private ProjectileModule[] _modules;

        public bool TryGetModule<T>(out T targetModule) where T : ProjectileModule
        {
            targetModule = null;

            foreach (ProjectileModule projectileModule in _modules)
            {
                if (projectileModule is T module)
                {
                    targetModule = module;
                    return true;
                }
            }

            return false;
        }
    }
}
