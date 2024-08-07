using System;
using UnityEngine;

namespace SiegeStorm.WeaponSystem.ProjectileSystem
{
    public class FlyingProjectile : Projectile
    {
        public event Action OnDestinationTarget;

        [SerializeReference] private FlyBehaviour _flyBehaviour;

        public override void Launch(Vector3 startPoint, Vector3 targetPoint, float speed)
        {
            base.Launch(startPoint, targetPoint, speed);

            if (_flyBehaviour == null)
            {
                Debug.LogError("Fly Behaviour is NULL");
                return;
            }

            _flyBehaviour.Init(transform, startPoint, targetPoint, speed);
            StartCoroutine(_flyBehaviour.StartFly());

            _flyBehaviour.OnDestinationTarget += OnFlyingEnded;
        }

        protected virtual void OnFlyingEnded()
        {
            _flyBehaviour.OnDestinationTarget -= OnFlyingEnded;

            OnDestinationTarget?.Invoke();

            Complete();
        }
    }
}