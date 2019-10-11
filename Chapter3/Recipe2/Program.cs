using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using static System.Console;

namespace Recipe2
{
    class Program
    {
        static void Main(string[] args)
        {
            const int x = 1;
            const int y = 2;
            const string lambdaState = "lambda state 2";

            ThreadPool.QueueUserWorkItem(AsyncOperation);
            Thread.Sleep(TimeSpan.FromSeconds(2));

            ThreadPool.QueueUserWorkItem(AsyncOperation, "async state");
            Thread.Sleep(TimeSpan.FromSeconds(2));

            ThreadPool.QueueUserWorkItem(state =>
            {
                WriteLine($"Operation state: {state}");
                WriteLine($"Worker thread id: {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(TimeSpan.FromSeconds(2));
            }, "lambda state");

            ThreadPool.QueueUserWorkItem(_ => {
                WriteLine($"Operation state: {x + y}, {lambdaState}");
                WriteLine($"Worker thread id: {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(TimeSpan.FromSeconds(2));
            }, "lambda state");

            Thread.Sleep(TimeSpan.FromSeconds(2));
        }

        private static void AsyncOperation(object state)
        {
            WriteLine($"Operation state: {state ?? "(null)"}");
            WriteLine($"Worker thread id: {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(TimeSpan.FromSeconds(2));
        }
    }
}
