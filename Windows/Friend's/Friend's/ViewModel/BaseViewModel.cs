using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace Friend_s.ViewModel
{
    public class BaseViewModel:ViewModelBase
    {
        public BaseViewModel()
        {
            UniversalSettingsCommand = new RelayCommand(UniversalSettingsRetriever);
            MessengerInstance.Register<NotificationMessage>(this,NotifyMe );
        }

        private string _themeColorPrimary;
        private string _themeColorSecondary;
        private string _userName;
        
        public RelayCommand UniversalSettingsCommand { get; private set; }
        
        public string UserName
        {
            get { return _userName;}
            set
            {
                if (value != _userName)
                {
                    _userName = value;
                    RaisePropertyChanged("UserName");
                }
            }
        }
        public string ThemeColorPrimary
        {
            get { return _themeColorPrimary; }
            set { _themeColorPrimary = value; RaisePropertyChanged(()=>ThemeColorPrimary); }
        }
        public string ThemeColorSecondary
        {
            get { return _themeColorSecondary; }
            set { _themeColorSecondary = value; RaisePropertyChanged(() => ThemeColorSecondary); }
        }
        public string CommandBarQuote { get; set; }
        public bool IsProgressBarEnabled { get; set; }
        public Visibility IsProgressBarVisibile { get; set; }

        private void RaisePropertyChangedBase()
        {
            try
            {
                RaisePropertyChanged(() => ThemeColorPrimary);
                RaisePropertyChanged(() => UserName);
                RaisePropertyChanged(() => IsProgressBarEnabled);
                RaisePropertyChanged(() => IsProgressBarVisibile);
                RaisePropertyChanged(()=> ThemeColorSecondary);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        private void UniversalSettingsRetriever()
        {
            try
            {
               var applicationData = Windows.Storage.ApplicationData.Current;
                var localsettings = applicationData.LocalSettings;
                if (localsettings.Values.ContainsKey("UserName"))
                    _userName = localsettings.Values["UserName"] as string;
                if (localsettings.Values.ContainsKey("ThemeColorPrimary"))
                {
                    _themeColorPrimary = localsettings.Values["ThemeColorPrimary"] as string;
                    _themeColorSecondary = localsettings.Values["ThemeColorSecondary"] as string;
                }
                else
                {
                    localsettings.Values.Add("ThemeColorPrimary", "#0371b2");
                    localsettings.Values.Add("ThemeColorSecondary", "#00a0ff");
                    _themeColorPrimary = localsettings.Values["ThemeColorPrimary"] as string;
                    _themeColorSecondary = localsettings.Values["ThemeColorSecondary"] as string;
                }
                IsProgressBarEnabled = false;
                IsProgressBarVisibile = Visibility.Collapsed;
                RaisePropertyChangedBase();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }

            try
            {
                var quotes = new List<string>
                {
                    "Be Yourself",
                    "Move On",
                    "Free Yourself",
                    "Look Up :)",
                    "Dream Big",
                    "Start Living!",
                    "Define Yourself",
                    "Be Happy",
                    "Be Fearless",
                    "Accept Yourself",
                    "Trust Yourself",
                    "Stay Positive",
                    "Don't Stop",
                    "Enjoy Life",
                    "Nobody is Perfect",
                    "Change is Good",
                    "Live the Moment",
                    "Never Stop Dreaming",
                    "Go For It",
                    "Never Give Up",
                    "Family is Forever"
                };

                var random = new Random();
                var number = random.Next(1, 20);

                CommandBarQuote = quotes[number];

                RaisePropertyChanged(() => CommandBarQuote);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
            
        }

        private void NotifyMe(NotificationMessage obj)
        {
            try
            {
                var notification = obj.Notification;
                double result;
                var parseresult = double.TryParse(notification, out result);

                if (parseresult) return;
                if (notification == "#ceb9fe" || notification == "#0371b2")
                {
                    _themeColorPrimary = notification;
                    
                }

                else if (notification == "#00a0ff" || notification == "#9f8fc4")
                {
                    _themeColorSecondary = notification;
                }

                else if (notification == "ProgressBarEnable" || notification == "ProgressBarDisable")
                {
                    if (notification == "ProgressBarEnable")
                    {
                        IsProgressBarEnabled = true;
                        IsProgressBarVisibile = Visibility.Visible;
                    }
                    else
                    {
                        IsProgressBarEnabled = false;
                        IsProgressBarVisibile = Visibility.Collapsed;
                    }
                }

                else
                {
                    _userName = notification;
                }
                RaisePropertyChangedBase();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        
    }
}
