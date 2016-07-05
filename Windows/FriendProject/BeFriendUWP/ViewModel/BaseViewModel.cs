﻿using System;
using System.Diagnostics;
using System.IO;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace BeFriend.ViewModel
{
    /// <summary>
    /// Methods in BaseViewModel will be fired on all pages.
    /// </summary>
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

        private async void UniversalSettingsRetriever()
        {
            try
            {
               var applicationData = ApplicationData.Current;
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
                var quotesFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///Quotes.txt"));

                var lines = File.ReadAllLines(quotesFile.Path);

                var rand = new Random();

                CommandBarQuote = lines[rand.Next(lines.Length)];

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
            var package = Package.Current.Id.Version;

            

            
            if (localSettings.Values.ContainsKey("AppUpdated") && localSettings.Values["AppUpdated"].ToString()!=(package.Build + package.Major.ToString()
                                                                                                                  + package.Minor))
            {
                var appUpdationString = ResourceLoader.GetForCurrentView().GetString("InAppMessage/AppUpdation");

                var messageDialog = new MessageDialog(appUpdationString);

                await messageDialog.ShowAsync();
                ApplicationData.Current.LocalSettings.Values["AddUpdated"]= (package.Build + package.Major.ToString()
                    + package.Minor);
            }
            if (localSettings.Values.ContainsKey("InAppMessageCount"))
            {
                switch (Convert.ToInt64(localSettings.Values["InAppMessageCount"]))
                {
                    case 1:

                        var bugStringContent = ResourceLoader.GetForCurrentView().GetString("InAppMessage/BugSuggestionsContent");
                        var bugStringTitle = ResourceLoader.GetForCurrentView().GetString("InAppMessage/BugSuggestionsTitle");
                        var dialog =
                            new MessageDialog(bugStringContent,
                                bugStringTitle) {Options = MessageDialogOptions.None};
                        dialog.Commands.Add(new UICommand("Gitter Chat", CommandInvokedHandler));
                        dialog.Commands.Add(new UICommand("Github Issues", CommandInvokedHandler));

                        dialog.DefaultCommandIndex = 0;
                        dialog.CancelCommandIndex = 1;

                        await dialog.ShowAsync();

                        break;

                    case 4:

                        var translateContent = ResourceLoader.GetForCurrentView().GetString("InAppMessage/TranslateContent");
                        var translateTitle = ResourceLoader.GetForCurrentView().GetString("InAppMessage/TranslateTitle");
                        var dialog1 =
                            new MessageDialog(translateContent,
                                translateTitle) {Options = MessageDialogOptions.None};
                        dialog1.Commands.Add(new UICommand("Translate Now!", CommandInvokedHandler));
                        dialog1.Commands.Add(new UICommand("Translate Later", CommandInvokedHandler));

                        dialog1.DefaultCommandIndex = 0;
                        dialog1.CancelCommandIndex = 1;

                        await dialog1.ShowAsync();

                        break;

                    case 9:

                        var developContent = ResourceLoader.GetForCurrentView().GetString("InAppMessage/DevelopContent");
                        var developTitle = ResourceLoader.GetForCurrentView().GetString("InAppMessage/DevelopTitle");
                        var dialog2 =
                            new MessageDialog(developContent,developTitle) {Options = MessageDialogOptions.None};
                        dialog2.Commands.Add(new UICommand("Join the Development!", CommandInvokedHandler));
                        dialog2.Commands.Add(new UICommand("Not interested", CommandInvokedHandler));

                        dialog2.DefaultCommandIndex = 0;
                        dialog2.CancelCommandIndex = 1;

                        await dialog2.ShowAsync();

                        break;
                }

                var count = Convert.ToInt64(localSettings.Values["InAppMessageCount"]);
                count++;
                localSettings.Values["InAppMessageCount"] = count.ToString();
            }
            else
            {
                var privacyContent = ResourceLoader.GetForCurrentView().GetString("InAppMessage/PrivacyContent");
                var privacyTitle = ResourceLoader.GetForCurrentView().GetString("InAppMessage/PrivacyTitle");
                var dialog = new MessageDialog(privacyContent,privacyTitle) {Options = MessageDialogOptions.None};
                dialog.Commands.Add(new UICommand("I Understand!", CommandInvokedHandler));
                dialog.Commands.Add(new UICommand("I didn't Understand", CommandInvokedHandler));

                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;
                await dialog.ShowAsync();
                localSettings.Values.Add("InAppMessageCount", "1");
            }
        }

        private async void CommandInvokedHandler(IUICommand command)
        {
            switch (command.Label)
            {
                case "I Understand!":

                    break;
                case "I didn't Understand":
                    await Launcher.LaunchUriAsync(new Uri("mailto:prajjwaldimri@outlook.com"));
                    break;
                case "Gitter Chat":
                    await Launcher.LaunchUriAsync(new Uri("https://gitter.im/prajjwaldimri/Friend-App"));
                    break;
                    
                case "Github Issues":
                    await Launcher.LaunchUriAsync(new Uri("https://github.com/prajjwaldimri/Friend-App/issues"));
                    break;

                case "Translate Now!":
                    await Launcher.LaunchUriAsync(new Uri("https://crowdin.com/project/befriend/invite"));
                    break;

                case "Join the Development!":
                    await Launcher.LaunchUriAsync(new Uri("https://gitter.im/prajjwaldimri/Friend-App"));
                    break;

            }
        }
    }
}
