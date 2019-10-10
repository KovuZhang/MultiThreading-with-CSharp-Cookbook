using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using static System.Console;
using static System.Threading.Thread;

namespace Recipe1
{
    class Program
    {
        static void Main(string[] args)
        {
            Parallel.Invoke(
                () => EmulateProcessing("Task 1"),
                () => EmulateProcessing("Task 2"),
                () => EmulateProcessing("Task 3")
            );

            var cts = new CancellationTokenSource();
            var result = Parallel.ForEach(
                Enumerable.Range(1, 10),
                new ParallelOptions
                {
                    CancellationToken = cts.Token,
                    MaxDegreeOfParallelism = Environment.ProcessorCount,
                    TaskScheduler = TaskScheduler.Default
                },
                (i, state) =>
                {
                    WriteLine(i);
                    if (i == 5)
                    {
                        state.Break();
                        WriteLine($"Loop is stopped: {state.IsStopped}");
                    }
                }
            );

            WriteLine("-----");
            WriteLine($"IsCompleted: {result.IsCompleted}");
            WriteLine($"Lowest break iteration: {result.LowestBreakIteration}");
        }

        static string EmulateProcessing(string taskName)
        {
            Sleep(TimeSpan.FromMilliseconds(new Random(DateTime.Now.Millisecond).Next(250, 350)));
            WriteLine($"{taskName} task was processed on a thread id {CurrentThread.ManagedThreadId}");

            return taskName;
        }
    }
}
