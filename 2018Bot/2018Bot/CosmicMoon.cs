using Flattiverse;

namespace _2018Bot
{
    internal class CosmicMoon : CosmicUnit
    {
        public CosmicMoon(Unit unit) : base(unit)
        {
            Type = CosmicUnitKind.Moon;
        }

        public override void Update(CosmicUnit unit)
        {
            base.Update(unit);
        }
    }
}