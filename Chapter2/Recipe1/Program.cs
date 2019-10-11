using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using static System.Console;

namespace Recipe1
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine("Correct counter");

            var c1 = new CounterNoLock();

            Thread t1 = new Thread(() => TestCounter(c1));
            Thread t2 = new Thread(() => TestCounter(c1));
            Thread t3 = new Thread(() => TestCounter(c1));
            t1.Start();
            t2.Start();
            t3.Start();
            t1.Join();
            t2.Join();
            t3.Join();

            WriteLine($"Total count: {c1.Count}");
        }

        static void TestCounter(CounterBase c)
        {
            for (int i = 0; i < 100000; i++)
            {
                c.Increment();
                c.Decrement();
            }
        }

        class CounterNoLock : CounterBase
        {
            private int _count;

            public int Count => _count;

            public override void Increment()
            {
                Interlocked.Increment(ref _count);
            }

            public override void Decrement()
            {
                Interlocked.Decrement(ref _count);
            }
        }

        abstract class CounterBase
        {
            public abstract void Increment();

            public abstract void Decrement();
        }
    }
}
