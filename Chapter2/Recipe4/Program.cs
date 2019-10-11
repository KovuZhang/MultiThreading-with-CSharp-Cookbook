using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using static System.Console;
using static System.Threading.Thread;

namespace Recipe4
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = new Thread(() => Process(10));
            t.Start();

            WriteLine("Waiting for another thread to complete its work");
            _workerEvent.WaitOne();

            WriteLine("First operation is completed!");
            WriteLine("Performing an operation on a main thread");
            Sleep(TimeSpan.FromSeconds(5));

            _mainEvent.Set();
            WriteLine("Now running the second operation on a second thread");

            _workerEvent.WaitOne();
            WriteLine("Second opeartion is completed");
        }

        private static AutoResetEvent _workerEvent = new AutoResetEvent(false);
        private static AutoResetEvent _mainEvent = new AutoResetEvent(false);

        static void Process(int seconds)
        {
            WriteLine("Starting a long running work...");

            Sleep(TimeSpan.FromSeconds(seconds));
            WriteLine("Long running word is done!");
            _workerEvent.Set();

            WriteLine("Waiting for a main thread to complete its work");
            _mainEvent.WaitOne();

            WriteLine("Starting second opeartion...");
            Sleep(TimeSpan.FromSeconds(seconds));
            WriteLine("Second opeartion work is done");

            _workerEvent.Set();
        }
    }
}
