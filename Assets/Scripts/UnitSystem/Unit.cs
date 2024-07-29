using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SiegeStorm.UnitSystem
{
    public abstract class Unit : MonoBehaviour
    {
        private readonly HashSet<IUnitSystem> _systems = new();

        private void OnValidate()
        {
            gameObject.layer = LayerMask.NameToLayer("Character");    
        }

        public abstract void Init();

        public void AddSystem(IUnitSystem system)
        {
            _systems.Add(system);
        }

        public void RemoveSystem(IUnitSystem system)
        {
            _systems.Remove(system);
        }

        public T GetSystem<T>() where T : IUnitSystem
        {
            return _systems.OfType<T>().FirstOrDefault();
        }
    }
}