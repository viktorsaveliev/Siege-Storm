using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace SiegeStorm.WeaponSystem.ProjectileSystem
{
    public abstract class Projectile : MonoBehaviour
    {
        public event Action OnDestinationTarget;

        public ProjectileData Data => _data;

        [SerializeField] private ProjectileData _data;
        [SerializeField, ReadOnly] protected LayerMask TargetLayerMask;

        private void OnValidate()
        {
            TargetLayerMask = LayerMask.GetMask("Character");
        }

        public virtual void Launch(Vector3 startPoint, Vector3 targetPoint, float speed)
        {
            transform.position = startPoint;
            transform.LookAt(targetPoint);

            FlyBehaviour flyBehaviour = Data.FlyBehaviour;
            flyBehaviour.Init(transform, startPoint, targetPoint, speed);
            StartCoroutine(flyBehaviour.StartFly());

            flyBehaviour.OnDestinationTarget += OnFlyingEnded;
        }

        protected virtual void OnFlyingEnded()
        {
            Data.FlyBehaviour.OnDestinationTarget -= OnFlyingEnded;
            OnDestinationTarget?.Invoke();
        }
    }
}