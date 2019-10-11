using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using static System.Console;
using static System.Threading.Thread;

namespace Recipe3
{
    class Program
    {
        static void Main(string[] args)
        {
            ParallelQuery<string> parallelQuery = from t in GetTypes().AsParallel()
                                select EmulateProcessing(t);

            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(0.1));

            try
            {
                parallelQuery
                    .WithDegreeOfParallelism(Environment.ProcessorCount)
                    .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                    .WithMergeOptions(ParallelMergeOptions.Default)
                    .WithCancellation(cts.Token)
                    .ForAll(WriteLine);
            }
            catch (OperationCanceledException)
            {
                WriteLine("---");
                WriteLine("Operation has been canceled!");
            }

            WriteLine("---");
            WriteLine("Unordered PLINQ query execution");
            ParallelQuery<int> unorderedQuery = from i in ParallelEnumerable.Range(1, 30)
                                 select i;

            foreach (var i in unorderedQuery)
            {
                WriteLine(i);
            }

            WriteLine("---");
            WriteLine("Ordered PLINQ query execution");
            var orderedQuery = from i in ParallelEnumerable.Range(1, 30).AsOrdered()
                                 select i;

            foreach (var i in orderedQuery)
            {
                WriteLine(i);
            }

            //orderedQuery.ForAll(WriteLine);       // 并行输出不会展现原来的顺序
        }

        static string EmulateProcessing(string typeName)
        {
            Sleep(TimeSpan.FromMilliseconds(new Random(DateTime.Now.Millisecond).Next(250, 350)));
            WriteLine($"{typeName} type was printed on a thread id {CurrentThread.ManagedThreadId}");

            return typeName;
        }

        static IEnumerable<string> GetTypes()
        {
            return from assembly in AppDomain.CurrentDomain.GetAssemblies()
                   from type in assembly.GetExportedTypes()
                   where type.Name.StartsWith("Web")
                   orderby type.Name.Length
                   select type.Name;
        }
    }
}
