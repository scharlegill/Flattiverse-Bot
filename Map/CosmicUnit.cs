using Flattiverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map
{
    public class CosmicUnit
    {
        public Vector Position;
        public Vector MoveVector;
        public float Radius;
        public string Name;
        public CosmicUnitKind Type;
        public Team Team;
        public bool Still;
        public float Timeout;

        internal Vector gravity;

        public CosmicUnit(Unit unit)
        {

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
                    return new CosmicMeteorid(unit);
                case UnitKind.MissionTarget:
                    return new CosmicMissionTarget(unit);
                case UnitKind.Moon:
                    return new CosmicMoon(unit);
                // ...
                default:
                    return new CosmicUnknownObject(unit);
            }
        }

        public virtual void UpdateTimeout()
        {


        }

        public virtual void Update(CosmicUnit unit)
        {


        }
    }
}
