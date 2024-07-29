using System;
using UnityEngine;

namespace SiegeStorm.UnitSystem
{
    public abstract class Warrior : Unit, IDamageable
    {
        public HealthSystem Health { get; private set; }
        public Vector3 CurrentPosition => transform.position;
        public WarriorData Data => _data;

        public event Action OnAttack;

        [SerializeField] private WarriorData _data;

        private void OnDestroy()
        {
            Health.OnDead -= OnDeath;
        }

        public override void Init()
        {
            Health = new(Data.Health);
            Health.OnDead += OnDeath;

            AnimSystem anim = new(this);
            AddSystem(anim);

            InitAI();
        }

        protected abstract void InitAI();

        protected virtual void OnDeath()
        {
            AISystem ai = GetSystem<AISystem>();
            AnimSystem anim = GetSystem<AnimSystem>();

            ai.Stop();
            anim.Death();
        }

        #region AnimEvents
        private void Attack()
        {
            OnAttack?.Invoke();
        }
        #endregion
    }
}