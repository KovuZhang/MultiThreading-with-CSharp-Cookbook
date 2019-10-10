using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using static System.Console;
using static System.Threading.Thread;

namespace Recipe2
{
    class Program
    {
        static void Main(string[] args)
        {
            const string MutexName = "CSharpThreadingCookbook";

            using (var m = new Mutex(false, MutexName))
            {
                if (!m.WaitOne(TimeSpan.FromSeconds(5), false))
                {
                    WriteLine("Second instance is running!");
                }
                else
                {
                    WriteLine("Running!");
                    ReadLine();
                    m.ReleaseMutex();
                }
            }
        }
    }
}
