using Flattiverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map
{
    public class CosmicMeteorid : CosmicUnit
    {
        public CosmicMeteorid(Unit unit) : base(unit)
        {
            Type = CosmicUnitKind.Meteroid;
        }

        public override void Update(CosmicUnit unit)
        {
            base.Update(unit);
        }
    }
}
