using Flattiverse;

namespace _2018Bot
{
    internal class CosmicPlanet : CosmicUnit
    {
        public CosmicPlanet(Unit unit) : base(unit)
        {
            Type = CosmicUnitKind.Planet;
        }

        public override void Update(CosmicUnit unit)
        {
            base.Update(unit);
        }
    }
}