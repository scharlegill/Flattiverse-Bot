using Flattiverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace _2018Bot
{
    public class Starship
    {
        public List<Map> ShipMapHolder = new List<Map>();

        public Map CurrentMap;
        public readonly string Name;
        public List<Vector> NewDestination = new List<Vector>();
        private float shotStartDistance;
        private Vector shootVectorUnscaled = new Vector();
        private float shotSpeed;
        private float shotTime;
        private Vector shootVector = new Vector();

        private Ship ship;
        private UniverseGroup universeGroup;
        private Starbase starBase;
        private Vector moveVector = new Vector();
        private int scanRotation;
        private float scanRange;
        private float scanAngle = 0;
        private Vector movement = new Vector();

        public object ShipMapSync = new object();

        public Starship(Starbase starBase, UniverseGroup universeGroup)
        {
            this.universeGroup = universeGroup;
            this.starBase = starBase;

            Random r = new Random();
            Name = "Scharle" + r.Next(1000, 2000).ToString();

            ship = universeGroup.RegisterShip("ScharleRover", Name);

            Thread thread = new Thread(shipRotation);
            thread.Name = "StarShip";
            thread.Start();

            scanRange = ship.ScannerArea.Limit;
            scanAngle = ship.ScannerDegreePerScan;

            shotStartDistance = ship.WeaponSize + ship.Radius - 10;

            shotSpeed = ship.WeaponShot.Speed.Limit;
            shotTime = ship.WeaponShot.Time.Limit;
        }

        private void shipRotation()
        {
            UniverseGroupFlowControl flowControl = universeGroup.GetNewFlowControl();

            ship.Continue();

            while (true)
            {
                flowControl.Commit();
                flowControl.Wait();

                try
                {
                    Map map = starBase.MergeAll(scan());

                    if (CurrentMap == null || CurrentMap.Count <= map.Count)
                        CurrentMap = map;

                    move();
                    shoot();

                    if (ship.Energy > ship.EnergyMax * 0.8 && ship.Shield < ship.ShieldMax)
                        ship.LoadShields(ship.ShieldLoad.Limit);
                }
                catch(GameException ex)
                {
                    switch (ex.ErrorNumber)
                    {
                        case 0x84:
                            lock (starBase.SyncMessages)
                                starBase.Messages.Add(ex.Message);
                            NewDestination = new List<Vector>();
                            ship.Continue();
                            break;
                        case 0x91:
                            lock (starBase.SyncMessages)
                                starBase.Messages.Add(ex.Message);
                            break;
                        case 0x92:
                            lock (starBase.SyncMessages)
                                starBase.Messages.Add(ex.Message);
                            break;
                        default:
                            lock (starBase.SyncMessages)
                                starBase.Messages.Add(ex.Message);
                            break;

                    }
                }
            }
        }

        public Starbase Starbase
        {
            get
            {
                return starBase;
            }

        }

        public float Radius
        {
            get
            {
                return ship.Radius;
            }
        }

        public float Weight
        {
            get
            {
                return ship.Gravity;
            }
        }

        public Team Team
        {
            get
            {
                return starBase.Connector.Player.Team;
            }
        }

        public Vector Movement
        {
            get
            {
                return movement;
            }

            set
            {
                movement = value;
            }
        }

        private Map scan()
        {
            Vector referenceVector;
            List<Unit> scannedUnits;

            if (scanRotation == 3)
                scanRotation = 0;
            else
                scanRotation++;

            try
            {
                referenceVector = new Vector() - moveVector;

                if (CurrentMap != null)
                    referenceVector = findScanreference();

                scannedUnits = ship.Scan(new ScanInfo(referenceVector.Angle - 5, referenceVector.Angle + 5, scanRange), new ScanInfo(90 * scanRotation - scanAngle / 2, 90 * scanRotation + scanAngle / 2, scanRange));
            }
            catch
            {
                scannedUnits = ship.Scan(new ScanInfo(45 * scanRotation - scanAngle / 2, 45 * scanRotation + scanAngle / 2, scanRange));
            }

            return new Map(scannedUnits, this);
        }

        private void shoot()
        {
            List<CosmicUnit> targets = new List<CosmicUnit>();
            CosmicUnit currentShip;
            CurrentMap.TryGetValue(Name, out currentShip);

            if (currentShip == null)
                return;

            if (currentShip.Type != CosmicUnitKind.StarShip)
                return;

            targets = CurrentMap.GetTargets(Name, Team);

            foreach (CosmicUnit target in targets)
            {
                Vector firstTargetVector = target.Position - currentShip.Position;

                firstTargetVector.Length -= shotStartDistance + 3;

                float ticks = firstTargetVector.Length / (shotSpeed);

                Vector targetVector = (-target.MoveVector) * ticks;

                Console.WriteLine("Movevector of target " + target.Name + ": " + target.MoveVector);

                shootVectorUnscaled = firstTargetVector + targetVector;

                int totalTicks = (int)(shootVectorUnscaled.Length / shotSpeed);

                if (totalTicks <= shotTime && ((CosmicStarship)currentShip).WeaponProductionStatus >= 1 && ((CosmicStarship)currentShip).Energy > ((CosmicStarship)currentShip).EnergyMax / 8)
                {
                    Console.WriteLine("Shooting target with vector " + shootVector + " and totalTicks: " + totalTicks);

                    shootVector = new Vector(shootVectorUnscaled);
                    shootVector.Length = shotSpeed - 0.01f;

                    ship.Shoot(shootVector, totalTicks);
                }
            }
        }

        private void move()
        {
            CosmicUnit shipUnit;
            Vector newDestinationVector;

            if (starBase.BaseMapHolder.Count > 0 && starBase.SelectedShip.CurrentMap != null)
            {
                if (!CurrentMap.TryGetValue(Name, out shipUnit))
                    return; 

                if (NewDestination.Count > 0)
                {
                    newDestinationVector = NewDestination[0] - shipUnit.Position;

                    moveVector = newDestinationVector;

                    if (newDestinationVector.Length < shipUnit.Radius * 8)
                        newDestinationVector.Length /= 2;

                    if (newDestinationVector.Length < shipUnit.Radius * 4)
                        newDestinationVector.Length /= 4;

                    if (newDestinationVector.Length < shipUnit.Radius * 2)
                        newDestinationVector.Length /= 8;
                }

                moveVector -= movement;
                moveVector.Length = ship.EngineAcceleration.Limit;

                ship.Move(moveVector);
            }
        }

        private Vector findNearestUnit()
        {
            CosmicUnit shipUnit;

            if (CurrentMap.TryGetValue(Name, out shipUnit))
                return CurrentMap.NextUnit(shipUnit.Position).Position;

            throw new InvalidOperationException("I, my self are not known in se wide wide universe of my own map. because of retarded design.");
        }

        private Vector findScanreference()
        {
            CosmicUnit shipUnit;

            if (CurrentMap.TryGetValue(Name, out shipUnit))
                return CurrentMap.ScanReference(shipUnit.Position).Position;

            throw new InvalidOperationException("I, my self are not known in se wide wide universe of my own map. because of retarded design.");
        }
    }
}
