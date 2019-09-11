using Flattiverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2018Bot
{
    public class Map
    {
        private Vector movement = null;
        private Vector shipmentMovement = new Vector();

        private Dictionary<string, CosmicUnit> namedUnits;
        private List<CosmicShip> ownShipUnits = new List<CosmicShip>();
        private List<CosmicUnit> shipUnits = new List<CosmicUnit>();
        private List<CosmicUnit> shotUnits = new List<CosmicUnit>();
        private List<CosmicUnit> stillUnits = new List<CosmicUnit>();
        private List<CosmicUnit> mobileUnits = new List<CosmicUnit>();

        private object sync = new object();

        public Map(List<Unit> scannedUnits, Starship scanningShip)
        {
            namedUnits = new Dictionary<string, CosmicUnit>();

            foreach (Unit scannedUnit in scannedUnits)
                if (scannedUnit.Mobility != Mobility.Still)
                {
                    movement = -scannedUnit.Movement;

                    break;
                }

            if (movement == null)
                movement = new Vector();

            foreach (Unit scannedUnit in scannedUnits)
            {
                CosmicUnit tUnit = CosmicUnit.FromFVUnit(scannedUnit);

                if (tUnit.Still)
                    tUnit.MoveVector = new Vector();
                else
                    tUnit.MoveVector += movement;

                if (tUnit.Still)
                    scanningShip.Movement = -scannedUnit.Movement;

                namedUnits.Add(tUnit.Name, tUnit);

                if (tUnit.Still)
                    stillUnits.Add(tUnit);
                else
                    mobileUnits.Add(tUnit);

                if (tUnit is CosmicShot || tUnit is CosmicExplosion)
                    shotUnits.Add(tUnit);

                if (tUnit is CosmicShip)
                    shipUnits.Add(tUnit);
            }

            CosmicStarship ownShip = new CosmicStarship(scanningShip);

            ownShip.MoveVector = scanningShip.Movement;

            namedUnits.Add(ownShip.Name, ownShip);

            if (ownShip.Still)
                stillUnits.Add(ownShip);
            else
                mobileUnits.Add(ownShip);

            shipUnits.Add(ownShip);
        }

        public List<CosmicUnit> GetTargets(string name, Team team)
        {
            List<CosmicUnit> targets = new List<CosmicUnit>();

            lock (sync)
                foreach (KeyValuePair<string, CosmicUnit> kvp in namedUnits)
                {
                    if (kvp.Value.Type == CosmicUnitKind.Ship && ((CosmicShip)kvp.Value).Timeout > 0 && kvp.Value.Name != name && ((CosmicShip)kvp.Value).Team.Name != team.Name || (kvp.Value.Type == CosmicUnitKind.MissionTarget && ((CosmicMissionTarget)kvp.Value).Team.Name == team.Name))
                    {
                        if (!targets.Contains(kvp.Value))
                            targets.Add(kvp.Value);
                    }
                }

            return targets;
        }

        public bool IsMergable
        {
            get
            {
                lock (sync)
                    return stillUnits.Count > 0;
            }
        }

        public List<CosmicUnit> Query(float left, float top, float right, float bottom)
        {
            List<CosmicUnit> units = new List<CosmicUnit>();

            lock (sync)
                foreach (CosmicUnit unit in namedUnits.Values)
                    if (unit.Position.X + unit.Radius > left && unit.Position.Y + unit.Radius > top && unit.Position.X - unit.Radius < right && unit.Position.Y - unit.Radius < bottom)
                        units.Add(unit);

            return units;
        }

        public CosmicUnit NextUnit(Vector position)
        {
            float distance = float.MaxValue;

            Vector cVector;
            CosmicUnit cUnit = null;

            lock (sync)
                foreach (KeyValuePair<string, CosmicUnit> kvp in namedUnits)
                {
                    cVector = kvp.Value.Position - position;

                    if (cVector < distance)
                    {
                        cUnit = kvp.Value;

                        distance = cVector.Length;
                    }
                }

            return cUnit;
        }

        public CosmicUnit ScanReference(Vector position)
        {
            float distance = float.MaxValue;

            Vector cVector;
            CosmicUnit cUnit = null;

            lock (sync)
                foreach (CosmicUnit unit in stillUnits)
                {
                    cVector = unit.Position - position;

                    if (cVector < distance)
                    {
                        cUnit = unit;

                        distance = cVector.Length;
                    }
                }

            return cUnit;
        }

        public bool TryGetValue(string name, out CosmicUnit unit)
        {
            lock (sync)
                return namedUnits.TryGetValue(name, out unit);
        }

        public CosmicUnit this[string name]
        {
            get
            {
                lock (sync)
                    return namedUnits[name];
            }
        }

        public int Count
        {
            get
            {
                lock (sync)
                    return namedUnits.Count;
            }
        }

        public bool HasUnits
        {
            get
            {
                lock (sync)
                    return namedUnits.Count > 0;
            }
        }

        public void CalculateMargins(out float left, out float top, out float right, out float bottom)
        {
            left = float.MaxValue;
            top = float.MaxValue;
            right = float.MinValue;
            bottom = float.MinValue;

            lock (sync)
                foreach (KeyValuePair<string, CosmicUnit> kvp in namedUnits)
                {
                    if (left > kvp.Value.Position.X - kvp.Value.Radius)
                        left = kvp.Value.Position.X - kvp.Value.Radius;

                    if (right < kvp.Value.Position.X + kvp.Value.Radius)
                        right = kvp.Value.Position.X + kvp.Value.Radius;

                    if (top > kvp.Value.Position.Y - kvp.Value.Radius)
                        top = kvp.Value.Position.Y - kvp.Value.Radius;

                    if (bottom < kvp.Value.Position.X + kvp.Value.Radius)
                        bottom = kvp.Value.Position.X + kvp.Value.Radius;
                }
        }

        public bool Merge(Map map)
        {
            CosmicUnit dst = null;
            Vector diff = null;

            lock (sync)
                lock (map.sync)
                {
                    foreach (CosmicUnit unit in map.stillUnits)
                        if (namedUnits.TryGetValue(unit.Name, out dst))
                        { // We found a unit match.
                            diff = unit.Position - dst.Position;

                            break;
                        }

                    if (diff == null)
                        return false;

                    foreach (KeyValuePair<string, CosmicUnit> kvp in map.namedUnits)
                        if (namedUnits.TryGetValue(kvp.Key, out dst))
                        {
                            if (!kvp.Value.Still)
                            {
                                kvp.Value.Position -= diff;

                                dst.Update(kvp.Value);
                            }
                        }
                        else
                        {
                            kvp.Value.Position -= diff;

                            namedUnits.Add(kvp.Value.Name, kvp.Value);

                            if (kvp.Value.Still)
                                stillUnits.Add(kvp.Value);
                            else
                                mobileUnits.Add(kvp.Value);

                            if (kvp.Value is CosmicShot || kvp.Value is CosmicExplosion)
                                shotUnits.Add(kvp.Value);

                            if (kvp.Value is CosmicShip)
                                shipUnits.Add(kvp.Value);
                        }
                }

            return true;
        }
    }
}
