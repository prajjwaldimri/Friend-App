using System;
using Windows.Storage;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace Friend_s.ViewModel
{
    public class TimerPageViewModel : BaseViewModel
    {
        public TimerPageViewModel()
        {
            TimerValueRetrieverCommand = new RelayCommand(TimerValueRetriever);
            MessengerInstance.Register<NotificationMessage>(this, NotifyMe);
        }

        public RelayCommand TimerValueRetrieverCommand { get; set; }
        public double timerValue;
        public double SecondsValue { get; set; }
        public double MinuteValue { get; set; }

        private void TimerValueRetriever()
        {
            var applicationData = ApplicationData.Current;
            var localsettings = applicationData.LocalSettings;
            if (localsettings.Values == null) return;
            if (localsettings.Values.ContainsKey("TimerTime"))
            {
                timerValue = Convert.ToDouble(localsettings.Values["TimerTime"]);

                if (timerValue > 60)
                {
                    MinuteValue = 01;
                    SecondsValue = timerValue - 60;
                }
                else if (timerValue < 60)
                {
                    MinuteValue = 00;
                    SecondsValue = timerValue;
                }
                else
                {
                    SecondsValue = 00;
                    MinuteValue = 1;
                }
            }

            else
            {
                MinuteValue = 00;
                SecondsValue = 20;
                localsettings.Values.Add("TimerTime",(double)20);
            }

            RaisePropertyChanged(()=>MinuteValue);
            RaisePropertyChanged(()=>SecondsValue);
        }

        private void NotifyMe(NotificationMessage obj)
        {
            var notification = obj.Notification;
            double result;
            var parseresult= double.TryParse(notification, out result);

            if (!parseresult) return;
            timerValue = Convert.ToDouble(notification);

            if (timerValue > 60)
            {
                MinuteValue = 01;
                SecondsValue = timerValue - 60;
            }
            else if (timerValue < 60)
            {
                MinuteValue = 00;
                SecondsValue = timerValue;
            }
            else
            {
                SecondsValue = 00;
                MinuteValue = 1;
            }
            RaisePropertyChanged(()=> MinuteValue);
            RaisePropertyChanged(()=>SecondsValue);
        }
    }
}
