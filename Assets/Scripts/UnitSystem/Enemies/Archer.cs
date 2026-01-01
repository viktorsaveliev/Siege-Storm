using SiegeStorm.WeaponSystem.ProjectileSystem;
using UnityEngine;

namespace SiegeStorm.UnitSystem
{
    public class Archer : Warrior
    {
        [SerializeField] private Projectile _projectilePrefab;

        protected override void InitAI()
        {
            float attackDistance = 15f;
            AISystem ai = new(this, new ProjectileShootState(this, _projectilePrefab, 2, 10, attackDistance));
            AddSystem(ai);
        }
    }
}