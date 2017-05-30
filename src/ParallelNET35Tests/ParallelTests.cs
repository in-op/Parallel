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
            byte comparison = 42;
            int size = 1000;
            byte[] bytes = new byte[size];
            Parallel.For(0, size, (i) =>
            {
                bytes[i] = comparison;
            });

            for (int i = 0; i < size; i++)
                if (bytes[i] != comparison)
                    Assert.Fail();
        }

        [TestMethod()]
        public void ForWithStateCorrectlyFillsArrayWithAValue()
        {
            byte comparison = 42;
            int size = 1000;
            byte[] bytes = new byte[size];
            Parallel.For(0, size, (i, state) =>
            {
                bytes[i] = comparison;
            });

            for (int i = 0; i < size; i++)
                if (bytes[i] != comparison)
                    Assert.Fail();
        }
    }
}