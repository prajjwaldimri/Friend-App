﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Contacts;
using Windows.ApplicationModel.Resources;
using Windows.Security.Authentication.Web;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using BackgroundProcesses;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.QueryStringDotNET;
using NotificationsExtensions.Toasts;

namespace BeFriend.ViewModel
{
    /// <summary>
    /// ViewModel for settings Page. Currently takes heavy resources and shows delay in UI on navigated.
    /// //TODO: Reduce load time of Settings Page
    /// </summary>
    public class CallandSettingsPageViewModel : BaseViewModel
    {
        public RelayCommand LocalStorageSettingsRetrieverCommand { get; private set; }
        public RelayCommand PasswordVaultRetrieverCommand { get; private set; }
        public RelayCommand<object> EditContactButtonHandlerCommand { get; private set; }
        public RelayCommand<object> SocialIntegrationRemoverCommand { get; }
        public RelayCommand<object> ButtonClickHandlerCommand { get; private set; }
        public RelayCommand ToastToggledCommand { get; private set; }
        public RelayCommand ThemeToggledCommand { get; private set; }
        public RelayCommand SliderValueChangedCommand { get; private set; }
        //public RelayCommand FacebookAuthenticatorCommand { get; private set; }

        public CallandSettingsPageViewModel()
        {
            LocalStorageSettingsRetrieverCommand = new RelayCommand(LocalStorageSettingsRetriever);
            PasswordVaultRetrieverCommand = new RelayCommand(PasswordVaultRetrieverMethod);
            EditContactButtonHandlerCommand = new RelayCommand<object>(EditContactButtonHandler);
            SocialIntegrationRemoverCommand = new RelayCommand<object>(PasswordVaultRemoverMethod);
            ButtonClickHandlerCommand = new RelayCommand<object>(ButtonClickHandlerMethod);
            ToastToggledCommand = new RelayCommand(ToastMakerToggledButton);
            ThemeToggledCommand = new RelayCommand(ThemeChangerToggledButton);
            SliderValueChangedCommand = new RelayCommand(SliderValueControllerMethod);
            //FacebookAuthenticatorCommand = new RelayCommand(FacebookLoginMethod);
        }

        
        private string _themeColorPrimary;
        private string _themeColorSecondary;
        private string _notificationStatus;
        private string FacebookConnected { get; set; }
        private string TwitterConnected { get; set; }
        public string FirstContactName { get; private set; }
        public string SecondContactName { get; private set; }
        public string ThirdContactName { get; private set; }
        public bool ToggleSwitchIsOn { get; private set; }
        public bool ToastToggleSwitchIsOn { get; private set; }
        private bool IsAppFirstTimeOn { get; set; }
        private bool IsAppFirstTimeOn1 { get; set; }
        public Visibility TwitterPlusIconVisibility { get; private set; }
        public Visibility TwitterRemoveIconVisibility { get; private set; }
        public Visibility FacebookPlusIconVisibility { get; private set; }
        public Visibility FacebookRemoveIconVisibility { get; private set; }
        public Visibility MessageEditIconVisibility { get; private set; }
        public Visibility MessageSaveIconVisibility { get; private set; }
        public bool IsMessageBoxDisabled { get; private set; }
        public double SliderValue { get; set; }
        public string MessageBox { get; set; }


