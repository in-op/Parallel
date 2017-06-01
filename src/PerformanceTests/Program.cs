using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

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
            Stopwatch sw = new Stopwatch();

            
            sw.Reset();
            sw.Start();
            ParallelNET35.Parallel.Invoke(Spin, Spin);
            sw.Stop();
            Console.WriteLine("multi-threaded: " + sw.ElapsedMilliseconds);


            sw.Reset();
            sw.Start();
            Spin();
            Spin();
            sw.Stop();
            Console.WriteLine("single-threaded: " + sw.ElapsedMilliseconds);


            Console.ReadLine();
        }
    }
}
