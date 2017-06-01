using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Performance;

namespace PerformanceTests
{
    class Program
    {
        static void Spin()
        {
            for (int i = 0; i < 10000000; i++)
            {
                int j = i;
            }
        }

        static void Main(string[] args)
        {
            Timer.RunAndDisplay(0, 1,
                () => { ParallelNET35.Parallel.Invoke(Spin, Spin); },
                () => { Spin(); Spin(); });
        }
    }
}
