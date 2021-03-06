﻿using Flattiverse;
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
        public float Gravity;

        public CosmicUnit(Unit unit)
        {
            Radius = unit.Radius;
            Name = unit.Name;
            Position = unit.Position;
            Team = unit.Team;
            MoveVector = unit.Movement;
            Type = CosmicUnitKind.Unknown;
            Gravity = unit.Gravity;

            Still = (unit.Mobility == Mobility.Still);

            Timeout = 10;
        }

        public CosmicUnit(Ship ownerShip)
        {
            Radius = ownerShip.Radius;
            Name = ownerShip.Name;
            Position = new Vector(0, 0);
            Type = CosmicUnitKind.Ship;
            MoveVector = new Vector();
            Gravity = ownerShip.Gravity;
            Still = false;
        }

        public static CosmicUnit FromFVUnit(Unit unit)
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
                    return new CosmicExplosion(unit);
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
            Timeout = 10;
        }

        public virtual void Update(CosmicUnit unit)
        {
            Position = unit.Position;
            MoveVector = unit.MoveVector;

            Radius = unit.Radius;
            Team = unit.Team;
        }
    }
}
