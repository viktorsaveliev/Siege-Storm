using SiegeStorm.FeedbackSystem;
using System.Collections;
using UnityEngine;
using static SiegeStorm.WeaponSystem.PlayerWeapon;

namespace SiegeStorm.WeaponSystem
{
    public class RaycastWeapon : Weapon
    {
        [SerializeField] private LineRenderer _tracerLine;
        [SerializeField] private float _tracerDuration = 1f;

        [SerializeReference] private IActionFeedback[] _shootFeedback;

        private Coroutine _coroutine;

        protected override void Shoot(ShootPhase shootPhase)
        {
            if (shootPhase != ShootPhase.Performed) return;

            Vector3 targetPosition = GetTargetPosition();
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0;

            float distance = 20f;
            FindTargetAndApplyDamage(direction, distance);

            foreach (IActionFeedback shoot in _shootFeedback)
            {
                shoot.Active();
            }

            base.Shoot(shootPhase);
        }

        private void FindTargetAndApplyDamage(Vector3 direction, float distance)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, distance))
            {
                if (hit.collider.TryGetComponent(out IDamageable damageable))
                {
                    damageable.Health.TakeDamage(Data.Damage);
                    _coroutine = StartCoroutine(ShowTracer(hit.point));
                }
            }
            else
            {
                _coroutine = StartCoroutine(ShowTracer(transform.position + direction * distance));
            }
        }

        private IEnumerator ShowTracer(Vector3 hitPoint)
        {
            _tracerLine.SetPosition(0, transform.position);
            _tracerLine.SetPosition(1, hitPoint);

            _tracerLine.enabled = true;

            yield return new WaitForSeconds(_tracerDuration);

            _tracerLine.enabled = false;
            _coroutine = null;
        }
    }
}