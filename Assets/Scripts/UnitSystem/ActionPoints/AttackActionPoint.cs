using UnityEngine;

namespace SiegeStorm.UnitSystem
{
    public class AttackActionPoint : IActionPoint
    {
        private readonly Unit _unit;

        public AttackActionPoint(Unit unit)
        {
            _unit = unit;
        }

        public void OnFinishMove()
        {
            
        }
    }
}