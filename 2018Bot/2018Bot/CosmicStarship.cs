using Flattiverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2018Bot
{
    class CosmicStarship : CosmicUnit
    {

        private readonly Controllable ci;

        public CosmicStarship(Starship starship) : base(starship)
        {
            Type = CosmicUnitKind.StarShip;
            Team = starship.Team;

            Timeout = 30;
            
            ci = starship.Starbase.Connector.Controllables[starship.Name];
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

        public float Energy
        {
            get
            {
                return ci.Energy;
            }
        }

        public float EnergyMax
        {
            get
            {
                return ci.EnergyMax;
            }
        }

        public float WeaponProductionStatus
        {
            get
            {
                return ci.WeaponProductionStatus;
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
