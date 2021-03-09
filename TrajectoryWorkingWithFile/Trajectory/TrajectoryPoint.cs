using System.Drawing;

namespace Trajectory
{
    public class TrajectoryPoint
    {
        public float TimeInSeconds { get; }
        public PointF Coords { get; }

        public TrajectoryPoint(float timeInSeconds, PointF coords)
        {
            TimeInSeconds = timeInSeconds;
            Coords = coords;
        }

        public override string ToString()
        {
            return $"Time: {TimeInSeconds}, Coords: {Coords}";
        }
    }
}