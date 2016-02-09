using System;
using System.Globalization;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Friend_s.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TimerPage : UserControl
    {
        public TimerPage()
        {
            InitializeComponent();
        }

        private DispatcherTimer timer = new DispatcherTimer();
        private DispatcherTimer militimer = new DispatcherTimer();

        private static TimeSpan _secondstime = TimeSpan.FromSeconds(10);
        private TimeSpan _minutestime = TimeSpan.FromMinutes(Convert.ToDouble(_secondstime.Minutes.ToString()));
        private double _i;

        private void TimerOnTick(object sender, object o)
        {
                _secondstime = _secondstime.Add(TimeSpan.FromSeconds(-1));
                SecondText.Text = _secondstime.ToString("ss");
            

            if (_i%60 == 0)
            {
                _minutestime = _minutestime.Add(TimeSpan.FromMinutes(-1));
                MinuteText.Text = _minutestime.ToString("mm");
            }

            if (_secondstime == TimeSpan.Zero)
            {
                timer.Stop();
                militimer.Stop();
                //TODO: Uncomment on release
                //SosPageViewModel.TimerStarter();
            }
            _i++;
        }

        private void HoldRectangle_OnHolding(object sender, HoldingRoutedEventArgs e)
        {
            if (e.HoldingState == HoldingState.Started)
            {
                if (timer.IsEnabled)
                {
                    timer.Stop();
                    militimer.Stop();
                    _secondstime = TimeSpan.FromSeconds(10);
                    _minutestime = TimeSpan.FromMinutes(Convert.ToDouble(_secondstime.Minutes.ToString()));
                    timer.Tick -= TimerOnTick;
                }
            }


            if (e.HoldingState != HoldingState.Completed) return;
            timer.Interval = new TimeSpan(0, 0, 1);
            militimer.Interval = new TimeSpan(0, 0, 0);
            timer.Start();
            militimer.Start();
            timer.Tick += TimerOnTick;
            militimer.Tick += (o, o1) =>
            {
                var j = 100;
                var random = new Random();
                j-= random.Next(0,90);

                MillisecondText.Text = j.ToString(CultureInfo.CurrentCulture);
            };
        }
    }
}
