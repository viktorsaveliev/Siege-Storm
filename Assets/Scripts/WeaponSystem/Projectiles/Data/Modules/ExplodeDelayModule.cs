using UnityEngine;

namespace SiegeStorm.WeaponSystem.ProjectileSystem
{
    public class ExplodeDelayModule : ProjectileModule
    {
        public float Delay => _delay;

        [SerializeField, Range(1, 5)] private float _delay = 1f;
    }
}