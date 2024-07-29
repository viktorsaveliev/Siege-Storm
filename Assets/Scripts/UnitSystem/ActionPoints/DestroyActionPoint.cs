using UnityEngine;

namespace SiegeStorm.UnitSystem
{
    public class DestroyActionPoint : IActionPoint
    {
        private readonly Unit _unit;

        public DestroyActionPoint(Unit unit)
        {
            _unit = unit;
        }

        public void OnFinishMove()
        {
            Object.Destroy(_unit.gameObject);
        }
    }
}