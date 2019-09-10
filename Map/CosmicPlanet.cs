using Flattiverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map
{
    public class CosmicPlanet : CosmicUnit
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
