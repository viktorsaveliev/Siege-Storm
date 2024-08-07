using SiegeStorm.UnitSystem;
using UnityEngine;

namespace SiegeStorm.UISystem
{
    public class WeaponRadiusUI : MonoBehaviour
    {
        [SerializeField] private Transform _radiusIndicator;
        [SerializeField] private MeshRenderer _renderer;

        [SerializeField] private Material _regularMaterial;
        [SerializeField] private Material _targetInsideMaterial;

        [SerializeField] private LayerMask _targetLayer;

        private int _targetsInsideCount;

        private void OnTriggerEnter(Collider other)
        {
            if (Layer.IsInLayerMask(other.gameObject.layer, _targetLayer))
            {
                if (other.TryGetComponent(out Warrior warrior) && warrior.Health.IsAlive)
                {
                    _targetsInsideCount++;
                    _renderer.material = _targetInsideMaterial;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (Layer.IsInLayerMask(other.gameObject.layer, _targetLayer))
            {
                if (--_targetsInsideCount <= 0)
                {
                    _targetsInsideCount = 0;
                    _renderer.material = _regularMaterial;
                }
            }
        }

        public void SetPosition(Vector3 position)
        {
            _radiusIndicator.transform.position = position;
        }

        public void SetActive(bool active) => gameObject.SetActive(active);

        public void SetRadius(float radius)
        {
            float finalRadius = radius * 2.5f;
            _radiusIndicator.transform.localScale = new Vector3(finalRadius, finalRadius, 1);

            SetActive(true);
        }
    }
}