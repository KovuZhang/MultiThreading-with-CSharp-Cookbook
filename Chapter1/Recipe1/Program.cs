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
        static void PrintNumbers()
        {
            WriteLine("Starting...");
            for (int i = 1; i < 10; i++)
            {
                WriteLine(i);
            }
        }

        static void Main(string[] args)
        {
            Thread t = new Thread(PrintNumbers);
            t.Start();
            PrintNumbers();
        }
    }
}
