using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Storage;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace BeFriend.ViewModel
{
    public class BaseViewModel:ViewModelBase
    {
        public BaseViewModel()
        {
            UniversalSettingsCommand = new RelayCommand(UniversalSettingsRetriever);
            MessengerInstance.Register<NotificationMessage>(this,NotifyMe );
            InAppMessagesCommand = new RelayCommand(InAppMessages);
        }

        private string _themeColorPrimary;
        private string _themeColorSecondary;
        private string _userName;
        
        public RelayCommand UniversalSettingsCommand { get; private set; }
        public RelayCommand InAppMessagesCommand { get; private set; }
        
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
                    localsettings.Values.Add("ThemeColorPrimary", "#237ba0");
                    localsettings.Values.Add("ThemeColorSecondary", "#70c1b4");
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
                if (notification == "#f25f5c" || notification == "#237ba0")
                {
                    _themeColorPrimary = notification;
                    
                }

                else if (notification == "#70c1b4" || notification == "#E55A57")
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

        private async void InAppMessages()
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("InAppMessageCount"))
            {
                switch (Convert.ToInt64(localSettings.Values["InAppMessageCount"]))
                {
                    case 1:
                        

                       break;
                }

                var count = (int) localSettings.Values["InAppMessageCount"];
                count++;
                localSettings.Values["InAppMessageCount"] = count.ToString();

         

    }
            else
            {
                var dialog = new MessageDialog("Your personal information is and will never be " +
                                                       "shared with any 3rd party. \n The only information collected is by using " +
                                                       "HockeyApp SDK which stores the debug log whenever a crash occurs and sends it " +
                                                       "so that I can check what bugs are happening in the app.", "Privacy Policy");
                dialog.Options = MessageDialogOptions.None;
                dialog.Commands.Add(new UICommand("I Understand!", CommandInvokedHandler));
                dialog.Commands.Add(new UICommand("I didn't Understand", CommandInvokedHandler));

                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;
                await dialog.ShowAsync();
                localSettings.Values.Add("InAppMessageCount","1");
            }
        }

        private async void CommandInvokedHandler(IUICommand command)
        {
            switch (command.Label)
            {
                case "I Understand!":

                    break;
                case "I didn't Understand":
                    var uri = new Uri("mailto:prajjwaldimri@outlook.com");
                    await Launcher.LaunchUriAsync(uri);
                    break;
            }
        }
    }
}
