using System;

namespace SiegeStorm
{
    public class HealthSystem
    {
        public int Health => _health;
        public int MaxHealth => _maxHealth;

        public event Action<int> OnTakedDamage;
        public event Action OnDead;

        private int _health;
        private int _maxHealth;

        public HealthSystem(int maxHealth)
        {
            _maxHealth = maxHealth;
            _health = maxHealth;
        }

        public void SetHealth(int health)
        {
            if (health < 1 || health > _maxHealth)
            {
                throw new ArgumentException();
            }

            _health = health;
        }

        public void SetMaxHealth(int health)
        {
            if (health < 1)
            {
                throw new ArgumentException();
            }

            _maxHealth = health;
        }

        public void TakeDamage(int damage)
        {
            if (_health <= 0) return;

            _health -= damage;

            if (_health <= 0)
            {
                OnDead?.Invoke();
            }

            OnTakedDamage?.Invoke(damage);
        }
    }
}