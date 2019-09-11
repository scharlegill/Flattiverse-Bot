using Flattiverse;

namespace _2018Bot
{
    internal class CosmicShot : CosmicUnit
    {
        public CosmicShot(Unit unit) : base(unit)
        {
            Timeout = (ulong)((Shot)unit).Time;

            Type = CosmicUnitKind.Shot;
            //Name = Name.Replace(Name[0], '0');
        }

        public override void UpdateTimeout()
        {

        }

        public override void Update(CosmicUnit unit)
        {
            base.Update(unit);

            Timeout = 10;
        }
    }
}