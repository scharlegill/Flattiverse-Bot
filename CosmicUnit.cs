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
    }
}
