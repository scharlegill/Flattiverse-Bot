using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flattiverse;
using Map;

namespace ShotModule
{
    public static class Weapon
    {
        private static List<CosmicShip> targetShips = new List<CosmicShip>();

        // instead of passing aimPoint, target ship can be passed
        public static void Shoot(Ship ship, CosmicOwnership myShip, Vector aimPoint, Vector gravity)
        {
            Vector aimVector = new Vector();
            Vector resultantVector = new Vector();

            CosmicShip target = null;
            foreach (CosmicShip targetShip in targetShips)
                if ((targetShip.Position - aimPoint).Length <= targetShip.Radius)
                    target = targetShip;

            if (target == null)
            {
                Console.WriteLine("Cannot shoot! No target ship found");
                return;
            }

            // 1. Target vector
            aimVector = target.Position - myShip.Position;

            float aimVectorTime = aimVector.Length / ship.WeaponShot.Speed.Limit;

            // 2. Adding target move vector 
            resultantVector = aimVector + target.MoveVector * aimVectorTime;

            // 3. Add gravity compensation
            //resultantVector -= gravity;

            // 4. Add myShip movement compensation
            //resultantVector -= myShip.MoveVector;

            Console.WriteLine("Target: " + target.Name);
            Console.WriteLine("Target move vector: " + target.MoveVector);
            Console.WriteLine("Gravity: " + gravity.X.ToString() + " " + gravity.Y.ToString());
            Console.WriteLine("myShip move vector " + myShip.MoveVector);

            int resultantVectorTime = (int)(resultantVector.Length / ship.WeaponShot.Speed.Limit) + 1;

            if (resultantVector.Length > ship.WeaponShot.Speed.Limit)
                resultantVector.Length = ship.WeaponShot.Speed.Limit;

            if (aimVectorTime > ship.WeaponShot.Time.Limit)
                aimVectorTime = (int)ship.WeaponShot.Time.Limit;

            //ship.Shoot(resultantVector, resultantVectorTime);
        }

        public static List<Vector> Evaluate(CosmicOwnership myShip, Ship ship, CosmicMap map)
        {
            if (ship.WeaponProductionStatus < 1)
                return new List<Vector>();

            float shotRadius = ship.WeaponShot.Speed.Limit * ship.WeaponShot.Time.Limit;

            targetShips.Clear();

            // Looking for enemies in range
            List<CosmicUnit> unitsInRange = map.Query(myShip.Position.X - shotRadius, myShip.Position.Y - shotRadius, myShip.Position.X + shotRadius, myShip.Position.Y + shotRadius);
            List<Vector> shotOptions = new List<Vector>();

            foreach (CosmicUnit unit in unitsInRange)
            {
                if (!(unit is CosmicShip) || unit.Timeout < 0)// || unit.Team.Name == myShip.Team.Name) // check team - null ex
                    continue;

                targetShips.Add((CosmicShip)unit);

                if ((unit.Position - myShip.Position).Length <= shotRadius + unit.Radius)
                {
                    shotOptions.Add(new Vector(unit.Position));
                    Console.WriteLine("in range: " + unit.Name + " with move vector: " + unit.MoveVector);
                }
            }

            return shotOptions;
        }
    }
}
