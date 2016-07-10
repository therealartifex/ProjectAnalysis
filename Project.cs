using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ProjectAnalysis
{
    public struct Stage
    {
        public int AdjacentStage, Cost;
        public Stage(int i, int j)
        {
            AdjacentStage = i;
            Cost = j;
        }
    }

    public class Project
    {
        private readonly List<Stage[]> stages;
        public Project(List<Stage[]> sl)
        {
            stages = sl;
        }

        public void Make(string filename)
        {
            using (var bw = new BinaryWriter(File.Open(filename, FileMode.Create), Encoding.UTF8))
            {
                bw.Write(0x54524550); // "PERT" header
                bw.Write(1);
            }
        }
    }
}