        private async void LocalStorageSettingsRetriever()
        {
            MessengerInstance.Send(new NotificationMessage("ProgressBarEnable"));
            try
            {
                var applicationData = ApplicationData.Current;
                var localsettings = applicationData.LocalSettings;
                if (localsettings.Values == null) return;
                if (localsettings.Values.ContainsKey("FirstContactName"))
                    FirstContactName = localsettings.Values["FirstContactName"] as string;
                if (localsettings.Values.ContainsKey("SecondContactName"))
                    SecondContactName = localsettings.Values["SecondContactName"] as string;
                if (localsettings.Values.ContainsKey("ThirdContactName"))
                    ThirdContactName = localsettings.Values["ThirdContactName"] as string;
                if (localsettings.Values.ContainsKey("FacebookConnect"))
                    FacebookConnected = localsettings.Values["FacebookConnect"] as string;
               
                if (localsettings.Values.ContainsKey("TwitterConnect"))
                    TwitterConnected = localsettings.Values["TwitterConnect"] as string;
                if (localsettings.Values.ContainsKey("ThemeColorPrimary"))
                    _themeColorPrimary = localsettings.Values["ThemeColorPrimary"] as string;
                if (localsettings.Values.ContainsKey("ThemeColorSecondary"))
                    _themeColorSecondary = localsettings.Values["ThemeColorSecondary"] as string;

                if (localsettings.Values.ContainsKey("ToastNotification"))
                    _notificationStatus = localsettings.Values["ToastNotification"] as string;
                else
                {
                    localsettings.Values.Add("ToastNotification", "Off");
                    _notificationStatus = "Off";
                }
                if (localsettings.Values.ContainsKey("TimerTime"))
                    SliderValue = (double) localsettings.Values["TimerTime"];
                if (localsettings.Values.ContainsKey("MessageToSend"))
                    MessageBox = localsettings.Values["MessageToSend"] as string;

                IsAppFirstTimeOn1 = true;

                if (_themeColorPrimary == "#237ba0")
                {
                    ToggleSwitchIsOn = false;
                }
                else if (_themeColorPrimary == "#f25f5c")
                {
                    ToggleSwitchIsOn = true;
                    IsAppFirstTimeOn = true;
                }

                const string taskName = "ActionCenterToastMaker";
                var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();

                if (backgroundAccessStatus != BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity &&
                    backgroundAccessStatus != BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity) return;
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    ToastToggleSwitchIsOn = task.Value.Name == taskName;
                    if (ToastToggleSwitchIsOn)
                    {
                        _notificationStatus = "On";
                        break;
                    }
                    _notificationStatus = "Off";
                }

                if (_notificationStatus == "Off")
                {
                    ToastToggleSwitchIsOn = false;
                }
                else if (_notificationStatus == "On")
                {
                    ToastToggleSwitchIsOn = true;
                }

               
                MessageEditIconVisibility = Visibility.Visible;
                MessageSaveIconVisibility = Visibility.Collapsed;
                IsMessageBoxDisabled = true;

                RaisePropertyChanged(() => FacebookPlusIconVisibility);
                RaisePropertyChanged(() => FacebookRemoveIconVisibility);
                RaisePropertyChanged(() => FirstContactName);
                RaisePropertyChanged(() => SecondContactName);
                RaisePropertyChanged(() => ThirdContactName);
                RaisePropertyChanged(() => ToggleSwitchIsOn);
                RaisePropertyChanged(() => ToastToggleSwitchIsOn);
                RaisePropertyChanged(() => SliderValue);
                RaisePropertyChanged(()=> MessageEditIconVisibility);
                RaisePropertyChanged(()=>MessageSaveIconVisibility);
                RaisePropertyChanged(()=>IsMessageBoxDisabled);
                RaisePropertyChanged(()=> MessageBox);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            MessengerInstance.Send(new NotificationMessage("ProgressBarDisable"));
        }

        private async void EditContactButtonHandler(object parameter)
        {
            var applicationData = ApplicationData.Current;
            var localsettings = applicationData.LocalSettings;
            switch (int.Parse(parameter.ToString()))
            {
                case 1:
                    var contactPicker = new ContactPicker();
                    // Ask the user to pick contact phone numbers.
                    contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);
                    var contacts = await contactPicker.PickContactAsync();
                    if (!localsettings.Values.ContainsKey("FirstContactName"))
                    {
                        localsettings.Values.Add("FirstContactName", contacts.DisplayName);
                        localsettings.Values.Add("FirstContactNumber", contacts.YomiDisplayName);
                        
                        FirstContactName = contacts.DisplayName;
                        RaisePropertyChanged(() => FirstContactName);
                    }
                    else
                    {
                        localsettings.Values.Remove("FirstContactName");
                        localsettings.Values.Remove("FirstContactNumber");
                        localsettings.Values.Add("FirstContactName", contacts.DisplayName);
                        localsettings.Values.Add("FirstContactNumber", contacts.YomiDisplayName);
                        FirstContactName = contacts.DisplayName;
                        RaisePropertyChanged(() => FirstContactName);

                    }
                    break;

                case 2:
                    var contactPicker1 = new ContactPicker();
                    // Ask the user to pick contact phone numbers.
                    contactPicker1.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);
                    var contacts1 = await contactPicker1.PickContactAsync();
                    if (!localsettings.Values.ContainsKey("SecondContactName"))
                    {
                        localsettings.Values.Add("SecondContactName", contacts1.DisplayName);
                        localsettings.Values.Add("SecondContactNumber", contacts1.YomiDisplayName);
                        SecondContactName = contacts1.DisplayName;
                        RaisePropertyChanged(() => SecondContactName);
                    }
                    else
                    {
                        localsettings.Values.Remove("SecondContactName");
                        localsettings.Values.Remove("SecondContactNumber");
                        localsettings.Values.Add("SecondContactName", contacts1.DisplayName);
                        localsettings.Values.Add("SecondContactNumber", contacts1.YomiDisplayName);
                        SecondContactName = contacts1.DisplayName;
                        RaisePropertyChanged(() => SecondContactName);
                    }
                    break;

