using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flattiverse;

namespace Map
{
    public class CosmicShot : CosmicUnit
    {
        public CosmicShot(Unit unit) : base(unit)
        {
            Type = CosmicUnitKind.Shot;
        }

        public override void UpdateTimeout()
        {
            Timeout = 100;
        }

        public override void Update(CosmicUnit unit)
        {
            Position = unit.Position;
            MoveVector = unit.MoveVector;

            Radius = unit.Radius;
        }
    }
}
