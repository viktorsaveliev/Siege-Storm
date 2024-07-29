using UnityEngine;

namespace SiegeStorm.TowerSystem
{
    public class Tower : MonoBehaviour, IDamageable
    {
        public HealthSystem Health { get; private set; }
        public Vector3 Position => transform.position;

        private void Awake()
        {
            Health = new(500);
        }

        private void OnEnable()
        {
            Health.OnTakedDamage += OnTakeDamage;
        }

        private void OnDisable()
        {
            Health.OnTakedDamage -= OnTakeDamage;
        }

        private void OnTakeDamage(int damage)
        {
            print($"Taked damage {damage}");
        }
    }
}