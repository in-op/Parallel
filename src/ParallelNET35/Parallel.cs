using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ParallelNET35
{
    public static class Parallel
    {
        public static void For(int start, int stop, Action<int> loopBlock)
        {
            int threadCount = Environment.ProcessorCount;
            object locker = new object();
            int currentIndex = start;
            Thread[] threads = new Thread[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                threads[i] = new Thread(new ParameterizedThreadStart(
                    (block) =>
                    {
                        Action<int> todo = (Action<int>)block;
                        int threadsCurrentIndex;
                        while (true)
                        {
                            lock (locker)
                            {
                                if (currentIndex >= stop) return;
                                else
                                {
                                    threadsCurrentIndex = currentIndex;
                                    currentIndex++;
                                }
                            }
                            todo.Invoke(threadsCurrentIndex);
                        }
                    }));
                threads[i].Start(loopBlock);
            }


            for (int i = 0; i < threadCount; i++)
            {
                threads[i].Join();
            }


        }
    }
}
