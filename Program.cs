using System;
using System.Linq;

namespace ProjectAnalysis
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine(args);
            var s = int.Parse(Console.ReadLine());
            int[][] adjMat = new int[s][];

            for (int i = 0; i < s; i++)
            {
                var stage = Console.ReadLine().Split(' ').Select(int.Parse);
                var adj = stage.First();
                var edges = stage.Skip(1).ToArray();
                adjMat[i] = new int[s];

                for (int j = 0; j < adj; j++) adjMat[i][0] = 0;
            }

        }
    }
}
