using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map
{
    public enum CosmicUnitKind
    {
        Planet = 1,
        Meteroid,
        Sun,
        MissionTarget,
        Moon,

        Ship = 10,
        Shot,
        Drone,

        Explosion = 20,

        StarShip = 254,
        Unknown = 255
    }
}
