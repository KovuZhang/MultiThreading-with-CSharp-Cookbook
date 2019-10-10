using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;
using static System.Console;

namespace Recipe3
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = RunProgram();
            t.Wait();
        }

        static async Task RunProgram()
        {
            var taskStack = new ConcurrentStack<CustomTask>();
            var cts = new CancellationTokenSource();

            var taskSource = Task.Run(() => TaskProducer(taskStack));

            Task[] processors = new Task[4];
            for (int i = 1; i <= 4; i++)
            {
                string processorId = i.ToString();
                processors[i - 1] = Task.Run(() => TaskProcessor(taskStack, $"Processor {processorId}", cts.Token));
            }

            await taskSource;
            cts.CancelAfter(TimeSpan.FromSeconds(2));

            await Task.WhenAll(processors);
        }

        static async Task TaskProducer(ConcurrentStack<CustomTask> queue)
        {
            for (int i = 1; i <= 20; i++)
            {
                await Task.Delay(50);
                var workItem = new CustomTask { ID = i };
                queue.Push(workItem);
                WriteLine($"Task {workItem.ID} has been posted. Thread id: {Thread.CurrentThread.ManagedThreadId} ");
            }
        }

        static async Task TaskProcessor(ConcurrentStack<CustomTask> stark, string name, CancellationToken token)
        {
            await GetRandomDelay();
            do
            {
                bool popSuccessful = stark.TryPop(out CustomTask workItem);

                if (popSuccessful)
                {
                    WriteLine($"Task {workItem.ID} has been processed by {name}. Thread id: {Thread.CurrentThread.ManagedThreadId} ");
                }

                await GetRandomDelay();
            } while (!token.IsCancellationRequested);
        }

        static Task GetRandomDelay()
        {
            int delay = new Random(DateTime.Now.Millisecond).Next(1, 500);
            return Task.Delay(delay);
        }

        class CustomTask
        {
            public int ID { get; set; }
        }
    }
}
