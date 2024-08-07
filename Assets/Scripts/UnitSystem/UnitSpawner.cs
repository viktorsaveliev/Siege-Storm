using SiegeStorm.TowerSystem;
using UnityEngine;

namespace SiegeStorm.UnitSystem
{
    public class UnitSpawner : MonoBehaviour
    {
        [SerializeField] private Warrior _enemy;
        [SerializeField] private Tower _tower;

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
        }
    }
}