                case 3:
                    var contactPicker2 = new ContactPicker();
                    // Ask the user to pick contact phone numbers.
                    contactPicker2.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);
                    var contacts2 = await contactPicker2.PickContactAsync();
                    if (!localsettings.Values.ContainsKey("ThirdContactName"))
                    {
                        localsettings.Values.Add("ThirdContactName", contacts2.DisplayName);
                        localsettings.Values.Add("ThirdContactNumber", contacts2.YomiDisplayName);
                        ThirdContactName = contacts2.DisplayName;
                        RaisePropertyChanged(() => ThirdContactName);
                    }
                    else
                    {
                        localsettings.Values.Remove("ThirdContactName");
                        localsettings.Values.Remove("ThirdContactNumber");
                        localsettings.Values.Add("ThirdContactName", contacts2.DisplayName);
                        localsettings.Values.Add("ThirdContactNumber", contacts2.YomiDisplayName);
                        ThirdContactName = contacts2.DisplayName;
                        RaisePropertyChanged(() => ThirdContactName);
                    }
                    break;

                case 4:

                    IsMessageBoxDisabled = true;
                    MessageEditIconVisibility = Visibility.Visible;
                    MessageSaveIconVisibility = Visibility.Collapsed;
                    RaisePropertyChanged(()=> MessageEditIconVisibility);
                    RaisePropertyChanged(()=> MessageSaveIconVisibility);
                    RaisePropertyChanged(()=> IsMessageBoxDisabled);
                    var localData = ApplicationData.Current.LocalSettings;
                    var roamData = ApplicationData.Current.RoamingSettings;
                    if (!localData.Values.ContainsKey("MessageToSend") && !roamData.Values.ContainsKey("MessageToSend"))
                    {
                        localData.Values.Add("MessageToSend", MessageBox);
                        roamData.Values.Add("MessageToSend", MessageBox);
                    }
                    else
                    {
                        localData.Values.Remove("MessageToSend");
                        roamData.Values.Remove("MessageToSend");
                        localData.Values.Add("MessageToSend", MessageBox);
                        roamData.Values.Add("MessageToSend", MessageBox);
                    }

                    break;

                case 5:
                    IsMessageBoxDisabled = false;
                    MessageEditIconVisibility = Visibility.Collapsed;
                    MessageSaveIconVisibility = Visibility.Visible;
                    RaisePropertyChanged(() => MessageEditIconVisibility);
                    RaisePropertyChanged(() => MessageSaveIconVisibility);
                    RaisePropertyChanged(() => IsMessageBoxDisabled);
                    break;

