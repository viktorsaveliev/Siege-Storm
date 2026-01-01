using Sirenix.OdinInspector;
using UnityEngine;

namespace SiegeStorm.TowerSystem
{
    public class Tower : MonoBehaviour, IDamageable
    {
        public HealthSystem Health { get; private set; }
        public Vector3 Position => transform.position;

        [SerializeField] private Animator _animator;
        [SerializeField, ReadOnly] private int _damageAnimIndex;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
                _damageAnimIndex = Animator.StringToHash("Damage");
            }
        }
#endif

        private void Awake()
        {
            Health = new(500);
        }

        private void OnEnable()
        {
            Health.OnTakedDamage += OnTakeDamage;
        }

        private void OnDisable()
        {
            Health.OnTakedDamage -= OnTakeDamage;
        }

        private void OnTakeDamage(int damage)
        {
            _animator.SetTrigger(_damageAnimIndex);
        }
    }
}