using Zenject;
using UnityEngine;

namespace SiegeStorm.PoolSystem
{
    public class DIObjectPool<T> : Pool<T> where T : Component
    {
        private readonly DiContainer _diContainer;

        public DIObjectPool(T prefab, DiContainer diContainer, Transform container, int capacity) : base(prefab, container, capacity)
        {
            _diContainer = diContainer;
        }

        protected override T CreateObject()
        {
            return _diContainer.InstantiatePrefabForComponent<T>(Prefab, Container);
        }
    }
}