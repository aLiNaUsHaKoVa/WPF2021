using System;

namespace TrajectoryClasses
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
        
        public static MathVector FromMagnitudeAndAngle(float magnitude, float angleInRad)
        {
            var x = magnitude * (float) Math.Cos(angleInRad);
            var y = magnitude * (float) Math.Sin(angleInRad);

            return new MathVector(x, y);
        }
    }
}