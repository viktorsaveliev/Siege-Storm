using UnityEngine;
using Sirenix.OdinInspector;

namespace SiegeStorm.PoolSystem
{
    public class PoolHandler : MonoBehaviour
    {
        public ObjectPool<AudioSource> AudioPool => _audioPool;

        [SerializeField] private Transform _container;

        [Title("Prefabs")]
        [SerializeField] private AudioSource _audioPrefab;

        private ObjectPool<AudioSource> _audioPool;

        private const int _defaultCapacity = 5;

        private void Awake()
        {
            CreatePools();
        }

        private void CreatePools()
        {
            _audioPool = new(_audioPrefab, _container, _defaultCapacity);
            _audioPool.CreatePool();
        }
    }
}