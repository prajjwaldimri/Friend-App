using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Friend_s.ViewModel
{
    public class BaseViewModel:ViewModelBase
    {
        public BaseViewModel()
        {
            UniversalSettingsCommand = new RelayCommand(UniversalSettingsRetriever);
        }

        private string _themeColor;
        private string _userName;
        private bool _isLoading;

        public RelayCommand UniversalSettingsCommand { get; private set; }

        public virtual bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                RaisePropertyChanged();
            }
        }

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
        public string ThemeColor
        {
            get { return _themeColor; }
            set { _themeColor = value; RaisePropertyChanged(()=>ThemeColor); }
        }

        public string CommandBarQuote { get; set; }

        public void RaisePropertyChangedBase()
        {
            RaisePropertyChanged(()=>ThemeColor);
            RaisePropertyChanged(()=>UserName);
        }

        public void UniversalSettingsRetriever()
        {
            var applicationData = Windows.Storage.ApplicationData.Current;
            var localsettings = applicationData.LocalSettings;
            var roamsettings = applicationData.RoamingSettings;
            if (localsettings.Values.ContainsKey("UserName"))
                _userName = localsettings.Values["UserName"] as string;
            if (localsettings.Values.ContainsKey("ThemeColor"))
            {
                _themeColor = localsettings.Values["ThemeColor"] as string;
            }
            else
            {
                localsettings.Values.Add("ThemeColor", "#18BC9C");
                _themeColor = localsettings.Values["ThemeColor"] as string;
            }
            RaisePropertyChanged(()=>UserName);
            RaisePropertyChanged(()=> ThemeColor);

            var quotes = new List<string>()
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
                "Enjoy Life"
            };

            var random = new Random();
            var number=  random.Next(1, 15);

            CommandBarQuote = quotes[number];

            RaisePropertyChanged(()=>CommandBarQuote);
        }
    }
}
