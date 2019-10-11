using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using static System.Console;
using static System.Threading.Thread;

namespace Recipe5
{
    class Program
    {
        static ManualResetEventSlim _mainEvent = new ManualResetEventSlim(false);

        static void Main(string[] args)
        {
            WriteLine("Start program...");

            var t1 = new Thread(() => TravelThroughGates("Thread 1", 5));
            var t2 = new Thread(() => TravelThroughGates("Thread 2", 6));
            var t3 = new Thread(() => TravelThroughGates("Thread 3", 12));
            t1.Start();
            t2.Start();
            t3.Start();

            DisplaySleep(1, 6);

            WriteLine("\tThe gates are now open!");
            _mainEvent.Set();
            DisplaySleep(7, 8);

            _mainEvent.Reset();
            WriteLine("\tThe gates have been closed!");

            DisplaySleep(9, 18);
            WriteLine("\tThe gates are now open for the second time!");
            _mainEvent.Set();

            DisplaySleep(19, 20);
            WriteLine("\tThe gates have been closed!");
            _mainEvent.Reset();
        }

        static void TravelThroughGates(string threadName, int seconds)
        {
            WriteLine($"\t{threadName} falls to sleep");
            Sleep(TimeSpan.FromSeconds(seconds));

            WriteLine($"\t{threadName} waits for the gates to open!");
            _mainEvent.Wait();

            WriteLine($"\t{threadName} enters the gates!");
        }

        static void DisplaySleep(int begin, int end)
        {
            for (int i = begin; i <= end; i++)
            {
                WriteLine($"Sleep {i}");
                Sleep(TimeSpan.FromSeconds(1));
            }
        }
    }
}
