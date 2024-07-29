using UnityEngine;
using UnityEngine.AI;

namespace SiegeStorm.UnitSystem
{
    public abstract class PursuitState : UnitState
    {
        protected readonly NavMeshAgent Agent;
        protected AnimSystem AnimSystem;

        private readonly Warrior _warrior;
        private IDamageable _target;

        private float _attackDelay;
        private bool _isAttacking;

        public PursuitState(Unit unit) : base(unit)
        {
            _warrior = (Warrior)unit;
            Agent = unit.GetComponent<NavMeshAgent>();
        }

        public override void Enter()
        {
            AnimSystem ??= Unit.GetSystem<AnimSystem>();
            _warrior.OnAttack += Attack;
        }

        public override void Exit()
        {
            _warrior.OnAttack -= Attack;
        }

        public void Pursuit(IDamageable target)
        {
            _target = target;
            Agent.enabled = true;
            Agent.destination = _target.CurrentPosition;
        }

        protected bool TryAttack()
        {
            if (_attackDelay > Time.time || _isAttacking) return false;

            _isAttacking = true;

            AnimSystem.Stop();
            AnimSystem.Attack();

            _attackDelay = Time.time + _warrior.Data.DelayBetweenAttacks;
            return true;
        }

        private void Attack()
        {
            int damage = _warrior.Data.Damage;
            _target.Health.TakeDamage(damage);

            _isAttacking = false;
        }
    }
}