using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectAnalysis
{
    public class WDAG
    {
        public bool IsFeasible { get; }
        public List<int> TopologicalOrder { get; } = new List<int>();
        public int[] EarlyStage { get; }
        public int[] LateStage { get; }
        public int[] EarlyActivity { get; }
        public int[] LateActivity { get; }
        private readonly int[][] AdjacencyMatrix;

        public WDAG(int[][] matrix)
        {
            AdjacencyMatrix = matrix;
            var len = AdjacencyMatrix.GetLength(0);
            var predCount = new int[len];
            EarlyStage = new int[len];
            LateStage = new int[len];
            var temp = new Stack<int>();

            for (var i = 0; i < len; i++) for (var j = 0; j < len; j++) if (AdjacencyMatrix[j][i] > 0) predCount[i]++;
            EarlyActivity = new int[predCount.Sum()];
            LateActivity = new int[predCount.Sum()];

            // Determine topological ordering
            for (var l = 0; l < len; l++)
            {
                for (var i = 1; i <= len; i++) if (predCount[i - 1] == 0 & !TopologicalOrder.Contains(i)) temp.Push(i);

                while (temp.Count > 0)
                {
                    var currentStage = temp.Pop();
                    var edges = AdjacencyMatrix[currentStage - 1];
                    TopologicalOrder.Add(currentStage);

                    for (var i = 0; i < edges.Length; i++) if (edges[i] > 0 & predCount[i] > 0) predCount[i]--;
                }
            }

            // Feasibility check
            try
            {
                if (TopologicalOrder.Count < len) throw new ArgumentException("Project is infeasible.");
                IsFeasible = true;
            }
            catch (Exception e)
            {
                Console.Error.Write($"{e.Message}\n");
            }


            // Determine early/late stage times
            for (var i = 0; i < len; i++) EarlyStage[i] = EST(i+1);
            for (var i = len - 1; i >= 0; i--) LateStage[i] = LST(i+1);

            // Determine early/late activity times
            var outgoing = new List<int>();
            var incoming = new List<int>();
            var costs = new List<int>();

            for (var i = 0; i < len; i++)
                for (var j = 0; j < len; j++)
                    if (AdjacencyMatrix[i][j] > 0)
                    {
                        outgoing.Add(i + 1);
                        incoming.Add(j + 1);
                        costs.Add(AdjacencyMatrix[i][j]);
                    }

            for (var k = 0; k < outgoing.Count; k++)
            {
                EarlyActivity[k] = EarlyStage[outgoing[k]-1];
                LateActivity[k] = LST(incoming[k]) - costs[k];
            }
        }

        // This uses recursion to get the early stage time for an arbitrary stage
        private int EST(int stage)
        {
            var len = AdjacencyMatrix.GetLength(0);
            var cost = new List<int>();
            var incoming = new List<int>();
            for (var i = 1; i <= len; i++) if (AdjacencyMatrix[i-1][stage-1] > 0) incoming.Add(i);

            if (!incoming.Any()) return 0;
            cost.AddRange(incoming.AsParallel().Select(s => AdjacencyMatrix[s-1][stage-1] + EST(s)).ToList());

            return cost.AsParallel().Max();
        }

        // This uses recursion to get the late stage time for an arbitrary stage
        private int LST(int stage)
        {
            var len = AdjacencyMatrix.GetLength(0);
            var cost = new List<int>();
            var outgoing = new List<int>();
            for (var i = 1; i <= len; i++) if (AdjacencyMatrix[stage-1][i-1] > 0) outgoing.Add(i);

            if (!outgoing.Any()) return EarlyStage[stage-1];
            cost.AddRange(outgoing.AsParallel().Select(s => LST(s) - AdjacencyMatrix[stage-1][s-1]).ToList());

            return cost.AsParallel().Min();
        }
    }
}
