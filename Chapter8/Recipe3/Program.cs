using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Subjects;
using static System.Console;
using static System.Threading.Thread;

namespace Recipe3
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine("Subject");
            var subject = new Subject<string>();

            subject.OnNext("A");
            using (var subscription = OutputToConsole(subject))
            {
                subject.OnNext("B");
                subject.OnNext("C");
                subject.OnNext("D");
                subject.OnCompleted();
                subject.OnNext("Will not be printed out");
            }

            WriteLine("ReplaySubject");
            var replaySubject = new ReplaySubject<string>();

            replaySubject.OnNext("A");
            using (var subscription = OutputToConsole(replaySubject))
            {
                replaySubject.OnNext("B");
                replaySubject.OnNext("C");
                replaySubject.OnNext("D");
                replaySubject.OnCompleted();
            }

            WriteLine("Buffered ReplaySubject");
            var bufferedSubject = new ReplaySubject<string>(2);

            bufferedSubject.OnNext("A");
            bufferedSubject.OnNext("B");
            bufferedSubject.OnNext("C");
            using (var subscription = OutputToConsole(bufferedSubject))
            {
                bufferedSubject.OnNext("D");
                bufferedSubject.OnCompleted();
            }

            WriteLine("Time window ReplaySubject");
            var timeSubject = new ReplaySubject<string>(TimeSpan.FromMilliseconds(200));

            timeSubject.OnNext("A");
            Sleep(TimeSpan.FromMilliseconds(100));
            timeSubject.OnNext("B");
            Sleep(TimeSpan.FromMilliseconds(100));
            timeSubject.OnNext("C");
            Sleep(TimeSpan.FromMilliseconds(100));
            using (var subscription = OutputToConsole(timeSubject))
            {
                Sleep(TimeSpan.FromMilliseconds(500));
                timeSubject.OnNext("D");
                timeSubject.OnCompleted();
            }

            WriteLine("Asynchronous Subject");
            var asyncSubject = new AsyncSubject<string>();

            asyncSubject.OnNext("A");
            using (var subscription = OutputToConsole(asyncSubject))
            {
                asyncSubject.OnNext("B");
                asyncSubject.OnNext("C");
                asyncSubject.OnNext("D");
                asyncSubject.OnCompleted();
            }

            WriteLine("Behavior Subject");
            var behaviorSubject = new BehaviorSubject<string>("Default");
            
            //behaviorSubject.OnNext("B");
            using (var subscription = OutputToConsole(behaviorSubject))
            {
                behaviorSubject.OnNext("B");
                behaviorSubject.OnNext("C");
                behaviorSubject.OnNext("D");
                behaviorSubject.OnCompleted();
            }
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
