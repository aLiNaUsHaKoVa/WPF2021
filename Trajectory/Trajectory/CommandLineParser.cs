using System;
using System.Collections.Generic;
using System.Linq;

namespace Trajectory
{
    public class CommandLineParser
    {
        private string[] args;
        
        public CommandLineParser(string[] args)
        {
            this.args = args;
        }
        
        public Dictionary<string, T> ParseWithFunc<T>(Func<string, T> parseFunc)
        {
            return args
                .Select(namedArg => namedArg.Split("="))
                .ToDictionary(
                    namedArg => namedArg[0].ToLower(),
                    namedArg => parseFunc(namedArg[1]));
        }
    }
}