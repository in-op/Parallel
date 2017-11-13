using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParallelNET35;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelNET35.Tests
{
    [TestClass()]
    public class ParallelTests
    {
        [TestMethod()]
        public void ForCorrectlyFillsArrayWithAValue()
        {
            const byte item = 42;
            const int size = 10000;
            byte[] bytes = new byte[size];

            Parallel.For(0, size, i => bytes[i] = item);

            for (int i = 0; i < size; i++)
                if (bytes[i] != item)
                    Assert.Fail();
        }

        [TestMethod()]
        public void ForCorrectlyUpdatesArrayValues()
        {
            const byte item = 0;
            const int size = 10000;
            byte[] bytes = new byte[size];
            for (int i = 0; i < size; i++)
                bytes[i] = item;

            Parallel.For(0, size, i => bytes[i] += 1);

            for (int i = 0; i < size; i++)
                if (bytes[i] != item + 1)
                    Assert.Fail();
        }

        [TestMethod()]
        public void ForWithStateCorrectlyFillsArrayWithAValue()
        {
            const byte item = 42;
            const int size = 1000;
            byte[] bytes = new byte[size];
            Parallel.For(0, size, (i, state) => bytes[i] = item);

            for (int i = 0; i < size; i++)
                if (bytes[i] != item)
                    Assert.Fail();
        }
    }
}