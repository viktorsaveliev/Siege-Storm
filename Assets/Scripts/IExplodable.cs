
namespace SiegeStorm.Destructibility
{
    public interface IExplodable
    {
        public ExplosionHandler ExplosionHandler { get; }
        public void Explode();
    }
}