using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using static System.Console;
using static System.Threading.Thread;

namespace Recipe3
{
    class Program
    {
        static void Main(string[] args)
        {
            const int numberOfOperations = 500;

            var sw = new Stopwatch();
            sw.Start();
            UseThreads(numberOfOperations);
            sw.Stop();

            WriteLine($"Execution time using threads: {sw.ElapsedMilliseconds}");

            sw.Reset();
            sw.Start();
            UseThreadPool(numberOfOperations);
            sw.Stop();
            WriteLine($"Execution time using the thread pool: {sw.ElapsedMilliseconds}");
        }

        static void UseThreads(int numberOfOpeartions)
        {
            using (var countdown = new CountdownEvent(numberOfOpeartions))
            {
                WriteLine("Scheduling work by creating threads");
                for (int i = 0; i < numberOfOpeartions; i++)
                {
                    var thread = new Thread(() =>
                    {
                        Write($"{CurrentThread.ManagedThreadId}, ");
                        Sleep(TimeSpan.FromSeconds(0.1));
                        countdown.Signal();
                    });
                    thread.Start();
                }
                countdown.Wait();
            }
        }

        static void UseThreadPool(int numberOfOperations)
        {
            using (var countdown = new CountdownEvent(numberOfOperations))
            {
                WriteLine("Starting work on a threadpool");
                for (int i = 0; i < numberOfOperations; i++)
                {
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        Write($"{CurrentThread.ManagedThreadId}, ");
                        Sleep(TimeSpan.FromSeconds(0.1));
                        countdown.Signal();
                    });
                }
                countdown.Wait();
                WriteLine();
            }
        }
    }
}
