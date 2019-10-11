using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using static System.Console;
using static System.Threading.Thread;

namespace Recipe8
{
    class Program
    {
        static void Main(string[] args)
        {
            // 类方法
            var sample = new ThreadSample(10);
            var threadOne = new Thread(sample.CountNumbers);
            threadOne.Name = "threadOne";
            threadOne.Start();
            threadOne.Join();

            WriteLine("--------------------------------------");

            // 接收object类型参数的方法
            var threadTwo = new Thread(Count);
            threadTwo.Name = "threadTwo";
            threadTwo.Start(8);
            threadTwo.Join();

            WriteLine("--------------------------------------");

            // lambda 表达式
            var threadThree = new Thread(() => CountNumbers(12));
            threadThree.Name = "threadThree";
            threadThree.Start();
            threadThree.Join();

            WriteLine("--------------------------------------");

            // 带局部变量的lambda表达式
            int i = 10;
            var threadFour = new Thread(() => PrintNumbers(i));
            threadFour.Name = "threadFour";
            threadFour.Start();
            i = 20;
            var threadFive = new Thread(() => PrintNumbers(i));
            threadFive.Start();
        }

        static void Count(object iterations)
        {
            CountNumbers((int)iterations);
        }

        static void CountNumbers(int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                Sleep(TimeSpan.FromSeconds(0.5));
                WriteLine($"{CurrentThread.Name} prints {i}");
            }
        }

        static void PrintNumbers(int number)
        {
            WriteLine(number);
        }

        class ThreadSample
        {
            private readonly int _iterations;

            public ThreadSample(int iterations)
            {
                _iterations = iterations;
            }

            public void CountNumbers()
            {
                for (int i = 0; i < _iterations; i++)
                {
                    Sleep(TimeSpan.FromSeconds(0.5));
                    WriteLine($"{CurrentThread.Name} prints {i}");
                }
            }
        }
    }
}
