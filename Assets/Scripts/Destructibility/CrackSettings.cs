using SiegeStorm.PoolSystem;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace SiegeStorm.Destructibility
{
    [CreateAssetMenu(fileName = "CrackSettings", menuName = "Destructibility/Crack Settings")]
    public class CrackSettings : ScriptableObject
    {
        [SerializeField] private DecalProjector _prefab;
        [SerializeField, Range(1, 5)] private int _poolCapacity = 5;

        private ObjectPool<DecalProjector> _pool;

        public void Init(Transform container)
        {
            _pool = new(_prefab, container, _poolCapacity);
            _pool.CreatePool();
        }

        public DecalProjector GetInactiveCrack() => _pool.GetInactiveObject();
    }
}