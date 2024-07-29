using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace SiegeStorm.UnitSystem
{
    [CreateAssetMenu(fileName = "(WarriorData) ", menuName = "Units/WarriorData")]
    public class WarriorData : UnitData
    {
        public int Health => _health;
        public int Damage => _damage;
        public float DelayBetweenAttacks => _delayBetweenAttacks;
        public int RewardForKill => _rewardForKill;

        [Title("Combat")]
        [SerializeField, Range(1, 1000)] private int _health;
        [SerializeField, Range(1, 500)] private int _damage;
        [SerializeField, Range(0.5f, 10f)] private float _delayBetweenAttacks;
        [SerializeField, Range(1, 100)] private int _rewardForKill;
    }
}