                default:
                    Debug.WriteLine("Default Case Hit!");
                    break;
            }
        }

        private async void PasswordVaultRemoverMethod(object obj)
        {
            switch (int.Parse(obj.ToString()))
            {
                case 1:
                    var vault = new PasswordVault();
                    try
                    {
                        var credentialList = vault.FindAllByUserName("TwitterAccessToken");
                        if (credentialList.Count <= 0) return;
                        var credential = vault.Retrieve("Friend", "TwitterAccessToken");
                        vault.Remove(new PasswordCredential("Friend", "TwitterAccessToken", credential.Password));
                        var credentialList1 = vault.FindAllByUserName("TwitterAccessTokenSecret");
                        if (credentialList1.Count <= 0) return;
                        var credential1 = vault.Retrieve("Friend", "TwitterAccessTokenSecret");
                        vault.Remove(new PasswordCredential("Friend", "TwitterAccessTokenSecret", credential1.Password));
                        TwitterPlusIconVisibility = Visibility.Visible;
                        TwitterRemoveIconVisibility = Visibility.Collapsed;
                    }

                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                    break;

                case 2:
                    //var sess = FBSession.ActiveSession;
                    //await sess.LogoutAsync();
                    //FacebookPlusIconVisibility = Visibility.Visible;
                    //FacebookRemoveIconVisibility = Visibility.Collapsed;
                    break;

                default:
                    break;
            }
            RaisePropertyChanged(() => TwitterRemoveIconVisibility);
            RaisePropertyChanged(() => TwitterPlusIconVisibility);
            RaisePropertyChanged(()=>FacebookPlusIconVisibility);
            RaisePropertyChanged(()=>FacebookRemoveIconVisibility);
        }

        private static async void BackgroundProcessRegisterer()
        {
            const string taskName = "ActionCenterToastMaker";

            var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();

            if (backgroundAccessStatus != BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity &&
                backgroundAccessStatus != BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity) return;
            if (BackgroundTaskRegistration.AllTasks.Any(task => task.Value.Name == taskName))
            {
                return;
            }

            var taskBuilder = new BackgroundTaskBuilder();
            taskBuilder.Name = taskName;
            taskBuilder.TaskEntryPoint = typeof(ActionCenterToastMaker).FullName;
            taskBuilder.SetTrigger(new TimeTrigger(500, false));

            var register = taskBuilder.Register();

            //Create a new Toast immediately after user toggles the switch     
            const string title = "Click on time of Emergency!";

            var visual = new ToastVisual
            {
                TitleText = new ToastText
                {
                    Text = title
                }

            };

            const int conversationId = 177777;

            var toastContent = new ToastContent
            {
                Visual = visual,
                Launch = new QueryString
                {
                    {"conversationId", conversationId.ToString()}
                }.ToString()
            };

            var toast = new ToastNotification(toastContent.GetXml())
            {
                ExpirationTime = DateTime.Now.AddHours(5),
                SuppressPopup = true,
                Tag = "Friends"
            };
            if (ToastNotificationManager.History != null)
            {
                ToastNotificationManager.History.Remove("Friends");
            }

            ToastNotificationManager.CreateToastNotifier().Show(toast);


        }

        private static async void BackgroundProcessRemover()
        {
            const string taskName = "ActionCenterToastMaker";

            var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();

            if (backgroundAccessStatus != BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity &&
                backgroundAccessStatus != BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity) return;
            foreach (var task in BackgroundTaskRegistration.AllTasks.Where(task => task.Value.Name == taskName))
            {
                task.Value.Unregister(true);
            }
        }

        private async void ToastMakerToggledButton()
        {

            if (IsAppFirstTimeOn1)
            {
                if (_notificationStatus == "On")
                {
                    IsAppFirstTimeOn1 = false;
                    return;
                }
            }

            IsAppFirstTimeOn1 = false;
            var localData = ApplicationData.Current.LocalSettings;
            var roamData = ApplicationData.Current.RoamingSettings;

            if (localData.Values.ContainsKey("ToastNotification"))
                _notificationStatus = localData.Values["ToastNotification"] as string;

            const string taskName = "ActionCenterToastMaker";

            var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();

            if (backgroundAccessStatus != BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity &&
                backgroundAccessStatus != BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity) return;
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                ToastToggleSwitchIsOn = task.Value.Name == taskName;
                if (ToastToggleSwitchIsOn)
                    break;
            }
            if (!ToastToggleSwitchIsOn)
            {
                if (_notificationStatus == "Off")
                {
                    if (!localData.Values.ContainsKey("ToastNotification"))
                        
                    {
                        localData.Values.Add("ToastNotification", "On");
                    }
                    else
                    {
                        localData.Values.Remove("ToastNotification");
                        localData.Values.Add("ToastNotification", "On");
                        
                    }
                    ToastToggleSwitchIsOn = true;
                    BackgroundProcessRegisterer();
                }
                else
                {
                    if (!localData.Values.ContainsKey("ToastNotification"))
                    {
                        localData.Values.Add("ToastNotification", "Off");
                    }
                    else
                    {
                        localData.Values.Remove("ToastNotification");
                        localData.Values.Add("ToastNotification", "Off");
                    }
                    ToastToggleSwitchIsOn = false;
                }
            }

            else
            {
                if (_notificationStatus == "On")
                {
                    if (!localData.Values.ContainsKey("ToastNotification"))
                    {
                        localData.Values.Add("ToastNotification", "Off");
                    }
                    else
                    {
                        localData.Values.Remove("ToastNotification");
                        localData.Values.Add("ToastNotification", "Off");
                    }
                    ToastToggleSwitchIsOn = false;
                    BackgroundProcessRemover();
                }
                else
                {
                    if (!localData.Values.ContainsKey("ToastNotification"))
                    {
                        localData.Values.Add("ToastNotification", "On");
                    }
                    else
                    {
                        localData.Values.Remove("ToastNotification");
                        localData.Values.Add("ToastNotification", "On");
                    }
                    ToastToggleSwitchIsOn = true;
                }
                
            }
           

            RaisePropertyChanged(() => ToastToggleSwitchIsOn);
            MessengerInstance.Send(new NotificationMessage("ProgressBarDisable"));
        }

        private void ThemeChangerToggledButton()
        {
            MessengerInstance.Send(new NotificationMessage("ProgressBarEnable"));
            var localData = ApplicationData.Current.LocalSettings;
            var roamData = ApplicationData.Current.RoamingSettings;

            if (ToggleSwitchIsOn)
            {
                if (IsAppFirstTimeOn)
                {
                    IsAppFirstTimeOn = false;
                    return;
                }
                if (!localData.Values.ContainsKey("ThemeColorPrimary") && !roamData.Values.ContainsKey("ThemeColorPrimary"))
                {
                    localData.Values.Add("ThemeColorPrimary", "#237ba0");
                    roamData.Values.Add("ThemeColorPrimary", "#237ba0");
                    localData.Values.Add("ThemeColorSecondary", "#70c1b4");
                    roamData.Values.Add("ThemeColorSecondary", "#70c1b4");
                }
                else
                {
                    localData.Values.Remove("ThemeColorPrimary");
                    roamData.Values.Remove("ThemeColorPrimary");
                    localData.Values.Remove("ThemeColorSecondary");
                    roamData.Values.Remove("ThemeColorSecondary");
                    localData.Values.Add("ThemeColorPrimary", "#237ba0");
                    roamData.Values.Add("ThemeColorPrimary", "#237ba0");
                    localData.Values.Add("ThemeColorSecondary", "#70c1b4");
                    roamData.Values.Add("ThemeColorSecondary", "#70c1b4");
                }
                ToggleSwitchIsOn = false;
                _themeColorPrimary = "#237ba0";
                _themeColorSecondary = "#70c1b4";
            }
            else
            {
                if (!localData.Values.ContainsKey("ThemeColorPrimary") && !roamData.Values.ContainsKey("ThemeColorPrimary"))
                {
                    localData.Values.Add("ThemeColorPrimary", "#f25f5c");
                    roamData.Values.Add("ThemeColorPrimary", "#f25f5c");
                    localData.Values.Add("ThemeColorSecondary", "#E55A57");
                    roamData.Values.Add("ThemeColorSecondary", "#E55A57");
                }
                else
                {
                    localData.Values.Remove("ThemeColorPrimary");
                    roamData.Values.Remove("ThemeColorPrimary");
                    localData.Values.Remove("ThemeColorSecondary");
                    roamData.Values.Remove("ThemeColorSecondary");
                    localData.Values.Add("ThemeColorPrimary", "#f25f5c");
                    roamData.Values.Add("ThemeColorPrimary", "#f25f5c");
                    localData.Values.Add("ThemeColorSecondary", "#E55A57");
                    roamData.Values.Add("ThemeColorSecondary", "#E55A57");
                }
                ToggleSwitchIsOn = true;
                _themeColorPrimary = "#f25f5c";
                _themeColorSecondary = "#E55A57";
            }

            RaisePropertyChanged(() => ToggleSwitchIsOn);
            MessengerInstance.Send(new NotificationMessage(_themeColorPrimary));
            MessengerInstance.Send(new NotificationMessage(_themeColorSecondary));
            MessengerInstance.Send(new NotificationMessage("ProgressBarDisable"));
        }

        private void PasswordVaultRetrieverMethod()
        {
            var vault = new PasswordVault();
            try
            {
                var credentialList = vault.FindAllByUserName("TwitterAccessToken");
                if (credentialList.Count <= 0) return;
                var credentialList1 = vault.FindAllByUserName("TwitterAccessTokenSecret");
                if (credentialList1.Count <= 0) return;
                TwitterPlusIconVisibility = Visibility.Collapsed;
                TwitterRemoveIconVisibility = Visibility.Visible;
            }

            catch (Exception ex)
            {
                TwitterPlusIconVisibility = Visibility.Visible;
                TwitterRemoveIconVisibility = Visibility.Collapsed;
                Debug.WriteLine(ex);
            }

            RaisePropertyChanged(() => TwitterPlusIconVisibility);
            RaisePropertyChanged(() => TwitterRemoveIconVisibility);

        }

        public void UserNameSaver(string userName)
        {
            MessengerInstance.Send(new NotificationMessage("ProgressBarEnable"));
            try
            {
                MessengerInstance.Send(new NotificationMessage(userName));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            MessengerInstance.Send(new NotificationMessage("ProgressBarDisable"));
        }

        private void SliderValueControllerMethod()
        {
            MessengerInstance.Send(new NotificationMessage("ProgressBarEnable"));
            var currentSliderValue = SliderValue;
            var localData = ApplicationData.Current.LocalSettings;

            if (localData.Values.ContainsKey("TimerTime")) localData.Values.Remove("TimerTime");
            localData.Values.Add("TimerTime", currentSliderValue);

            try
            {
                MessengerInstance.Send(new NotificationMessage(currentSliderValue.ToString()));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            MessengerInstance.Send(new NotificationMessage("ProgressBarDisable"));
        }

        //private async void FacebookLoginMethod()
        //{
        //    var fbMessage = ResourceLoader.GetForCurrentView().GetString("FacebookMessage");
        //    var message = new MessageDialog(fbMessage);
        //    await message.ShowAsync();

        //    MessengerInstance.Send(new NotificationMessage("ProgressBarEnable"));
        //    var sess = FBSession.ActiveSession;
        //    //Use your FB App ID
        //    sess.FBAppId = AuthTokens.FacebookAppID;
        //    sess.WinAppId = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString();
            

        //    // Add permissions required by the app
        //    var permissionList = new List<string>();
        //    permissionList.Add("public_profile");
        //    permissionList.Add("publish_actions");
        //    var permissions = new FBPermissions(permissionList);


        //    // Login to Facebook
        //    var result = await sess.LoginAsync(permissions);

        //    if (result.Succeeded)
        //    {
        //        FacebookPlusIconVisibility = Visibility.Collapsed;
        //        FacebookRemoveIconVisibility = Visibility.Visible;
        //    }
        //    else
        //    {
                
        //        FacebookPlusIconVisibility = Visibility.Visible;
        //        FacebookRemoveIconVisibility = Visibility.Collapsed;
        //    }
        //    RaisePropertyChanged(()=>FacebookPlusIconVisibility);
        //    RaisePropertyChanged(()=>FacebookRemoveIconVisibility);
        //    MessengerInstance.Send(new NotificationMessage("ProgressBarDisable"));
        //}

        private async void ButtonClickHandlerMethod(object obj)
        {
            var localsettings = ApplicationData.Current.LocalSettings;

            switch (Convert.ToInt64(obj))
            {
                case 1:
                    if (localsettings.Values.ContainsKey("FirstTimeRunComplete"))
                    {
                        localsettings.Values.Remove("FirstTimeRunComplete");
                        var dialog = new MessageDialog("Removed Key! Please restart the app to see the tutorial page!");
                        await dialog.ShowAsync();
                    }
                    break;

                case 2:
                    localsettings.Values.Clear();
                    var dialog1 = new MessageDialog("Removed all local settings. Restart the app to see effects!");
                    await dialog1.ShowAsync();
                    break;
            }
        }
       

    }


}
