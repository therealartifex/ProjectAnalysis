using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectAnalysis
{
    public class Project
    {
        private readonly AcyclicDigraph stages;
        public int[] EarlyStage { get; }
        public int[] LateStage { get; }
        public int[] EarlyActivity { get; }
        public int[] LateActivity { get; }
        public bool IsFeasible { get; }

        public Project(int[][] mat)
        {
            try
            {
                stages = new AcyclicDigraph(mat);
                IsFeasible = true;
            }
            catch (ArgumentException e)
            {
                return;
            }
            
            EarlyStage = new int[stages.Size];
            LateStage = new int[stages.Size];
            EarlyActivity = new int[stages.PredecessorCount.Sum()];
            LateActivity = new int[stages.PredecessorCount.Sum()];

            // Determine early/late stage times
            for (var i = 0; i < stages.Size; i++) EarlyStage[i] = EST(i + 1);
            for (var i = stages.Size - 1; i >= 0; i--) LateStage[i] = LST(i + 1);

            // Determine early/late activity times
            var outgoing = new List<int>();
            var incoming = new List<int>();
            var costs = new List<int>();

            for (var i = 0; i < stages.Size; i++)
                for (var j = 0; j < stages.Size; j++)
                    if (stages.AdjacencyMatrix[i][j] > 0)
                    {
                        outgoing.Add(i + 1);
                        incoming.Add(j + 1);
                        costs.Add(stages.AdjacencyMatrix[i][j]);
                    }

            for (var k = 0; k < outgoing.Count; k++)
            {
                EarlyActivity[k] = EarlyStage[outgoing[k] - 1];
                LateActivity[k] = LST(incoming[k]) - costs[k];
            }
        }

        private int EST(int stage)
        {
            var Size = stages.AdjacencyMatrix.GetLength(0);
            var cost = new List<int>();
            var incoming = new List<int>();
            for (var i = 1; i <= Size; i++) if (stages.AdjacencyMatrix[i - 1][stage - 1] > 0) incoming.Add(i);

            if (!incoming.Any()) return 0;
            cost.AddRange(incoming.AsParallel().Select(s => stages.AdjacencyMatrix[s - 1][stage - 1] + EST(s)).ToList());

            return cost.AsParallel().Max();
        }

        private int LST(int stage)
        {
            var Size = stages.AdjacencyMatrix.GetLength(0);
            var cost = new List<int>();
            var outgoing = new List<int>();
            for (var i = 1; i <= Size; i++) if (stages.AdjacencyMatrix[stage - 1][i - 1] > 0) outgoing.Add(i);

            if (!outgoing.Any()) return EarlyStage[stage - 1];
            cost.AddRange(outgoing.AsParallel().Select(s => LST(s) - stages.AdjacencyMatrix[stage - 1][s - 1]).ToList());

            return cost.AsParallel().Min();
        }
    }
}