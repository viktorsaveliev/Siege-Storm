using UnityEngine;
using UnityEngine.AI;

namespace SiegeStorm.UnitSystem
{
    public abstract class AttackState : UnitState
    {
        protected bool IsAttacking { get; set; }

        protected readonly Warrior Warrior;
        protected readonly NavMeshAgent Agent;

        protected AnimSystem AnimSystem { get; private set; }
        protected IDamageable Target { get; private set; }

        private float _attackDelay;

        public AttackState(Unit unit) : base(unit)
        {
            Warrior = (Warrior)unit;
            Agent = unit.GetComponent<NavMeshAgent>();
        }

        public override void Enter()
        {
            AnimSystem ??= Unit.GetSystem<AnimSystem>();
            Warrior.OnAttack += Attack;
        }

        public override void Exit()
        {
            Warrior.OnAttack -= Attack;
        }

        public void Pursuit(IDamageable target)
        {
            Target = target;
            Agent.enabled = true;
            Agent.destination = Target.Position;
        }

        protected bool TryAttack()
        {
            if (_attackDelay > Time.time || IsAttacking) return false;

            IsAttacking = true;

            AnimSystem.Stop();
            AnimSystem.Attack();

            _attackDelay = Time.time + Warrior.Data.DelayBetweenAttacks;
            return true;
        }

        protected abstract void Attack();
    }
}