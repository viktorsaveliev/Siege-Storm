
namespace SiegeStorm.UnitSystem
{
    public class CloseCombatState : AttackState
    {
        private readonly float _attackDistance;
        private CharacterMover _mover;

        public CloseCombatState(Unit unit, float attackDistance) : base(unit)
        {
            _attackDistance = attackDistance;
        }

        public override void Enter()
        {
            base.Enter();
            float rotationSpeed = 200f;
            _mover = new CharacterMover(Unit, Agent, AnimSystem, _attackDistance, rotationSpeed);
        }

        public override void Tick()
        {
            if (_mover.HasReachedTarget())
            {
                TryAttack();
            }
        }

        protected override void Attack()
        {
            int damage = Warrior.Data.Damage;
            Target.Health.TakeDamage(damage);

            IsAttacking = false;
        }
    }
}