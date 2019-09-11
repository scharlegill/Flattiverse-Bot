using Flattiverse;
using System.Collections.Generic;

namespace _2018Bot
{
    internal class CosmicSun : CosmicUnit
    {
        public List<CosmicCorona> Coronas;

        public CosmicSun(Unit unit) : base(unit)
        {
            Coronas = new List<CosmicCorona>();

            foreach (Corona corona in ((Sun)unit).Coronas)
            {
                Coronas.Add(new CosmicCorona(corona));
            }

            Type = CosmicUnitKind.Sun;
        }

        public override void Update(CosmicUnit unit)
        {
            base.Update(unit);
        }
    }
}