using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Recipe1
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            _timer = new DispatcherTimer();
            _ticks = 0;
        }

        private readonly DispatcherTimer _timer;
        private int _ticks;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Grid.Children.Clear();

            var commonPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var textBlock = new TextBlock
            {
                Text = "Sample timer application",
                FontSize = 32,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(40)
            };

            var timerTextBlock = new TextBlock
            {
                Text = "0",
                FontSize = 32,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(40)
            };

            var timerStateTextBlock = new TextBlock
            {
                Text = "Timer is enabled",
                FontSize = 32,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(40)
            };

            var startButton = new Button
            {
                Content = "Start",
                FontSize = 32
            };

            var stopButton = new Button
            {
                Content = "Stop",
                FontSize = 32
            };

            buttonPanel.Children.Add(startButton);
            buttonPanel.Children.Add(stopButton);

            commonPanel.Children.Add(textBlock);
            commonPanel.Children.Add(timerTextBlock);
            commonPanel.Children.Add(timerStateTextBlock);
            commonPanel.Children.Add(buttonPanel);

            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (sender, eventArgs) =>
            {
                timerTextBlock.Text = _ticks.ToString();
                _ticks++;
            };
            _timer.Start();

            startButton.Click += (sender, eventArgs) =>
            {
                timerTextBlock.Text = "0";
                _timer.Start();
                _ticks = 1;
                timerStateTextBlock.Text = "Timer is enabled";
            };

            stopButton.Click += (sender, eventArgs) =>
            {
                _timer.Stop();
                timerStateTextBlock.Text = "Timer is disabled";
            };

            Grid.Children.Add(commonPanel);
        }

        private void _timer_Tick(object sender, object e)
        {
            throw new NotImplementedException();
        }
    }
}
