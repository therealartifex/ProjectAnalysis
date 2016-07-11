using System;
using System.Collections.Generic;

namespace ProjectAnalysis
{
    public class AcyclicDigraph
    {
        public List<int> TopologicalOrder { get; } = new List<int>();
        public int[][] AdjacencyMatrix { get; }
        public int[] PredecessorCount { get; }
        public int Size { get; }

        public AcyclicDigraph(int[][] matrix)
        {
            AdjacencyMatrix = matrix;
            Size = AdjacencyMatrix.GetLength(0);
            PredecessorCount = new int[Size];
            
            for (var i = 0; i < Size; i++) for (var j = 0; j < Size; j++) if (AdjacencyMatrix[j][i] > 0) PredecessorCount[i]++;
            
            // Determine topological ordering
            var temp = new Stack<int>();
            for (var l = 0; l < Size; l++)
            {
                for (var i = 1; i <= Size; i++) if (PredecessorCount[i - 1] == 0 & !TopologicalOrder.Contains(i)) temp.Push(i);

                while (temp.Count > 0)
                {
                    var currentStage = temp.Pop();
                    var edges = AdjacencyMatrix[currentStage - 1];
                    TopologicalOrder.Add(currentStage);

                    for (var i = 0; i < edges.Length; i++) if (edges[i] > 0 & PredecessorCount[i] > 0) PredecessorCount[i]--;
                }
            }

            // Cycle check
            if (TopologicalOrder.Count < Size) throw new ArgumentException("Graph contains one or more cycles.");
        }
    }
}
