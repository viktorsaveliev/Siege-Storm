using SiegeStorm.TowerSystem;
using UnityEngine;

namespace SiegeStorm.UnitSystem
{
    public class UnitSpawner : MonoBehaviour
    {
        [SerializeField] private Warrior[] _enemies;
        [SerializeField] private Tower _tower;

        private void Start()
        {
            foreach (Warrior enemy in _enemies)
            {
                enemy.Init();

                AISystem aiSystem = enemy.GetSystem<AISystem>();

                if (aiSystem != null)
                {
                    aiSystem.Pursuit(_tower);
                }
                else
                {
                    print("ERROR");
                }
            }
        }
    }
}