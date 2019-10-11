using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using static System.Console;
using static System.Threading.Thread;

namespace Recipe11
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = new Thread(FaultyThread);
            t.Start();
            t.Join();

            try
            {
                t = new Thread(BadFaultyThread);
                t.Start();
            }
            catch (Exception ex)
            {
                WriteLine("We won't get here!");
            }
        }

        static void BadFaultyThread()
        {
            WriteLine("Starting a faulty thread...");
            Sleep(TimeSpan.FromSeconds(2));
            throw new Exception("Boom");
        }

        static void FaultyThread()
        {
            try
            {
                WriteLine("Starting a faulty thread...");
                Sleep(TimeSpan.FromSeconds(2));
                throw new Exception("Boom");
            }
            catch (Exception ex)
            {
                WriteLine($"Exception handled: {ex.Message}");
            }
        }
    }
}
