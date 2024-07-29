using Sirenix.OdinInspector;
using UnityEngine;

namespace SiegeStorm.UnitSystem
{
    public class UnitData : ScriptableObject
    {
        public string Name => _name;
        public string Description => _description;

        [Title("Base")]
        [SerializeField] private string _name;
        [SerializeField] private string _description;
    }
}