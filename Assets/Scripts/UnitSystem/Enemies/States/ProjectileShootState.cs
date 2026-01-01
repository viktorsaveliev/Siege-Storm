using SiegeStorm.PoolSystem;
using SiegeStorm.WeaponSystem.ProjectileSystem;

namespace SiegeStorm.UnitSystem
{
    public class ProjectileShootState : AttackState
    {
        private readonly ObjectPool<Projectile> _projectilePool;
        private readonly float _attackDistance;
        private readonly float _projectileSpeed;
        private CharacterMover _mover;

        public ProjectileShootState(Unit unit, Projectile projectilePrefab, int projectileCapacity, float projectileSpeed, float attackDistance) : base(unit)
        {
            _attackDistance = attackDistance;
            _projectileSpeed = projectileSpeed;
            _projectilePool = new(projectilePrefab, unit.transform, projectileCapacity);
            _projectilePool.CreatePool();
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
            Projectile projectile = _projectilePool.GetInactiveObject();
            projectile.Launch(Unit.transform.position, Target.Position, _projectileSpeed);

            IsAttacking = false;
        }
    }
}