using UnityEngine;

namespace SiegeStorm.PoolSystem
{
    public class ObjectPool<T> where T : Component
    {
        private readonly int _capacity;
        private readonly T _prefab;
        private readonly T[] _pool;
        private readonly Transform _container;

        public ObjectPool(T prefab, Transform container, int capacity)
        {
            _prefab = prefab;
            _container = container;
            _capacity = capacity;
            _pool = new T[capacity];
        }

        public void CreatePool()
        {
            for (int i = 0; i < _capacity; i++)
            {
                _pool[i] = Object.Instantiate(_prefab, _container);
            }
        }

        public T GetInactiveObject()
        {
            T freeObj = null;

            foreach (T pool in _pool)
            {
                if (pool.gameObject.activeSelf) continue;
                freeObj = pool;
                break;
            }

            return freeObj;
        }
    }
}