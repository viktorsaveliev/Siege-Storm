
using UnityEngine;

namespace SiegeStorm
{
    public interface IDamageable
    {
        public HealthSystem Health { get; }
        public Vector3 CurrentPosition { get; }
    }
}