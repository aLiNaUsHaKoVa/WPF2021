using System;
using System.Collections.Generic;
using System.Drawing;

namespace Trajectory
{
    public class TrajectoryCalculatorWithResistance  
    {
        private const float G = 9.81f;

        public float StartSpeed { get; }
        public float AngleInRad { get; }
        public PointF StartPoint { get; }
        public float ResistanceCoefficient { get; }
        public float Mass { get; }

        public TrajectoryCalculatorWithResistance( 
            PointF startPoint, float startSpeed, float angleInDeg, float mass, float resistanceCoefficient)
        {
            StartSpeed = startSpeed;
            AngleInRad = angleInDeg * (float) Math.PI / 180;
            StartPoint = startPoint;
            ResistanceCoefficient = resistanceCoefficient;
            Mass = mass;
        }

        public IEnumerable<TrajectoryPoint> GetPoints(float timeIntervalInSeconds)
        {
            var moveState = new MoveState(
                StartPoint,
                MathVector.CreateFromMagnitudeAndAngle(StartSpeed, AngleInRad),
                0.0f);

            while (moveState.Coords.Y != 0 || moveState.Time == 0)
            {
                yield return new TrajectoryPoint(moveState.Time, moveState.Coords);
                moveState = CalculateNextMoveState(moveState, timeIntervalInSeconds);
            }
            yield return new TrajectoryPoint(moveState.Time, moveState.Coords);
        }

        private MoveState CalculateNextMoveState(MoveState moveState, float dt)
        {
            var nextPoint = CalculateNextPoint(moveState, dt);
            if (nextPoint.Y < 0)
                nextPoint.Y = 0;
            
            var nextSpeed = CalculateNextSpeed(moveState, dt);
            var nextTime = moveState.Time + dt;  

            return new MoveState(nextPoint, nextSpeed, nextTime);
        }

        private PointF CalculateNextPoint(MoveState moveState, float dt)
        {
            var nextX = moveState.Coords.X + dt * moveState.Speed.X;
            var nextY = moveState.Coords.Y + dt * moveState.Speed.Y;

            return new PointF(
                (float) Math.Round(nextX, 4),
                (float) Math.Round(nextY, 4));  
        }

        private MathVector CalculateNextSpeed(MoveState moveState, float dt)
        {
            var windImpact = CalculateWindImpact(moveState.Time);
            var nextSpeedX = moveState.Speed.X * (1 - dt * windImpact / Mass);
            var nextSpeedY = moveState.Speed.Y - dt * (G + windImpact * moveState.Speed.Y / Mass);

            return new MathVector(nextSpeedX, nextSpeedY);
        }

        private float CalculateWindImpact(float time)
        { 
            return ResistanceCoefficient * time + 0.1f;
        }
    }
}