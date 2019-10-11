using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using static System.Console;
using static System.Threading.Thread;

namespace Recipe1
{
    class Program
    {
        private delegate string RunOnThreadPool(out int threadId);

        static void Main(string[] args)
        {
            int threadId = 0;

            RunOnThreadPool poolDelegate = Test;

            var t = new Thread(() => Test(out threadId));
            t.Start();
            t.Join();

            WriteLine($"Thread id: {threadId}");

            IAsyncResult r = poolDelegate.BeginInvoke(out threadId, Callback, "a delegate asynchronous call");
            r.AsyncWaitHandle.WaitOne();

            string result = poolDelegate.EndInvoke(out threadId, r);

            WriteLine($"Thread pool worker thread id: {threadId}");
            WriteLine($"result is {result}");

            Sleep(TimeSpan.FromSeconds(2));
        }

        private static void Callback(IAsyncResult ar)
        {
            WriteLine("Starting a callback...");
            WriteLine($"State passed to a callback: {ar.AsyncState}");
            WriteLine($"Callback Is thread pool thread: {CurrentThread.IsThreadPoolThread}");
            WriteLine($"Callback Thread pool worker thread id: {CurrentThread.ManagedThreadId}");
        }

        private static string Test(out int threadId)
        {
            WriteLine("Starting...");
            WriteLine($"Test Is thread pool thread: {CurrentThread.IsThreadPoolThread}");

            Sleep(TimeSpan.FromSeconds(2));
            threadId = CurrentThread.ManagedThreadId;

            return $"Test Thread pool worker thread id: {threadId}";
        }
    }
}
