using UnityEngine;

namespace SiegeStorm.UnitSystem
{
    public class CloseCombatPursuitState : PursuitState
    {
        private readonly float _attackRadius = 2f;

        public CloseCombatPursuitState(Unit unit) : base(unit)
        {
        }

        public override void Tick()
        {
            if (Agent.hasPath)
            {
                if (Vector3.Distance(Unit.transform.position, Agent.destination) < _attackRadius)
                {
                    TryAttack();
                }
                else
                {
                    Vector3 dir = (Agent.steeringTarget - Unit.transform.position).normalized;
                    AnimSystem.Move();

                    Unit.transform.rotation = Quaternion.RotateTowards(Unit.transform.rotation, Quaternion.LookRotation(dir), 200 * Time.fixedDeltaTime);
                }
            }
            else
            {
                AnimSystem.Stop();
            }
        }
    }
}