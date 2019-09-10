using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flattiverse;

namespace Map
{
    public class CosmicOwnership : CosmicUnit
    {
        private readonly Controllable controllable;

        public CosmicOwnership(Ship scanningShip) : base(scanningShip)
        {
            Type = CosmicUnitKind.StarShip;
/*            Team = scanningShip.*/;

            Timeout = 30;

            controllable = scanningShip;
        }

        public float HullMax
        {
            get
            {
                return controllable.HullMax;
            }
        }

        public float Hull
        {
            get
            {
                return controllable.Hull;
            }
        }

        public float ShieldMax
        {
            get
            {
                return controllable.ShieldMax;
            }
        }

        public float Shield
        {
            get
            {
                return controllable.Shield;
            }
        }

        public float Energy
        {
            get
            {
                return controllable.Energy;
            }
        }

        public float EnergyMax
        {
            get
            {
                return controllable.EnergyMax;
            }
        }

        public float WeaponProductionStatus
        {
            get
            {
                return controllable.WeaponProductionStatus;
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
