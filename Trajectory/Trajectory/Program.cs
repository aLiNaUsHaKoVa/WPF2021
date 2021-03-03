using System;
using System.Collections.Generic;
using System.Globalization;

namespace Trajectory
{
    class Program
    {
        static void Main(string[] args)
        {
            var parsedArgs = new CommandLineParser(args)
                .ParseWithFunc(str => float.Parse(str, CultureInfo.InvariantCulture));
            
            var calculator = CreateTrajectoryCalculator(parsedArgs);
            
            PrintPoints(calculator.GetPoints(parsedArgs["interval"]));
        }

        private static TrajectoryCalculator CreateTrajectoryCalculator(Dictionary<string, float> parsedArgs)
        {
            float x0 = 0;
            float y0 = 0;
            if (parsedArgs.TryGetValue("x0", out var startX))
                x0 = startX;
            if (parsedArgs.TryGetValue("y0", out var startY))
                y0 = startY;
            
            return new TrajectoryCalculator(
                parsedArgs["speed"], parsedArgs["angle"], x0, y0);
        }

        private static void PrintPoints(IEnumerable<TrajectoryPoint> points)
        {
            foreach (var point in points)
            {
                Console.WriteLine(point);
            }
        }
    }
}