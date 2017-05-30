using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ParallelNET35
{
    class Program
    {
        public static void Main(string[] args)
        {
            Thread t = new Thread(Print);
            t.Start();
            for (int j = -20; j < 0; j++)
                Console.WriteLine(j);
            t.Join();
            Console.WriteLine("Threads done");
            Console.ReadLine();
        }

        public static void Print()
        {
            for (int i = 0; i < 20; i++)
                Console.WriteLine(i);
        }
    }
}
