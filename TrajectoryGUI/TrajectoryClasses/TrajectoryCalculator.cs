using System;
using System.Collections.Generic;
using System.Drawing;

namespace TrajectoryClasses
{
    public class TrajectoryCalculator  // TODO add interface
    {
        private const float G = 9.81f;

        public float StartSpeed { get; }
        public float AngleInRad { get; }
        public PointF StartPoint { get; }
        public float ResistanceCoefficient { get; }
        public float Mass { get; }

        public TrajectoryCalculator( // TODO create builder or configuration class
            PointF startPoint, float startSpeed, float angleInDeg, float mass, float resistanceCoefficient)
        {
            StartSpeed = startSpeed;
            AngleInRad = angleInDeg * (float) Math.PI / 180;
            StartPoint = startPoint;
            ResistanceCoefficient = resistanceCoefficient;
            Mass = mass;
        }

        public Trajectory CalculateTrajectory(float timeIntervalInSeconds)
        {
            return new Trajectory(
                CalculateMoveStates(timeIntervalInSeconds));
        }

        private IEnumerable<MoveState> CalculateMoveStates(float timeIntervalInSeconds)
        {
            var moveState = new MoveState(
                StartPoint,
                MathVector.FromMagnitudeAndAngle(StartSpeed, AngleInRad),
                0.0f);

            while (moveState.Coords.Y != 0 || moveState.TimeInSeconds == 0)
            {
                yield return moveState;
                moveState = CalculateNextMoveState(moveState, timeIntervalInSeconds);
            }
            yield return moveState;
        }

        private MoveState CalculateNextMoveState(MoveState moveState, float dt)
        {
            var nextPoint = CalculateNextPoint(moveState, dt);
            if (nextPoint.Y < 0)
                nextPoint.Y = 0;
            
            var nextSpeed = CalculateNextSpeed(moveState, dt);
            var nextTime = moveState.TimeInSeconds + dt;  // TODO round time or add function in MoveState for this

            return new MoveState(nextPoint, nextSpeed, nextTime);
        }

        private PointF CalculateNextPoint(MoveState moveState, float dt)
        {
            var nextX = moveState.Coords.X + dt * moveState.Speed.X;
            var nextY = moveState.Coords.Y + dt * moveState.Speed.Y;

            return new PointF(
                (float) Math.Round(nextX, 4),
                (float) Math.Round(nextY, 4));  // TODO move rounding functions to MoveState?
        }

        private MathVector CalculateNextSpeed(MoveState moveState, float dt)
        {
            var windImpact = CalculateWindImpact(moveState.TimeInSeconds);
            // TODO fix formula (problem: negative * negative)
            var nextSpeedX = moveState.Speed.X * (1 - dt * windImpact / Mass);
            var nextSpeedY = moveState.Speed.Y - dt * (G + windImpact * moveState.Speed.Y / Mass);

            return new MathVector(nextSpeedX, nextSpeedY);
        }

        private float CalculateWindImpact(float time)
        {
            return ResistanceCoefficient; // todo return ResistanceCoefficient * time + 0.1f;
        }
    }
}