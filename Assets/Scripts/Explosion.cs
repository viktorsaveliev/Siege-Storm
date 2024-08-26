using System;
using UnityEngine;

namespace SiegeStorm.Destructibility
{
    public class Explosion
    {
        public event Action OnExploded;

        private readonly ParticleSystem _explode;
        private readonly LayerMask _targetLayer;

        private readonly Collider[] _targets = new Collider[10];

        public Explosion(ParticleSystem effect, LayerMask targetLayer)
        {
            _explode = effect;
            _targetLayer = targetLayer;
        }

        public void Init()
        {
            if (_explode != null)
            {
                _explode.transform.SetParent(null);
                _explode.gameObject.SetActive(false);
            }
        }

        public void Explode(Vector3 position, float radius, int damage)
        {
            if (_explode != null)
            {
                _explode.transform.position = position;
                _explode.gameObject.SetActive(true);
                _explode.Play();
            }

            int numColliders = Physics.OverlapSphereNonAlloc(position, radius, _targets, _targetLayer);

            for (int i = 0; i < numColliders; i++)
            {
                IDamageable target = _targets[i].GetComponent<IDamageable>();
                target.Health.TakeDamage(damage);
            }

            OnExploded?.Invoke();
        }
    }
}