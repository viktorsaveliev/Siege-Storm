using SiegeStorm.Destructibility;
using System.Collections;
using UnityEngine;

namespace SiegeStorm.WeaponSystem.ProjectileSystem
{
    public class DelayableProjectile : Projectile, IExplodable
    {
        public ExplosionHandler ExplosionHandler { get; private set; }

        [SerializeField] private ParticleSystem _explode;

        private void Awake()
        {
            ExplosionHandler = new(_explode, Data, TargetLayerMask);
        }

        public override void Launch(Vector3 startPoint, Vector3 targetPoint, float speed)
        {
            base.Launch(startPoint, targetPoint, speed);

            if(Data.TryGetModule(out ExplodeDelayModule explodeDelay))
            {
                StartCoroutine(StartTimer(explodeDelay.Delay + Random.Range(0, 0.6f)));
            }
        }

        public virtual void Explode() => ExplosionHandler.Explode(transform.position);

        private IEnumerator StartTimer(float delay)
        {
            yield return new WaitForSeconds(delay);

            Explode();
            Complete();

            gameObject.SetActive(false);
        }
    }
}