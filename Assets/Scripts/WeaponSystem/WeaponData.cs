using SiegeStorm.WeaponSystem.ProjectileSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SiegeStorm.WeaponSystem
{
    [CreateAssetMenu(fileName = "(WeaponData) ", menuName = "Weapon/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        public Sprite Icon => _icon;
        public string Name => _name;
        public string Description => _description;
        public int BulletsInMagazine => _bulletsInMagazine;
        public float ShootDelay => _shootDelay;
        public float ReloadDuration => _reloadDuration;
        public float ProjectileSpeed => _projectileSpeed;
        public Projectile Projectile => _projectilePrefab;
        public int Damage => _projectilePrefab.Data.Damage;

        [SerializeField, PreviewField(Alignment = ObjectFieldAlignment.Left)] 
        private Sprite _icon;
        
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField, Range(1, 30)] private int _bulletsInMagazine;
        [SerializeField, Range(0.05f, 2)] private float _shootDelay;
        [SerializeField, Range(1, 5)] private float _reloadDuration;
        [SerializeField, Range(1, 25)] private float _projectileSpeed;
        [SerializeField] private Projectile _projectilePrefab;
    }
}