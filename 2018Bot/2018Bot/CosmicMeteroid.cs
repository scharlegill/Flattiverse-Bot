using Flattiverse;

namespace _2018Bot
{
    internal class CosmicMeteroid : CosmicUnit
    {
        public CosmicMeteroid(Unit unit) : base(unit)
        {
            Type = CosmicUnitKind.Meteroid;
        }

        public override void Update(CosmicUnit unit)
        {
            base.Update(unit);
        }
    }
}