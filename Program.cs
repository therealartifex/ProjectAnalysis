using System;
using System.Linq;

// ReSharper disable PossibleMultipleEnumeration
// ReSharper disable PossibleNullReferenceException
// ReSharper disable AssignNullToNotNullAttribute

namespace ProjectAnalysis
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            var s = int.Parse(Console.ReadLine());
            var am = new int[s][];

            for (var i = 0; i < s; i++)
            {
                var stage = Console.ReadLine().Split(' ').Select(int.Parse);
                var adj = stage.First();
                var edges = stage.Skip(1).ToArray();
                am[i] = new int[s];

                for (var j = 0; j < adj; j++) am[i][edges[2 * j] - 1] = edges[2 * j + 1];
            }
            Console.Clear();
            var proj = new Project(am);


            if (proj.IsFeasible)
            {
                Console.Write($"Feasible.\nTopological ordering: [{string.Join(" ", proj.TopologicalOrder)}]\n\nStg\tEar\tLat\n");
                for (var k = 0; k < proj.EarlyStage.Length; k++) Console.Write($"{k+1}\t{proj.EarlyStage[k]}\t{proj.LateStage[k]}\n");
                Console.Write($"Total project time: {proj.EarlyStage.Last()}\n\nAct\tEar\tLat\n");

                for (var l = 0; l < proj.EarlyActivity.Length; l++) Console.Write($"{l+1}\t{proj.EarlyActivity[l]}\t{proj.LateActivity[l]}\n");

                Console.Write($"\nCritical activities: [{string.Join(" ", proj.CriticalActivities)}]\n");

            }




            /*
            Console.Write("Save project? [y/n] ");
            var save = Console.ReadLine() == "y";
            if (!save) return;
            Console.Write("Filename: ");
            Save(Console.ReadLine());
            */

            Console.ReadKey();
        }
        
        //private static void Save(string filename)
        //{
        //    using (var bw = new BinaryWriter(File.Open(filename, FileMode.Create), Encoding.UTF8))
        //    {
        //        bw.Write(0x54524550); // "PERT" header
        //        bw.Write(1);
        //    }
        //}

    }
}
