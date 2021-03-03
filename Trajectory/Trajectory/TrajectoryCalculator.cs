using System;
using System.Collections.Generic;
using System.Drawing;

namespace Trajectory
{
    public class TrajectoryCalculator
    {
        public float Speed { get; }
        public float AngleInRad { get; }
        public float X0 { get; }
        public float Y0 { get; }
        
        private const float G = 9.81f;

        public TrajectoryCalculator(
            float speed, float angleInDeg, float x0 = 0, float y0 = 0)
        {
            Speed = speed;
            AngleInRad = angleInDeg * (float)Math.PI/180;
            X0 = x0;
            Y0 = y0;
        }

        public IEnumerable<TrajectoryPoint> GetPoints(float timeIntervalInSeconds)
        {
            var currentTime = 0.0f;
            var nextPoint = new PointF(X0, Y0);
            
            while (nextPoint.Y >= 0)
            {
                yield return new TrajectoryPoint(currentTime, nextPoint);
                currentTime += timeIntervalInSeconds;
                nextPoint = CalculatePoint(currentTime);
            }
        }

        private PointF CalculatePoint(float time)
        {
            return new PointF(
                CalculateX(time),
                CalculateY(time));
        }

        private float CalculateX(float time)
        {
            var x = X0 + Speed * Math.Cos(AngleInRad) * time;
            return (float)Math.Round(x, 4);
        }

        private float CalculateY(float time)
        {
            var y = Y0 + Speed * Math.Sin(AngleInRad) * time - G*time*time/2;
            return (float)Math.Round(y, 4);
        }
    }
}