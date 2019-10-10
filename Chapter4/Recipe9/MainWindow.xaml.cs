using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Threading;

namespace Recipe9
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Task<string> TaskMehtod()
        {
            return TaskMethod(TaskScheduler.Default);
        }

        Task<string> TaskMethod(TaskScheduler scheduler)
        {
            Task delay = Task.Delay(TimeSpan.FromSeconds(5));

            return delay.ContinueWith(t =>
            {
                string str = $"Task is running on a thread id {Thread.CurrentThread.ManagedThreadId}. " +
                             $"Is thread pool thread: {Thread.CurrentThread.IsThreadPoolThread}";

                ContentTextBlock.Text = str;

                return str;
            }, scheduler);
        }

        private void ButtonSync_Click(object sender, RoutedEventArgs e)
        {
            ContentTextBlock.Text = string.Empty;

            try
            {
                string result = TaskMethod(TaskScheduler.FromCurrentSynchronizationContext()).Result;
                //string result = TaskMehtod().Result;
                ContentTextBlock.Text = result;
            }
            catch (Exception ex)
            {
                ContentTextBlock.Text = ex.InnerException.Message;
            }
        }

        private void ButtonAsync_Click(object sender, RoutedEventArgs e)
        {
            ContentTextBlock.Text = string.Empty;
            Mouse.OverrideCursor = Cursors.Wait;

            Task<string> task = TaskMehtod();
            task.ContinueWith(t =>
            {
                ContentTextBlock.Text = t.Exception.InnerException.Message;
                Mouse.OverrideCursor = null;
            }, 
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnFaulted,
            TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void ButtonAsyncOK_Click(object sender, RoutedEventArgs e)
        {
            ContentTextBlock.Text = string.Empty;
            Mouse.OverrideCursor = Cursors.Wait;

            Task<string> task = TaskMethod(TaskScheduler.FromCurrentSynchronizationContext());

            task.ContinueWith(t => Mouse.OverrideCursor = null,
                              CancellationToken.None,
                              TaskContinuationOptions.None,
                              TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
