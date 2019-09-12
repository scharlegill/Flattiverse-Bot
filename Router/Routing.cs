using Flattiverse;
using Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Router
{
    public class Routing
    {
        private List<RoutingPoint> openList;
        private List<RoutingPoint> closedList;
        private CosmicOwnership ownShip;
        private List<Vector> directionList;
        private RoutingPoint destinationPoint;
        private float minimumDistance;
        private float unitLength;
        private CosmicMap map;
        private List<Vector> destinationRouter;

        public Routing(Vector destination, CosmicMap map, CosmicOwnership routingShip)
        {
            directionList = new List<Vector>();
            closedList = new List<RoutingPoint>();
            openList = new List<RoutingPoint>();
            this.map = map;


            ownShip = routingShip;
            unitLength = routingShip.Radius * 4;
            ownShip = routingShip;
            minimumDistance = routingShip.Radius * 4;
            destinationPoint = new RoutingPoint(destination, null, routingShip, 0);

            Vector rightVector = new Vector(1, 0);
            rightVector.Length = unitLength;
            directionList.Add(rightVector);

            Vector righttopVector = new Vector(1, 1);
            righttopVector.Length = unitLength * (float)Math.Sqrt(2);
            directionList.Add(righttopVector);

            Vector rightdownVector = new Vector(1, -1);
            rightdownVector.Length = unitLength * (float)Math.Sqrt(2);
            directionList.Add(rightdownVector);

            Vector leftVector = new Vector(-1, 0);
            leftVector.Length = unitLength;
            directionList.Add(leftVector);

            Vector lefttopVector = new Vector(-1, 1);
            lefttopVector.Length = unitLength * (float)Math.Sqrt(2);
            directionList.Add(lefttopVector);

            Vector leftdownVector = new Vector(-1, -1);
            leftdownVector.Length = unitLength * (float)Math.Sqrt(2);
            directionList.Add(leftdownVector);

            Vector topVector = new Vector(0, 1);
            topVector.Length = unitLength;
            directionList.Add(topVector);

            Vector downVector = new Vector(0, -1);
            downVector.Length = unitLength;
            directionList.Add(downVector);

            openList.Add(new RoutingPoint(routingShip.Position, null , routingShip, 0));
            openList.Sort();
        }

        public List<Vector> Route()
        {
            List<RoutingPoint> possibleNewPoints;
            RoutingPoint reachedDestinationWithLeastCost;
            float destinationCost;

            while (true)
            {
                possibleNewPoints = new List<RoutingPoint>();

                foreach (RoutingPoint openPoint in openList)
                {
                    reachedDestinationWithLeastCost = null;
                    destinationCost = float.MaxValue;

                    foreach (Vector direction in directionList)
                    {
                        Vector possibleNewPoint = openPoint.Point + direction;

                        if (CheckClosedList(possibleNewPoint) && map.WayFree(possibleNewPoint, minimumDistance))
                        {
                            if (destinationPoint.Distance(possibleNewPoint) < minimumDistance)
                            {
                                RoutingPoint currentParent = new RoutingPoint(possibleNewPoint, openPoint, ownShip, direction.Length);

                                if (reachedDestinationWithLeastCost == null || destinationCost > currentParent.Cost)
                                {
                                    reachedDestinationWithLeastCost = currentParent;
                                    destinationCost = currentParent.Cost;
                                }
                            }

                            possibleNewPoints.Add(new RoutingPoint(possibleNewPoint, openPoint, ownShip, direction.Length));
                        }
                    }

                    if (reachedDestinationWithLeastCost != null)
                    {
                        destinationRouter = new List<Vector>();

                        destinationRouter.Add(destinationPoint.Point);
                        destinationRouter.Add(reachedDestinationWithLeastCost.Point);

                        reachedDestinationWithLeastCost = reachedDestinationWithLeastCost.GoBack(ref destinationRouter);

                        while (reachedDestinationWithLeastCost != null)
                        {
                            reachedDestinationWithLeastCost = reachedDestinationWithLeastCost.GoBack(ref destinationRouter);
                        }

                        destinationRouter.Reverse();

                        return destinationRouter;
                    }

                    if (!closedList.Contains(openPoint))
                        closedList.Add(openPoint);
                }

                if (possibleNewPoints.Count == 0)
                    throw new Exception();

                foreach (RoutingPoint possibleNewPoint in possibleNewPoints)
                    openList.Add(possibleNewPoint);

                openList.Sort();
            }
        }

        public bool CheckClosedList(Vector newPoint)
        {
            bool free = true;

            foreach (RoutingPoint point in closedList)
            {
                if (!point.WayFree(newPoint, minimumDistance))
                    free = false;
            }

            return free;
        }
    }
}
