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
        static ReaderWriterLockSlim rw = new ReaderWriterLockSlim();
        static Dictionary<int, int> items = new Dictionary<int, int>();

        static void Main(string[] args)
        {
            new Thread(Read) { IsBackground = true }.Start();
            new Thread(Read) { IsBackground = true }.Start();
            new Thread(Read) { IsBackground = true }.Start();
            new Thread(() => Write("Thread 1")) { IsBackground = true }.Start();
            new Thread(() => Write("Thread 2")) { IsBackground = true }.Start();

            Sleep(TimeSpan.FromSeconds(30));
        }

        static void Read()
        {
            WriteLine("Reading contents of a dictionary");
            while (true)
            {
                try
                {
                    rw.EnterReadLock();
                    foreach (var key in items.Keys)
                    {
                        WriteLine($"Key: {key}");
                        Sleep(TimeSpan.FromSeconds(0.1));
                    }
                }
                finally
                {
                    rw.ExitReadLock();
                }
            }
        }

        static void Write(string threadName)
        {
            while (true)
            {
                try
                {
                    int newKey = new Random().Next(250);
                    rw.EnterUpgradeableReadLock();

                    if (!items.ContainsKey(newKey))
                    {
                        try
                        {
                            rw.EnterWriteLock();
                            items[newKey] = 1;
                            WriteLine($"New key {newKey} is added to a dictionary by a {threadName}");
                        }
                        finally
                        {
                            rw.ExitWriteLock();
                        }
                    }
                    Sleep(TimeSpan.FromSeconds(0.1));
                }
                finally
                {
                    rw.ExitUpgradeableReadLock();
                }
            }
        }
    }
}
