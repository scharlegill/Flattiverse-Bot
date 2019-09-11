using Flattiverse;

namespace _2018Bot
{
    internal class CosmicExplosion : CosmicUnit
    {
        public CosmicExplosion(Unit unit) : base(unit)
        {
            Timeout = 100;

            Type = CosmicUnitKind.Explosion;
        }

        public override void UpdateTimeout()
        {
            Timeout = 100;
        }

        public override void Update(CosmicUnit unit)
        {
            base.Update(unit);
        }

    }
}