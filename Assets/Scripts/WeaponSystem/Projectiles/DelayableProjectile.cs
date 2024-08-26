using SiegeStorm.Destructibility;
using System.Collections;
using UnityEngine;

namespace SiegeStorm.WeaponSystem.ProjectileSystem
{
    public class DelayableProjectile : Projectile, IExplodable
    {
        public Explosion Explosion { get; private set; }

        [SerializeField] private ParticleSystem _explode;
        [SerializeField] private Rigidbody _rigidbody;

        private void OnValidate()
        {
            if (_rigidbody == null)
            {
                _rigidbody = GetComponent<Rigidbody>();
            }
        }

        private void Awake()
        {
            Explosion = new(_explode, TargetLayerMask);
            Explosion.Init();
        }

        public override void Launch(Vector3 startPoint, Vector3 targetPoint, float speed)
        {
            base.Launch(startPoint, targetPoint, speed);

            Vector3 direction = (startPoint - targetPoint).normalized;
            _rigidbody.AddForce(direction * speed, ForceMode.Impulse);

            if (Data.TryGetModule(out ExplodeDelayModule explodeDelay))
            {
                StartCoroutine(StartTimer(explodeDelay.Delay + Random.Range(0, 0.6f)));
            }
        }

        public virtual void Explode() => Explosion.Explode(transform.position, Data.Radius, Data.Damage);

        private IEnumerator StartTimer(float delay)
        {
            yield return new WaitForSeconds(delay);

            Explode();
            Complete();

            gameObject.SetActive(false);
        }
    }
}