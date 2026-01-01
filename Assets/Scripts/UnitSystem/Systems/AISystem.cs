using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SiegeStorm.UnitSystem
{
    public class AISystem : IUnitSystem
    {
        public IState CurrentState => _stateMachine.CurrentState;

        private readonly StateMachine _stateMachine = new();
        private readonly Unit _unit;

        private Coroutine _timer;

        public AISystem(Unit unit, AttackState pursuitState)
        {
            _unit = unit;

            InitStates(pursuitState);

            _timer = _unit.StartCoroutine(Timer());
        }

        #region StateMachine
        public void Pursuit(IDamageable target)
        {
            AttackState pursuitState = (AttackState)_stateMachine.GetState<AttackState>();
            pursuitState.Pursuit(target);

            _stateMachine.ChangeState(pursuitState);
        }

        public void Stop()
        {
            if (_timer != null)
            {
                _unit.StopCoroutine(_timer);
                _timer = null;
            }
        }

        private void InitStates(AttackState pursuitState)
        {
            _stateMachine.StateMap = new Dictionary<Type, IState>
            {
                [typeof(AttackState)] = pursuitState
            };
        }
        #endregion

        private IEnumerator Timer()
        {
            WaitForSeconds waitForSeconds = new(0.1f);
            yield return waitForSeconds;

            while (_stateMachine.CurrentState != null)
            {
                _stateMachine.CurrentState.Tick();
                yield return waitForSeconds;
            }
        }
    }
}