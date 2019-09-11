using Flattiverse;

namespace _2018Bot
{
    internal class CosmicUnknownObject : CosmicUnit
    {
        public CosmicUnknownObject(Unit unit) : base(unit)
        {
            Type = CosmicUnitKind.Unknown;

        }
    }
}