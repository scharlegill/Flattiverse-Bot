using Flattiverse;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map
{
    public class CosmicMissionTarget : CosmicUnit
    {
        public readonly Vector Direction;

        public Color Color;

        public readonly int Number;

        public readonly float DominationRadius;

        public int DominationTicks;

        public CosmicMissionTarget(Unit unit) : base(unit)
        {
            Direction = new Vector();

            Type = CosmicUnitKind.MissionTarget;

            foreach (Vector hint in ((MissionTarget)unit).Hints)
                Direction = hint;

            Color = ((MissionTarget)unit).Team.Color;

            Number = ((MissionTarget)unit).SequenceNumber;

            DominationRadius = ((MissionTarget)unit).DominationRadius;

            DominationTicks = ((MissionTarget)unit).DominationTicks;
        }
    }
}
