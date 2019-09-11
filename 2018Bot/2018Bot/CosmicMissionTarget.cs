using Flattiverse;
using System.Drawing;

namespace _2018Bot
{
    internal class CosmicMissionTarget : CosmicUnit
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