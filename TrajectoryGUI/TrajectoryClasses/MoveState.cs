using System.Drawing;

namespace TrajectoryClasses
{
    public class MoveState
    {
        public PointF Coords { get; }
        public MathVector Speed { get; }
        public float TimeInSeconds { get; }

        public MoveState(PointF coords, MathVector speed, float timeInSeconds)
        {
            Coords = coords;
            Speed = speed;
            TimeInSeconds = timeInSeconds;
        }

        public override string ToString()
        {
            return $"Time: {TimeInSeconds}, Coords: {Coords}, Speed: ({Speed.X}, {Speed.Y})";
        }
    }
}