using Flattiverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map
{
    public class CosmicExplosion : CosmicUnit
    {
        public CosmicExplosion(Unit unit) : base(unit)
        {
            Timeout = 100;

            Name = "e-" + unit.Name;

            Type = CosmicUnitKind.Explosion;

            Still = false;
        }

        public override void UpdateTimeout()
        {
            Timeout = 100;
        }

        public override void Update(CosmicUnit unit)
        {
            base.Update(unit);
        }
    }
}
