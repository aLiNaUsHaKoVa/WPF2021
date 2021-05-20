using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp
{
    public class ArgsParser
    {
        private readonly IEnumerable<string> args;

        public ArgsParser(IEnumerable<string> args)
        {
            this.args = args.Select(str => str.ToLower());
        }

        public static Dictionary<string, string> CreateDict(IEnumerable<string> args)
        {
            return new ArgsParser(args).ParseWithFunc(argValue => argValue);
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