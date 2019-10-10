using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using static System.Console;
using static System.Threading.Thread;

namespace Recipe2
{
    class Program
    {
        static void Main(string[] args)
        {
            var observer = new CustomObserver();

            var goodObservable = new CustomSequence(new[] { 1, 2, 3, 4, 5 });
            var badObservable = new CustomSequence(null);

            using (IDisposable sunscription = goodObservable.Subscribe(observer))
            {

            }

            WriteLine($"Thread Id: {CurrentThread.ManagedThreadId}. Is Threadpool thread: {CurrentThread.IsThreadPoolThread}");

            using (IDisposable subscription = goodObservable.SubscribeOn(TaskPoolScheduler.Default).Subscribe(observer))
            {
                Sleep(TimeSpan.FromMilliseconds(100));
                WriteLine("Press ENTER to continue");
                ReadLine();
            }

            using (IDisposable subscription = badObservable.SubscribeOn(TaskPoolScheduler.Default).Subscribe(observer))
            {
                Sleep(TimeSpan.FromMilliseconds(100));
                WriteLine("Press ENTER to continue");
                ReadLine();
            }
        }

        class CustomObserver : IObserver<int>
        {
            public void OnCompleted()
            {
                WriteLine("Completed");
            }

            public void OnError(Exception error)
            {
                WriteLine($"Error: {error.Message}");
            }

            public void OnNext(int value)
            {
                WriteLine($"Next value: {value}; Thread Id: {CurrentThread.ManagedThreadId}");
            }
        }

        class CustomSequence : IObservable<int>
        {
            private readonly IEnumerable<int> numbers;

            public CustomSequence(IEnumerable<int> number)
            {
                numbers = number;
            }

            public IDisposable Subscribe(IObserver<int> observer)
            {
                WriteLine($"Thread Id: {CurrentThread.ManagedThreadId}. Is Threadpool thread: {CurrentThread.IsThreadPoolThread}");

                foreach (var n in numbers)
                {
                    observer.OnNext(n);
                }
                observer.OnCompleted();

                return Disposable.Empty;
            }
        }
    }
}
