using Flattiverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map
{
    public class CosmicSun : CosmicUnit
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
