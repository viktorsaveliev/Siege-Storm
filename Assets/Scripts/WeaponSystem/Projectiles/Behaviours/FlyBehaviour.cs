using System;
using System.Collections;
using UnityEngine;

namespace SiegeStorm
{
    public abstract class FlyBehaviour
    {
        public event Action OnDestinationTarget;

        protected Transform Target;

        protected Vector3 StartPosition;
        protected Vector3 TargetPosition;
        protected float Speed;
        protected float FlySpeed;
        protected float TimeElapsed;
        protected bool IsFlying;

        public void Init(Transform target, Vector3 startPoint, Vector3 targetPoint, float speed)
        {
            Target = target;

            StartPosition = startPoint;
            TargetPosition = targetPoint;
            Speed = speed;
            TimeElapsed = 0;
            IsFlying = true;
            FlySpeed = speed;
        }

        public abstract IEnumerator StartFly();

        protected virtual void EndFly()
        {
            OnDestinationTarget?.Invoke();
        }
    }
}