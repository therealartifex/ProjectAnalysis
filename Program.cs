using System;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;

// ReSharper disable PossibleNullReferenceException
// ReSharper disable PossibleMultipleEnumeration

namespace ProjectAnalysis
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var proj = new Project();

            switch (args[0])
            {
                case "new":
                    proj.New();
                    break;
                case "load":
                    proj.Load(args[1]);
                    break;
                default:
                    Environment.Exit(0);
                    break;
            }

            var wg = new WDAG(adjMat);
            Console.ReadKey();
        }
    }
}
