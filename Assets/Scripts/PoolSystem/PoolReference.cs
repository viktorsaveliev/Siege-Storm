using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace SiegeStorm.PoolSystem
{
    [Serializable]
    public class PoolReference
    {
        public PoolHandler Pool => _poolHandler;

        [SerializeField] private PoolHandler _poolHandler;

        [Button("Find Pool Object")]
        private void FindPool()
        {
            _poolHandler = UnityEngine.Object.FindObjectOfType<PoolHandler>();
        }
    }
}