using SiegeStorm.WeaponSystem.ProjectileSystem;
using System;
using UnityEngine;

namespace SiegeStorm.Destructibility
{
    public class Explosion
    {
        public event Action OnExploded;

        private readonly ParticleSystem _effect;
        private readonly Vector3 _position;
        private readonly float _radius;
        private readonly int _damage;
        private readonly LayerMask _targetLayer;

        private readonly Collider[] _targets = new Collider[10];

        public Explosion(ParticleSystem effect, Vector3 position, float radius, int damage, LayerMask targetLayer)
        {
            _effect = effect;
            _position = position;
            _radius = radius;
            _damage = damage;
            _targetLayer = targetLayer;
        }

        public void Explode()
        {
            if (_effect != null)
            {
                _effect.transform.position = _position;
                _effect.gameObject.SetActive(true);
                _effect.Play();
            }

            int numColliders = Physics.OverlapSphereNonAlloc(_position, _radius, _targets, _targetLayer);

            for (int i = 0; i < numColliders; i++)
            {
                IDamageable target = _targets[i].GetComponent<IDamageable>();
                target.Health.TakeDamage(_damage);
            }

            OnExploded?.Invoke();
        }
    }

    public class ExplosionHandler
    {
        private readonly ParticleSystem _explode;
        private readonly ProjectileData _data;

        private LayerMask _targetLayerMask;

        public ExplosionHandler(ParticleSystem explode, ProjectileData data, LayerMask targetLayerMask)
        {
            _explode = explode;
            _data = data;
            _targetLayerMask = targetLayerMask;

            if (_explode != null)
            {
                _explode.transform.SetParent(null);
                _explode.gameObject.SetActive(false);
            }
        }

        public void Explode(Vector3 position)
        {
            if (_explode != null)
            {
                _explode.transform.position = position;
                _explode.gameObject.SetActive(true);
                _explode.Play();
            }

            Explosion explosion = new(_explode, position, _data.Radius, _data.Damage, _targetLayerMask);
            explosion.Explode();
        }
    }
}