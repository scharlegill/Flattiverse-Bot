using Flattiverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2018Bot
{
    public class CosmicUnit 
    {
        public Vector Position;
        public float Radius;
        public string Name;
        public CosmicUnitKind Type;
        public Team Team;
        public Vector MoveVector;
        public bool Still;
        public float Timeout;

        public CosmicUnit(Unit unit)
        {
            Radius = unit.Radius;
            Name = unit.Name;
            Position = unit.Position;
            Team = unit.Team;
            MoveVector = new Vector();
            Type = CosmicUnitKind.Unknown;

            Still = (unit.Mobility == Mobility.Still);

            Timeout = 10;
        }

        public static CosmicUnit FromFVUnit(Flattiverse.Unit unit)
        {
            switch (unit.Kind)
            {
                case UnitKind.Sun:
                    return new CosmicSun(unit);
                case UnitKind.Shot:
                    return new CosmicShot(unit);
                case UnitKind.PlayerShip:
                    return new CosmicShip(unit);
                case UnitKind.PlayerBase:
                    return new CosmicShip(unit);
                case UnitKind.PlayerProbe:
                    return new CosmicShip(unit);
                case UnitKind.Planet:
                    return new CosmicPlanet(unit);
                case UnitKind.Explosion:
                    return new CosmicUnit(unit);
                case UnitKind.Meteoroid:
                    return new CosmicMeteroid(unit);
                case UnitKind.MissionTarget:
                    return new CosmicMissionTarget(unit);
                case UnitKind.Moon:
                    return new CosmicMoon(unit);
                // ...
                default:
                    return new CosmicUnknownObject(unit);
            }
        }

        public CosmicUnit(Starship starship)
        {
            Radius = starship.Radius;
            Name = starship.Name;
            Position = new Vector(0, 0);
            Type = CosmicUnitKind.Ship;
            MoveVector = new Vector();
            Still = false;
        }

        public virtual void UpdateTimeout()
        {
            Timeout = 10;
        }

        public virtual void Update(CosmicUnit unit)
        {
            Position = unit.Position;
            MoveVector = unit.MoveVector;

            Radius = unit.Radius;
        }
    }
}
