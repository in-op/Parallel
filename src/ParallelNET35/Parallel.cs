using System;
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
            Thread[] threads = new Thread[threadCount];
            --fromInclusive;

            Action a = () =>
            {
                int index;
                while (true)
                {
                    index = Interlocked.Increment(ref fromInclusive);
                    if (index >= toExclusive) return;
                    body.Invoke(index);
                }
            };

            for (int i = 0; i < threadCount; i++)
            {
                threads[i] = new Thread(new ThreadStart(a));
                threads[i].Start();
            }

            for (int i = 0; i < threadCount; i++)
            {
                threads[i].Join();
            }
            
        }


        



        public class ParallelLoopState
        {
            private bool stopTriggered = false;
            internal bool StopTriggered
            {
                get
                {
                    return stopTriggered;
                }

                private set
                {
                    stopTriggered = value;
                }
            }

            public void Stop()
            {
                StopTriggered = true;
            }

            internal ParallelLoopState() { }
        }


        /// <summary>
        /// Executes a for loop in which iterations may run in parallel and the state of the loop may be monitored and manipulated.
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
                            if (state.StopTriggered) return;
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


        /// <summary>
        /// Executes a for loop with 64-bit indexes in which iterations may run in parallel.
        /// </summary>
        /// <param name="fromInclusive">The start index, inclusive.</param>
        /// <param name="toExclusive">The end index, exclusive.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        public static void For(long fromInclusive, long toExclusive, Action<long> body)
        {
            int threadCount = Environment.ProcessorCount;
            object locker = new object();
            long currentIndex = fromInclusive;
            Thread[] threads = new Thread[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                threads[i] = new Thread(new ParameterizedThreadStart(
                    (block) =>
                    {
                        Action<long> todo = (Action<long>)block;
                        long threadsCurrentIndex;
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





        /// <summary>
        /// Executes a for loop with 64-bit indexes in which iterations may run in parallel and the state of the loop may be monitored and manipulated.
        /// </summary>
        /// <param name="fromInclusive">The start index, inclusive.</param>
        /// <param name="toExclusive">The end index, exclusive.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        public static void For(long fromInclusive, long toExclusive, Action<long, ParallelLoopState> body)
        {
            int threadCount = Environment.ProcessorCount;
            object locker = new object();
            long currentIndex = fromInclusive;
            Thread[] threads = new Thread[threadCount];
            ParallelLoopState state = new ParallelLoopState();

            for (int i = 0; i < threadCount; i++)
            {
                threads[i] = new Thread(new ParameterizedThreadStart(
                    (block) =>
                    {
                        Action<long, ParallelLoopState> todo = (Action<long, ParallelLoopState>)block;
                        long threadsCurrentIndex;
                        while (true)
                        {
                            if (state.StopTriggered) return;
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







        /// <summary>
        /// Executes a for loop with thread-local data in which iterations may run in parallel, and the state of the loop may be monitored and manipulated.
        /// </summary>
        /// <param name="fromInclusive">The start index, inclusive.</param>
        /// <param name="toExclusive">The end index, exclusive.</param>
        /// <param name="localInit">The function delegate that returns the initial state of the local data for each task.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="localFinally">The delegate that performs a final action on the local state of each task.</param>
        /// <typeparam name="TLocal">The type of the thread-local data.</typeparam>
        public static void For<TLocal>(
            int fromInclusive,
            int toExclusive,
            Func<TLocal> localInit,
            Func<int, ParallelLoopState, TLocal, TLocal> body,
            Action<TLocal> localFinally)
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
                        TLocal localVars = localInit.Invoke();
                        Func<int, ParallelLoopState, TLocal, TLocal> todo = (Func<int, ParallelLoopState, TLocal, TLocal>)block;
                        int threadsCurrentIndex;
                        while (true)
                        {
                            if (state.StopTriggered) break;
                            lock (locker)
                            {
                                if (currentIndex >= toExclusive) break;
                                else
                                {
                                    threadsCurrentIndex = currentIndex;
                                    currentIndex++;
                                }
                            }
                            localVars = todo.Invoke(threadsCurrentIndex, state, localVars);
                        }
                        localFinally.Invoke(localVars);
                    }));
                threads[i].Start(body);
            }

            for (int i = 0; i < threadCount; i++)
            {
                threads[i].Join();
            }

        }

        /// <summary>
        /// Executes a for loop with thread-local data in which iterations may run in parallel, and the state of the loop may be monitored and manipulated.
        /// </summary>
        /// <param name="fromInclusive">The start index, inclusive.</param>
        /// <param name="toExclusive">The end index, exclusive.</param>
        /// <param name="localInit">The function delegate that returns the initial state of the local data for each task.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="localFinally">The delegate that performs a final action on the local state of each task.</param>
        /// <typeparam name="TLocal">The type of the thread-local data.</typeparam>
        public static void For<TLocal>(
            long fromInclusive,
            long toExclusive,
            Func<TLocal> localInit,
            Func<long, ParallelLoopState, TLocal, TLocal> body,
            Action<TLocal> localFinally)
        {
            int threadCount = Environment.ProcessorCount;
            object locker = new object();
            long currentIndex = fromInclusive;
            Thread[] threads = new Thread[threadCount];
            ParallelLoopState state = new ParallelLoopState();

            for (int i = 0; i < threadCount; i++)
            {
                threads[i] = new Thread(new ParameterizedThreadStart(
                    (block) =>
                    {
                        TLocal localVars = localInit.Invoke();
                        Func<long, ParallelLoopState, TLocal, TLocal> todo = (Func<long, ParallelLoopState, TLocal, TLocal>)block;
                        long threadsCurrentIndex;
                        while (true)
                        {
                            if (state.StopTriggered) break;
                            lock (locker)
                            {
                                if (currentIndex >= toExclusive) break;
                                else
                                {
                                    threadsCurrentIndex = currentIndex;
                                    currentIndex++;
                                }
                            }
                            localVars = todo.Invoke(threadsCurrentIndex, state, localVars);
                        }
                        localFinally.Invoke(localVars);
                    }));
                threads[i].Start(body);
            }

            for (int i = 0; i < threadCount; i++)
            {
                threads[i].Join();
            }

        }



        /// <summary>
        /// Executes each action in parallel.
        /// </summary>
        /// <param name="actions">The set of delegates to invoke.</param>
        public static void Invoke(params Action[] actions)
        {
            int actionCount = actions.Length;
            Thread[] threads = new Thread[actionCount];

            for (int i = 0; i < actionCount; i++)
            {
                threads[i] = new Thread(new ThreadStart(actions[i]));
                threads[i].Start();
            }
            for (int i = 0; i < actionCount; i++)
            {
                threads[i].Join();
            }
        }







    }
}
