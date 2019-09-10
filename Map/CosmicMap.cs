using Flattiverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map
{
    public class CosmicMap
    {
        public Vector ScanningShipMovement = new Vector();
        private Vector movement = null;

        private Dictionary<string, CosmicUnit> namedUnits;
        private List<CosmicShip> ownShipUnits = new List<CosmicShip>();
        private List<CosmicUnit> shipUnits = new List<CosmicUnit>();
        private List<CosmicUnit> shotUnits = new List<CosmicUnit>();
        private List<CosmicUnit> stillUnits = new List<CosmicUnit>();
        private List<CosmicUnit> mobileUnits = new List<CosmicUnit>();
        private List<CosmicUnit> gravitalUnits = new List<CosmicUnit>();

        public List<Vector> NewDestination = new List<Vector>();

        private object sync = new object();

        public CosmicMap(List<Unit> scannedUnits, Ship scanningShip)
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
                CosmicUnit cosmicUnit = CosmicUnit.FromFVUnit(scannedUnit);

                if (cosmicUnit.Still)
                    cosmicUnit.MoveVector = new Vector();
                else
                    cosmicUnit.MoveVector += movement;

                if (cosmicUnit.Still)
                    ScanningShipMovement = -scannedUnit.Movement;

                if (cosmicUnit.Still)
                    stillUnits.Add(cosmicUnit);
                else
                    mobileUnits.Add(cosmicUnit);

                if (cosmicUnit.Gravity != 0)
                    gravitalUnits.Add(cosmicUnit);

                if (cosmicUnit is CosmicExplosion)
                    namedUnits.Add("e-" + scannedUnit.Name, cosmicUnit);
                else
                    namedUnits.Add(scannedUnit.Name, cosmicUnit);

                if (cosmicUnit is CosmicShot || cosmicUnit is CosmicExplosion)
                    shotUnits.Add(cosmicUnit);

                if (cosmicUnit is CosmicShip)
                    shipUnits.Add(cosmicUnit);
            }

            CosmicOwnership ownShip = new CosmicOwnership(scanningShip);

            ownShip.MoveVector = ScanningShipMovement;

            namedUnits.Add(ownShip.Name, ownShip);

            if (ownShip.Still)
                stillUnits.Add(ownShip);
            else
                mobileUnits.Add(ownShip);

            shipUnits.Add(ownShip);

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

        public bool TryGetValue(string name, out CosmicUnit unit)
        {
            lock (sync)
                return namedUnits.TryGetValue(name, out unit);
        }

        public bool IsMergable
        {
            get
            {
                lock (sync)
                    return stillUnits.Count > 0;
            }
        }

        public List<CosmicUnit> GravitalUnits { get => gravitalUnits; set => gravitalUnits = value; }

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

        public bool Merge(CosmicMap map)
        {
            CosmicUnit dst = null;
            Vector diff = null;

            lock (sync)
                lock (map.sync)
                {
                    ScanningShipMovement = map.ScanningShipMovement;

                    foreach (CosmicUnit unit in map.stillUnits)
                        if (namedUnits.TryGetValue(unit.Name, out dst))
                        {
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

        public List<CosmicUnit> Query(float left, float top, float right, float bottom)
        {
            List<CosmicUnit> units = new List<CosmicUnit>();

            lock (sync)
                foreach (CosmicUnit unit in namedUnits.Values)
                    if (unit.Position.X + unit.Radius > left && unit.Position.Y + unit.Radius > top && unit.Position.X - unit.Radius < bottom && unit.Position.Y - unit.Radius < right)
                        units.Add(unit);

            return units;
        }

        public Vector CalculateGravity(Vector point)
        {
            Vector gravity = new Vector();

            foreach (CosmicUnit gCUnit in gravitalUnits)
            {
                Vector diff = gCUnit.Position - point;

                if (diff.Length < 100)
                    diff.Length = 100;

                diff.Length = gCUnit.Gravity * 100 / diff.Length;

                gravity += diff;
            }

            return gravity;
        }

        public List<CosmicUnit> GetTargets(string name, Team team)
        {
            List<CosmicUnit> targets = new List<CosmicUnit>();

            lock (sync)
            {


            }

            return targets;
        }
    }
}
