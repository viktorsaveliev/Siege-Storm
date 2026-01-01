
namespace SiegeStorm.UnitSystem
{
    public class Skeleton : Warrior
    {
        protected override void InitAI()
        {
            float attackDistance = 2f;
            AISystem ai = new(this, new CloseCombatState(this, attackDistance));
            AddSystem(ai);
        }
    }
}