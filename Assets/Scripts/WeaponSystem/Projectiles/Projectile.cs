using UnityEngine;
using Sirenix.OdinInspector;
using System;
using SiegeStorm.FeedbackSystem;

namespace SiegeStorm.WeaponSystem.ProjectileSystem
{
    public abstract class Projectile : MonoBehaviour
    {
        public event Action OnCompleted;

        public ProjectileData Data => _data;

        [SerializeField] private ProjectileData _data;
        [SerializeField, ReadOnly] protected LayerMask TargetLayerMask;
        [SerializeReference] private IActionFeedback[] _feedback;

        private void OnValidate()
        {
            TargetLayerMask = LayerMask.GetMask("Character");
        }

        public virtual void Launch(Vector3 startPoint, Vector3 targetPoint, float speed)
        {
            gameObject.SetActive(true);
            transform.SetPositionAndRotation(startPoint, Quaternion.identity);
        }

        public void Complete()
        {
            OnCompleted?.Invoke();

            foreach (IActionFeedback feedback in _feedback)
            {
                feedback.Active();
            }

            gameObject.SetActive(false);
        }
    }
}