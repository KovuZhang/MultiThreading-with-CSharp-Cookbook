#define Example_5

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;
using static System.Threading.Thread;

namespace Recipe1
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = ProcessAsynchronously();
            t.GetAwaiter().GetResult();
        }

        static async Task ProcessAsynchronously()
        {
            Task[] tasks = new Task[4];

#if Example_1
            var unsafeState = new UnsafeState();

            for (int i = 0; i < 4; i++)
            {
                tasks[i] = Task.Run(() => Worker(unsafeState));
            }
            await Task.WhenAll(tasks);

            WriteLine("---------------------------------------");

#elif Example_2
            var firstState = new DoubleCheckLocking();

            for (int i = 0; i < 4; i++)
            {
                tasks[i] = Task.Run(() => Worker(firstState));
            }
            await Task.WhenAll(tasks);

            WriteLine("---------------------------------------");
#elif Example_3
            var secondState = new BCLDoubleChecked();

            for (int i = 0; i < 4; i++)
            {
                tasks[i] = Task.Run(() => Worker(secondState));
            }
            await Task.WhenAll(tasks);

            WriteLine("---------------------------------------");
#elif Example_4
            var lazy = new Lazy<ValuetoAccess>(Compute);
            var thirdState = new LazyWrapper(lazy);

            for (int i = 0; i < 4; i++)
            {
                tasks[i] = Task.Run(() => Worker(thirdState));
            }
            await Task.WhenAll(tasks);

            WriteLine("---------------------------------------");
#elif Example_5
            var fourthState = new BCLThreadSafeFactory();

            for (int i = 0; i < 4; i++)
            {
                tasks[i] = Task.Run(() => Worker(fourthState));
            }
            await Task.WhenAll(tasks);

            WriteLine("---------------------------------------");
#endif
        }

        static void Worker(IHasValue state)
        {
            WriteLine($"Worker runs on thread id {CurrentThread.ManagedThreadId}.");

            WriteLine($"State value: {state.Value.Text}");
        }

        static ValuetoAccess Compute()
        {
            WriteLine($"The value is being constructed on a thread id {CurrentThread.ManagedThreadId}");

            Sleep(TimeSpan.FromSeconds(1));

            return new ValuetoAccess($"Constructed on thread id {CurrentThread.ManagedThreadId}.");
        }

        class LazyWrapper : IHasValue
        {
            private readonly Lazy<ValuetoAccess> value;

            public LazyWrapper(Lazy<ValuetoAccess> vle)
            {
                value = vle;
            }

            public ValuetoAccess Value => value.Value;
        }

        class BCLThreadSafeFactory : IHasValue
        {
            private ValuetoAccess value;

            public ValuetoAccess Value => LazyInitializer.EnsureInitialized(ref value, Compute);
        }

        class BCLDoubleChecked : IHasValue
        {
            private object syncRoot = new object();

            private ValuetoAccess value;

            private bool initilized;

            public ValuetoAccess Value => LazyInitializer.EnsureInitialized(ref value, ref initilized, ref syncRoot, Compute);
        }

        class DoubleCheckLocking : IHasValue
        {
            private readonly object syncRoot = new object();

            private volatile ValuetoAccess value;

            public ValuetoAccess Value
            {
                get
                {
                    if (value == null)
                    {
                        lock (syncRoot)
                        {
                            if (value == null)
                            {
                                value = Compute();
                            }
                        }
                    }

                    return value;
                }
            }
        }

        class UnsafeState : IHasValue
        {
            private ValuetoAccess value;

            public ValuetoAccess Value => value ?? (value = Compute());
        }

        class ValuetoAccess
        {
            public ValuetoAccess(string txt)
            {
                Text = txt;
            }

            public string Text { get; }
        }

        interface IHasValue
        {
            ValuetoAccess Value { get; }
        }
    }
}
