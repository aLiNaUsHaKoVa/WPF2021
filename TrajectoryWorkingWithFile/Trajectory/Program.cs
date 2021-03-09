using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Trajectory
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new CommandLineParser(args);

            var parsedArgs = GetParsedArgs(parser.GetArgValueByName("source"));
            
            var points = CreateTrajectoryCalculator(parsedArgs)
                .GetPoints(parsedArgs["interval"]);

            File.WriteAllLines(
                parser.GetArgValueByName("destination"),
                points.Select(point => point.ToString()));
        }

        private static Dictionary<string, float> GetParsedArgs(string sourceFilePath)
        {
            var parser = new CommandLineParser(File.ReadLines(sourceFilePath));
            return parser.ParseWithFunc(
                str => float.Parse(str, CultureInfo.InvariantCulture));
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
    }
}