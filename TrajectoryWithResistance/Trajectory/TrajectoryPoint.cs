using System.Drawing;

namespace Trajectory
{
    public class TrajectoryPoint
    {
        public TrajectoryPoint(float timeInSeconds, PointF coords)
        {
            TimeInSeconds = timeInSeconds;
            Coords = coords;
        }

        public float TimeInSeconds { get; }
        public PointF Coords { get; }

        public override string ToString()
        {
            return $"Time: {TimeInSeconds}, Coords: {Coords}";
        }
    }
}