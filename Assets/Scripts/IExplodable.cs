
namespace SiegeStorm.Destructibility
{
    public interface IExplodable
    {
        public Explosion Explosion { get; }
        public void Explode();
    }
}