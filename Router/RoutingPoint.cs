using Flattiverse;
using Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Router
{
    class RoutingPoint : IComparable
    {
        public Vector Point;
        public RoutingPoint LastPoint;
        private CosmicOwnership ship;
        public float Cost;

        public RoutingPoint(Vector point, RoutingPoint lastPoint, CosmicOwnership ownShip, float cost)
        {
            if (lastPoint != null)
                Cost = cost + lastPoint.Cost;

            Point = point;
            LastPoint = lastPoint;
            ship = ownShip;
        }

        public int CompareTo(object obj)
        {
            RoutingPoint otherRoutingPoint = obj as RoutingPoint;

            if (otherRoutingPoint.Cost > Cost)
                return -1;

            return 1;
        }

        public bool WayFree(Vector newPoint, float minimumDistance)
        {
            return (newPoint - Point).Length > minimumDistance;
        }

        public float Distance(Vector newPoint)
        {
            return (newPoint - Point).Length;
        }

        public RoutingPoint GoBack(ref List<Vector> list)
        {
            if (LastPoint == null)
                return null;

            list.Add(LastPoint.Point);

            return LastPoint;
        }
    }
}
