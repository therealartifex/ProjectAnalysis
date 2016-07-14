using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectAnalysis
{
    public class Project
    {
        public List<int> TopologicalOrder { get; } = new List<int>();
        public int[] EarlyStage { get; }
        public int[] LateStage { get; }
        public int[] EarlyActivity { get; }
        public int[] LateActivity { get; }
        public bool IsFeasible { get; }
        private readonly int Size;
        private readonly int[][] AdjacencyMatrix;
        
        public Project(int[][] mat)
        {
            AdjacencyMatrix = mat;
            Size = AdjacencyMatrix.GetLength(0);
            var predecessors = new int[Size];
                        
            EarlyStage = new int[Size];
            LateStage = new int[Size];

            for (var i = 0; i < Size; i++) for (var j = 0; j < Size; j++) if (AdjacencyMatrix[j][i] > 0) predecessors[i]++;
            EarlyActivity = new int[predecessors.Sum()];
            LateActivity = new int[predecessors.Sum()];

            // Determine topological ordering (Kahn's algorithm)
            var temp = new Stack<int>();
            for (var l = 0; l < Size; l++)
            {
                for (var i = 1; i <= Size; i++) if (predecessors[i - 1] == 0 & !TopologicalOrder.Contains(i)) temp.Push(i);

                while (temp.Count > 0)
                {
                    var currentStage = temp.Pop();
                    var edges = AdjacencyMatrix[currentStage - 1];
                    TopologicalOrder.Add(currentStage);

                    for (var i = 0; i < edges.Length; i++) if (edges[i] > 0 & predecessors[i] > 0) predecessors[i]--;
                }
            }

            // Cycle check
            try
            {
                if (TopologicalOrder.Count < Size) throw new ArgumentException("Graph contains one or more cycles.");
                IsFeasible = true;
            }
            catch (ArgumentException)
            {
                IsFeasible = false;
                return;
            }


            // Determine early/late stage times
            for (var i = 0; i < Size; i++) EarlyStage[i] = EST(i + 1);
            for (var i = Size - 1; i >= 0; i--) LateStage[i] = LST(i + 1);

            // Determine early/late activity times
            var outgoing = new List<int>();
            var incoming = new List<int>();
            var costs = new List<int>();

            for (var i = 0; i < Size; i++)
                for (var j = 0; j < Size; j++)
                    if (AdjacencyMatrix[i][j] > 0)
                    {
                        outgoing.Add(i + 1);
                        incoming.Add(j + 1);
                        costs.Add(AdjacencyMatrix[i][j]);
                    }

            for (var k = 0; k < outgoing.Count; k++)
            {
                EarlyActivity[k] = EarlyStage[outgoing[k] - 1];
                LateActivity[k] = LST(incoming[k]) - costs[k];
            }
        }

        private int EST(int stage)
        {
            var cost = new List<int>();
            var incoming = new List<int>();
            for (var i = 1; i <= Size; i++) if (AdjacencyMatrix[i - 1][stage - 1] > 0) incoming.Add(i);

            if (!incoming.Any()) return 0;
            cost.AddRange(incoming.Select(s => AdjacencyMatrix[s - 1][stage - 1] + EST(s)).ToList());

            return cost.AsParallel().Max();
        }

        private int LST(int stage)
        {
            var cost = new List<int>();
            var outgoing = new List<int>();
            for (var i = 1; i <= Size; i++) if (AdjacencyMatrix[stage - 1][i - 1] > 0) outgoing.Add(i);

            if (!outgoing.Any()) return EarlyStage[stage - 1];
            cost.AddRange(outgoing.Select(s => LST(s) - AdjacencyMatrix[stage - 1][s - 1]).ToList());

            return cost.AsParallel().Min();
        }
    }
}