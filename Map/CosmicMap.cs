using Flattiverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map
{
    public class CosmicMap
    {
        private Vector shipmentMovement = new Vector();

        private Dictionary<string, CosmicUnit> namedUnits;
        private List<CosmicShip> ownShipUnits = new List<CosmicShip>();
        private List<CosmicUnit> shipUnits = new List<CosmicUnit>();
        private List<CosmicUnit> shotUnits = new List<CosmicUnit>();
        private List<CosmicUnit> stillUnits = new List<CosmicUnit>();
        private List<CosmicUnit> mobileUnits = new List<CosmicUnit>();
        private List<CosmicUnit> gravitalUnits = new List<CosmicUnit>();

        private object sync = new object();

        public CosmicMap(List<Unit> scannedUnits)
        {
            namedUnits = new Dictionary<string, CosmicUnit>();

            foreach (Unit scannedUnit in scannedUnits)
            {
                CosmicUnit cosmicUnit = CosmicUnit.FromFVUnit(scannedUnit);

                if (cosmicUnit.Still)
                    stillUnits.Add(cosmicUnit);
                else
                    mobileUnits.Add(cosmicUnit);

                if (cosmicUnit.Gravity == 0)
                    gravitalUnits.Add(cosmicUnit);

                namedUnits.Add(scannedUnit.Name, cosmicUnit);
            }

        }

        public bool IsMergable
        {
            get
            {
                lock (sync)
                    return stillUnits.Count > 0;
            }
        }

        public bool Merge(CosmicMap map)
        {
 
            lock (sync)
                lock (map.sync)
                {
             
                }

            return true;
        }

        public CosmicMap MergeAll(CosmicMap map)
        {
            return map;
        }

        public List<CosmicUnit> Query(float left, float top, float right, float bottom)
        {
            List<CosmicUnit> units = new List<CosmicUnit>();

            return units;
        }

        public Vector CalculateGravity(Vector point)
        {
            Vector gravity = new Vector();

            foreach (CosmicUnit gCUnit in gravitalUnits)
            {
                Vector diff = gCUnit.Position - point;

                if (diff < 100)
                    diff.Length = 100;

                diff.Length = gCUnit.Gravity * (-100) / diff.Length;

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
