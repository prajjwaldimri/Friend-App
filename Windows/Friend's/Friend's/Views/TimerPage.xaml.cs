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
            var applicationData = ApplicationData.Current.LocalSettings;
            
        }

        private DispatcherTimer timer = new DispatcherTimer();
        private DispatcherTimer militimer = new DispatcherTimer();

        static TimeSpan secondstime = TimeSpan.FromSeconds(120);
        TimeSpan minutestime = TimeSpan.FromMinutes(Convert.ToDouble(secondstime.Minutes.ToString()));
        double i=0;

        private void TimerOnTick(object sender, object o)
        {
                secondstime = secondstime.Add(TimeSpan.FromSeconds(-1));
                SecondText.Text = secondstime.ToString("ss");
            

            if (i%60 == 0)
            {
                minutestime = minutestime.Add(TimeSpan.FromMinutes(-1));
                MinuteText.Text = minutestime.ToString("mm");
            }

            if (secondstime == TimeSpan.Zero)
            {
                timer.Stop();
                militimer.Stop();
                //TODO: Uncomment on release
                //SosPageViewModel.TimerStarter();
            }
            i++;
        }

        private void HoldRectangle_OnHolding(object sender, HoldingRoutedEventArgs e)
        {
            if (e.HoldingState == HoldingState.Started)
            {
                if (timer.IsEnabled)
                {
                    timer.Stop();
                    militimer.Stop();
                    timer.Tick -= TimerOnTick;
                }
            }
            
            
            if (e.HoldingState == HoldingState.Completed)
            {
                timer.Interval = new TimeSpan(0, 0, 1);
                militimer.Interval = new TimeSpan(0, 0, 0);
                timer.Start();
                militimer.Start();
                timer.Tick += TimerOnTick;
                militimer.Tick += (o, o1) =>
                {
                    int j = 100;
                    var random = new Random();
                    j-= random.Next(0,90);

                    MillisecondText.Text = j.ToString(CultureInfo.CurrentCulture);
                };
            }
        }
    }
}
