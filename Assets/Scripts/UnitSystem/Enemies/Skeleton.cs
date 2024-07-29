
namespace SiegeStorm.UnitSystem
{
    public class Skeleton : Warrior
    {
        protected override void InitAI()
        {
            AISystem ai = new(this, new CloseCombatPursuitState(this));
            AddSystem(ai);
        }
    }
}