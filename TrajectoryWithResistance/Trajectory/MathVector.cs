using System;

namespace Trajectory
{
    public class MathVector
    {
        public float X { get; }
        public float Y { get; }

        public MathVector(float x, float y)
        {
            X = x;
            Y = y;
        }
        
        public static MathVector CreateFromMagnitudeAndAngle(float magnitude, float angleInRad)
        {
            var x = magnitude * (float) Math.Cos(angleInRad);
            var y = magnitude * (float) Math.Sin(angleInRad);

            return new MathVector(x, y);
        }
    }
}