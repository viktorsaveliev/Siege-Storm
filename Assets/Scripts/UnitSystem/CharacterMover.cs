using UnityEngine.AI;
using UnityEngine;

namespace SiegeStorm.UnitSystem
{
    public class CharacterMover
    {
        private readonly Unit _unit;
        private readonly NavMeshAgent _agent;
        private readonly AnimSystem _animSystem;

        private readonly float _attackDistance;
        private readonly float _rotationSpeed;

        public CharacterMover(Unit unit, NavMeshAgent agent, AnimSystem animSystem, float attackDistance, float rotationSpeed)
        {
            _unit = unit;
            _agent = agent;
            _animSystem = animSystem;

            _attackDistance = attackDistance;
            _rotationSpeed = rotationSpeed;
        }

        public bool HasReachedTarget()
        {
            if (_agent.hasPath)
            {
                if (IsWithinAttackDistance(_agent.steeringTarget))
                {
                    return true;
                }
                else
                {
                    Vector3 dir = (_agent.steeringTarget - _unit.transform.position).normalized;
                    _animSystem.Move();

                    _unit.transform.rotation = Quaternion.RotateTowards(_unit.transform.rotation, Quaternion.LookRotation(dir), _rotationSpeed * Time.fixedDeltaTime);
                    return false;
                }
            }
            else
            {
                _animSystem.Stop();
                return true;
            }
        }

        private bool IsWithinAttackDistance(Vector3 target)
        {
            return Vector3.Distance(_unit.transform.position, target) < _attackDistance;
        }
    }
}