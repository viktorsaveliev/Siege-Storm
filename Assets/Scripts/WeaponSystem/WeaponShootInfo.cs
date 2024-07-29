using UnityEngine;

namespace SiegeStorm.WeaponSystem
{
    public struct WeaponShootInfo
    {
        public IDamageable Target { get; private set; }
        public Vector3 TargetPosition { get; private set; }

        public WeaponShootInfo(IDamageable target)
        {
            Target = target;
            TargetPosition = Vector3.zero;
        }

        public WeaponShootInfo(Vector3 targetPosition)
        {
            Target = null;
            TargetPosition = targetPosition;
        }
    }
}