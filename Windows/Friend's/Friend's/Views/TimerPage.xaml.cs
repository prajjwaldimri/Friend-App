using System;
using System.Globalization;
using Windows.Storage;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Friend_s.ViewModel;

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
            TimerValueRetriever();
        }

        

        private DispatcherTimer timer = new DispatcherTimer();
        private DispatcherTimer militimer = new DispatcherTimer();

        private static TimeSpan _secondstime;
        private TimeSpan _minutestime;
        private double _timerValue;
        private double _i;

        private void TimerOnTick(object sender, object o)
        {
                _secondstime = _secondstime.Add(TimeSpan.FromSeconds(-1));
                SecondText.Text = _secondstime.ToString("ss");
            

            if (_minutestime== new TimeSpan(0,1,0))
            {
                _i--;
                if (_i==0)
                {
                    _minutestime = _minutestime.Add(TimeSpan.FromMinutes(-1));
                    MinuteText.Text = _minutestime.ToString("mm");
                }
            }

            if (_secondstime == TimeSpan.Zero && _minutestime == new TimeSpan(0, 0, 0))
            {
                timer.Stop();
                militimer.Stop();
                //TODO: Uncomment on release
                var frame = Window.Current.Content as Frame;
                if (frame != null)
                {
                    frame.Navigate(typeof (Sospage));
                }

            }
            
        }

        private void HoldRectangle_OnHolding(object sender, HoldingRoutedEventArgs e)
        {
            TimerValueRetriever();
            if (e.HoldingState == HoldingState.Started)
            {
                if (timer.IsEnabled)
                {
                    timer.Stop();
                    militimer.Stop();
                    _secondstime = TimeSpan.FromSeconds(_timerValue);
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

        private void TimerValueRetriever()
        {
            var applicationData = ApplicationData.Current;
            var localsettings = applicationData.LocalSettings;
            if (localsettings.Values == null) return;
            if (localsettings.Values.ContainsKey("TimerTime"))
            {
                _timerValue = Convert.ToDouble(localsettings.Values["TimerTime"]);
                _secondstime = TimeSpan.FromSeconds(_timerValue);
                _minutestime = TimeSpan.FromMinutes(Convert.ToDouble(_secondstime.Minutes.ToString()));
            }
            else
            {
                _secondstime = TimeSpan.FromSeconds(20);
                _minutestime = TimeSpan.FromMinutes(Convert.ToDouble(_secondstime.Minutes.ToString()));
                localsettings.Values.Add("TimerTime", (double)20);
            }
            _i = _secondstime.Seconds;
            _i++;

        }
    }
}
