using Flattiverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map
{
    public class CosmicMoon : CosmicUnit
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
