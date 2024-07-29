using SiegeStorm.TowerSystem;
using SiegeStorm.WeaponSystem.ProjectileSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SiegeStorm.UnitSystem
{
    public class UnitSpawner : MonoBehaviour
    {
        [SerializeField] private Warrior _enemy;
        [SerializeField] private Tower _tower;

        [SerializeField] private Projectile projectile;
        [SerializeField] private Transform _targetPoint;

        private void Start()
        {
            _enemy.Init();

            AISystem aiSystem = _enemy.GetSystem<AISystem>();

            if(aiSystem != null)
            {
                aiSystem.Pursuit(_tower);
            }
            else
            {
                print("ERROR");
            }

            projectile.Launch(projectile.transform.position, _targetPoint.position, 15);
        }
    }
}