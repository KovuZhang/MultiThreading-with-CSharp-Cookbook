using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Diagnostics;
using static System.Console;

namespace Recipe1
{
    class Program
    {
        const string Item = "Dictionary item";
        const int iterations = 1000000;
        public static string currentItem;

        static void Main(string[] args)
        {
            var concurrentDictionary = new ConcurrentDictionary<int, string>();
            var dictionary = new Dictionary<int, string>();

            var sw = new Stopwatch();

            sw.Start();
            for (int i = 0; i < iterations; i++)
            {
                lock (dictionary)
                {
                    dictionary[i] = Item;
                }
            }
            sw.Stop();
            WriteLine($"Writing to dictionary with a lock: {sw.Elapsed}");

            sw.Restart();
            for (int i = 0; i < iterations; i++)
            {
                concurrentDictionary[i] = Item;
            }
            sw.Stop();
            WriteLine($"Writing to concurrent dictionary: {sw.Elapsed}");

            sw.Restart();
            for (int i = 0; i < iterations; i++)
            {
                lock (dictionary)
                {
                    currentItem = dictionary[i];
                }
            }
            sw.Stop();
            WriteLine($"Reading from dictionary with a lock: {sw.Elapsed}");

            sw.Restart();
            for (int i = 0; i < iterations; i++)
            {
                currentItem = concurrentDictionary[i];
            }
            sw.Stop();
            WriteLine($"Reading from concurrent dictionary: {sw.Elapsed}");
        }
    }
}
