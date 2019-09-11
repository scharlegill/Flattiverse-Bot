using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Flattiverse;

namespace _2018Bot
{
    public class CosmicShip : CosmicUnit
    {
        private readonly ControllableInfo ci;

        public CosmicShip(Unit unit) : base(unit)
        {
            Type = CosmicUnitKind.Ship;

            ci = ((PlayerUnit)unit).ControllableInfo;
        }

        public float HullMax
        {
            get
            {
                return ci.HullMax;
            }
        }

        public float Hull
        {
            get
            {
                return ci.Hull;
            }
        }

        public float ShieldMax
        {
            get
            {
                return ci.ShieldMax;
            }
        }

        public float Shield
        {
            get
            {
                return ci.Shield;
            }
        }

        public override void UpdateTimeout()
        {
            Timeout = 30;
        }

        public override void Update(CosmicUnit unit)
        {
            base.Update(unit);
            Timeout = 30;
        }
    }
}
