using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Timers;
using static System.Console;
using static System.Threading.Thread;

namespace Recipe6
{
    class Program
    {
        delegate string AsyncDelegate(string name);

        static void Main(string[] args)
        {
            IObservable<string> observable = LongRunningOperationAsync("Task 1");
            using (var sub = OutputToConsole(observable))
            {
                Sleep(TimeSpan.FromSeconds(2));
            }
            WriteLine("-------------------------------------");

            Task<string> t = LongRunningOperationTaskAsync("Task 2");
            using (var sub = OutputToConsole(t.ToObservable()))
            {
                Sleep(TimeSpan.FromSeconds(2));
            }
            WriteLine("-------------------------------------");

            AsyncDelegate asyncDelegate = LongRunningOperation;

            //Func<string, IObservable<string>> observableFactory = Observable.FromAsyncPattern<string, string>(asyncDelegate.BeginInvoke, asyncDelegate.EndInvoke);
            Task<string> obt = Task<string>.Factory.FromAsync(asyncDelegate.BeginInvoke("Task 3", null, null), asyncDelegate.EndInvoke);
            //observable = observableFactory("Task 3");
            using (var sub = OutputToConsole(obt.ToObservable()))
            {
                Sleep(TimeSpan.FromSeconds(2));
            }
            WriteLine("-------------------------------------");

            //observable = observableFactory("Task 4");
            obt = Task<string>.Factory.FromAsync(asyncDelegate.BeginInvoke("Task 4", null, null), asyncDelegate.EndInvoke);
            AwaitOnObservable(obt.ToObservable()).Wait();
            WriteLine("-------------------------------------");

            using (var timer = new Timer(1000))
            {
                var ot = Observable.FromEventPattern<ElapsedEventHandler, ElapsedEventArgs>(h => timer.Elapsed += h, h => timer.Elapsed -= h);
                timer.Start();

                using (var sub = OutputToConsole(ot))
                {
                    Sleep(TimeSpan.FromSeconds(5));
                }
                WriteLine("-------------------------------------");
                timer.Stop();
            }
        }

        static async Task<T> AwaitOnObservable<T>(IObservable<T> observable)
        {
            T obj = await observable;
            WriteLine($"{obj}");
            return obj;
        }

        static Task<string> LongRunningOperationTaskAsync(string name)
        {
            return Task.Run(() => LongRunningOperation(name));
        }

        static IObservable<string> LongRunningOperationAsync(string name)
        {
            return Observable.Start(() => LongRunningOperation(name));
        }

        static string LongRunningOperation(string name)
        {
            Sleep(TimeSpan.FromSeconds(1));
            return $"Task {name} is completed. Thread Id {CurrentThread.ManagedThreadId}";
        }

        static IDisposable OutputToConsole(IObservable<EventPattern<ElapsedEventArgs>> sequence)
        {
            return sequence.Subscribe(
                obj => WriteLine($"{obj.EventArgs.SignalTime}"),
                ex => WriteLine($"Error: {ex.Message}"),
                () => WriteLine("Completed")
                );
        }

        static IDisposable OutputToConsole<T>(IObservable<T> sequence)
        {
            return sequence.Subscribe(
                obj => WriteLine($"{obj}"),
                ex => WriteLine($"Error: {ex.Message}"),
                () => WriteLine("Completed")
                );
        }
    }
}
