using System.Drawing;

namespace Trajectory
{
    public class MoveState
    {
        public PointF Coords { get; }
        public MathVector Speed { get; }
        public float Time { get; }

        public MoveState(PointF coords, MathVector speed, float time)
        {
            Coords = coords;
            Speed = speed;
            Time = time;
        }
    }
}