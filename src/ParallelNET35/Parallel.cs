using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ParallelNET35
{
    public static class Parallel
    {
        /// <summary>
        /// Executes a for loop in which iterations may run in parallel.
        /// </summary>
        /// <param name="fromInclusive">The start index, inclusive.</param>
        /// <param name="toExclusive">The end index, exclusive.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        public static void For(int fromInclusive, int toExclusive, Action<int> body)
        {
            int threadCount = Environment.ProcessorCount;
            object locker = new object();
            int currentIndex = fromInclusive;
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
                                if (currentIndex >= toExclusive) return;
                                else
                                {
                                    threadsCurrentIndex = currentIndex;
                                    currentIndex++;
                                }
                            }
                            todo.Invoke(threadsCurrentIndex);
                        }
                    }));
                threads[i].Start(body);
            }

            for (int i = 0; i < threadCount; i++)
            {
                threads[i].Join();
            }
            
        }



        public class ParallelLoopState
        {
            internal bool stop = false;
            internal readonly object locker = new object();

            public void Stop()
            {
                lock (locker)
                {
                    stop = true;
                }
            }

            internal ParallelLoopState() { }
        }


        /// <summary>
        /// Executes a for loop in which iterations may run in parallel.
        /// </summary>
        /// <param name="fromInclusive">The start index, inclusive.</param>
        /// <param name="toExclusive">The end index, exclusive.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        public static void For(int fromInclusive, int toExclusive, Action<int, ParallelLoopState> body)
        {
            int threadCount = Environment.ProcessorCount;
            object locker = new object();
            int currentIndex = fromInclusive;
            Thread[] threads = new Thread[threadCount];
            ParallelLoopState state = new ParallelLoopState();

            for (int i = 0; i < threadCount; i++)
            {
                threads[i] = new Thread(new ParameterizedThreadStart(
                    (block) =>
                    {
                        Action<int, ParallelLoopState> todo = (Action<int, ParallelLoopState>)block;
                        int threadsCurrentIndex;
                        while (true)
                        {
                            if (state.stop) return;
                            lock (locker)
                            {
                                if (currentIndex >= toExclusive) return;
                                else
                                {
                                    threadsCurrentIndex = currentIndex;
                                    currentIndex++;
                                }
                            }
                            todo.Invoke(threadsCurrentIndex, state);
                        }
                    }));
                threads[i].Start(body);
            }

            for (int i = 0; i < threadCount; i++)
            {
                threads[i].Join();
            }

        }
    }
}
