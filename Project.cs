using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
// ReSharper disable PossibleMultipleEnumeration

namespace ProjectAnalysis
{

    public class Project
    {
        public int[][] AdjacencyMatrix { get; private set; }
 
        private static void Save(string filename)
        {
            using (var bw = new BinaryWriter(File.Open(filename, FileMode.Create), Encoding.UTF8))
            {
                bw.Write(0x54524550); // "PERT" header
                bw.Write(1);
            }
        }

        public void Load(string filename)
        {
            
        }

        public void New()
        {
            var s = int.Parse(Console.ReadLine());
            AdjacencyMatrix = new int[s][];

            for (int i = 0; i < s; i++)
            {
                var stage = Console.ReadLine().Split(' ').Select(int.Parse);
                var adj = stage.First();
                var edges = stage.Skip(1).ToArray();
                AdjacencyMatrix[i] = new int[s];

                for (var j = 0; j < adj; j++) AdjacencyMatrix[i][edges[2 * j] - 1] = edges[2 * j + 1];
            }

            Console.Write("Save project? [y/n] ");
            var save = Console.ReadLine() == "y";
            if (!save) return;
            Console.Write("Filename: ");
            Save(Console.ReadLine());
        }
    }
}