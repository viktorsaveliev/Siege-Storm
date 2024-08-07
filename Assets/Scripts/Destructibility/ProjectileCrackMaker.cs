using SiegeStorm.WeaponSystem.ProjectileSystem;
using UnityEngine;
using Zenject;

namespace SiegeStorm.Destructibility
{
    [RequireComponent(typeof(Projectile))]
    public class ProjectileCrackMaker : MonoBehaviour
    {
        [SerializeField] private CrackSettings _crackSettings;
        [SerializeField] private Projectile _projectile;

        private CrackSystem _crackSystem;

        private void OnValidate()
        {
            if (_projectile == null)
            {
                _projectile = GetComponent<Projectile>();
            }
        }

        private void OnEnable()
        {
            _projectile.OnCompleted += CreateCrack;
        }

        private void OnDisable()
        {
            _projectile.OnCompleted -= CreateCrack;
        }

        [Inject]
        public void Construct(CrackSystem crackSystem)
        {
            _crackSystem = crackSystem;
        }

        private void CreateCrack()
        {
            Vector3 crackPosition = _projectile.transform.position;
            crackPosition.y += 0.3f;

            _crackSystem.ActiveCrack(_crackSettings, crackPosition);
        }
    }
}