using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using static System.Console;
using static System.Threading.Thread;

namespace Recipe4
{
    class Program
    {
        static void Main(string[] args)
        {
            IObservable<int> observable = Observable.Return(0);
            using (var sub = OutputToConsole(observable))
            {

            }
            WriteLine(" --------------------------------------- ");

            observable = Observable.Empty<int>();
            using (var sub = OutputToConsole(observable))
            {

            }
            WriteLine(" --------------------------------------- ");

            observable = Observable.Throw<int>(new Exception());
            using (var sub = OutputToConsole(observable))
            {

            }
            WriteLine(" --------------------------------------- ");

            observable = Observable.Repeat(42);
            using (var sub = OutputToConsole(observable.Take(5)))
            {

            }
            WriteLine(" --------------------------------------- ");

            observable = Observable.Range(0, 10);
            using (var sub = OutputToConsole(observable))
            {

            }
            WriteLine(" --------------------------------------- ");

            observable = Observable.Create<int>(ob => {
                for (int i = 0; i < 10; i++)
                {
                    ob.OnNext(i);
                }
                return Disposable.Empty;
            });
            using (var sub = OutputToConsole(observable))
            {

            }
            WriteLine(" --------------------------------------- ");

            observable = Observable.Generate(
                0,
                i => i < 5,
                i => ++i,
                i => i * 2
                );
            using (var sub = OutputToConsole(observable))
            {

            }
            WriteLine(" --------------------------------------- ");

            IObservable<long> observableLong = Observable.Interval(TimeSpan.FromSeconds(1));
            using (var sub = OutputToConsole(observableLong))
            {
                Sleep(TimeSpan.FromSeconds(4));
            }
            WriteLine(" --------------------------------------- ");

            observableLong = Observable.Timer(DateTimeOffset.Now.AddSeconds(2));
            using (var sub = OutputToConsole(observableLong))
            {
                Sleep(TimeSpan.FromSeconds(3));
            }
            WriteLine(" --------------------------------------- ");
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
