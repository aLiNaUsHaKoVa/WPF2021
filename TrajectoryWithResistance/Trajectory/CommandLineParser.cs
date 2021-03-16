using System;
using System.Collections.Generic;
using System.Linq;

namespace Trajectory
{
    public class CommandLineParser
    {
        private readonly IEnumerable<string> args;

        public CommandLineParser(IEnumerable<string> args)
        {
            this.args = args.Select(str => str.ToLower());
        }

        public Dictionary<string, T> ParseWithFunc<T>(Func<string, T> parseFunc)
        {
            return args
                .Select(namedArg => namedArg.Split("="))
                .ToDictionary(
                    namedArg => namedArg[0],
                    namedArg => parseFunc(namedArg[1]));
        }

        public string GetArgValueByName(string argName)
        {
            return args
                .First(arg => arg.StartsWith(argName))
                .Split("=")[1];
        }
    }
}