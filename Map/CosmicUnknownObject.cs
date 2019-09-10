using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flattiverse;

namespace Map
{
    public class CosmicUnknownObject : CosmicUnit
    {
        public CosmicUnknownObject(Unit unit) : base(unit)
        {
            Type = CosmicUnitKind.Unknown;
        }
    }
}
