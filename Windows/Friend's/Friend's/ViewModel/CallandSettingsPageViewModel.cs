using System;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Contacts;
using Windows.Storage;
using Windows.UI.Xaml;
using Friend_s.Services;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace Friend_s.ViewModel
{
    public class CallandSettingsPageViewModel : BaseViewModel
    {
        public RelayCommand LocalStorageSettingsRetrieverCommand { get; private set; }
        public RelayCommand<object> EditContactButtonHandlerCommand { get; private set; }
        public RelayCommand TwitterCommand { get; private set; }
        public RelayCommand ToastToggledCommand { get; private set; }
        public RelayCommand ThemeToggledCommand { get; private set; }
        
        public CallandSettingsPageViewModel()
        {
            LocalStorageSettingsRetrieverCommand = new RelayCommand(LocalStorageSettingsRetriever);
            EditContactButtonHandlerCommand = new RelayCommand<object>(EditContactButtonHandler);
            TwitterCommand = new RelayCommand(TwitterConnector);
            ToastToggledCommand = new RelayCommand(ToastMakerToggledButton);
            ThemeToggledCommand = new RelayCommand(ThemeChangerToggledButton);
        }

        public void UserNameSaver(string userName)
        {
            MessengerInstance.Send<NotificationMessage>(new NotificationMessage(userName));
        }


        private string _themeColor;
        private string _notificationStatus;
        private string FacebookConnected { get; set; }
        private string TwitterConnected { get; set; }
        public string FirstContactName { get; private set; }
        public string SecondContactName { get; private set; }
        public string ThirdContactName { get; private set; }
        public bool ToggleSwitchIsOn { get; private set; }
        public bool ToastToggleSwitchIsOn { get; private set; }
        


        private void LocalStorageSettingsRetriever()
        {
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
                if (localsettings.Values.ContainsKey("ThemeColor"))
                    _themeColor = localsettings.Values["ThemeColor"] as string;
                if (localsettings.Values.ContainsKey("ToastNotification"))
                    _notificationStatus = localsettings.Values["ToastNotification"] as string;

                if (_themeColor == "#18BC9C")
                {
                    ToggleSwitchIsOn = false;
                }
                else if (_themeColor == "#BA4C63")
                {
                    ToggleSwitchIsOn = true;
                }
                if (_notificationStatus == "Off")
                {
                    ToastToggleSwitchIsOn = false;
                }
                else if (_notificationStatus == "On")
                {
                    ToastToggleSwitchIsOn = true;
                }

                RaisePropertyChanged(() => FirstContactName);
                RaisePropertyChanged(() => SecondContactName);
                RaisePropertyChanged(() => ThirdContactName);
                RaisePropertyChanged(() => ToggleSwitchIsOn);
                RaisePropertyChanged(() => ToastToggleSwitchIsOn);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
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

                default:
                    Debug.WriteLine("Default Case Hit!");
                    break;
            }
        }

        private void TwitterConnector()
        {
            SpineClass.TwitterAuthenticator();
        }

        private async void BackgroundProcessRegisterer()
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
            taskBuilder.TaskEntryPoint = typeof (BackgroundProcesses.ActionCenterToastMaker).FullName;
            taskBuilder.SetTrigger(new TimeTrigger(500, false));

            var register = taskBuilder.Register();
        }

        private async void BackgroundProcessRemover()
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
            }
            if (!ToastToggleSwitchIsOn)
            {
                if (!localData.Values.ContainsKey("ToastNotification") ||
                    !roamData.Values.ContainsKey("ToastNotification"))
                {
                    localData.Values.Add("ToastNotification", "On");
                    roamData.Values.Add("ToastNotification", "On");
                }
                else
                {
                    localData.Values.Remove("ToastNotification");
                    roamData.Values.Remove("ToastNotification");
                    localData.Values.Add("ToastNotification", "On");
                    roamData.Values.Add("ToastNotification", "On");
                }
                ToastToggleSwitchIsOn = true;
                BackgroundProcessRegisterer();
            }
            else
            {
                if (!localData.Values.ContainsKey("ToastNotification") ||
                    !roamData.Values.ContainsKey("ToastNotification"))
                {
                    localData.Values.Add("ToastNotification", "Off");
                    roamData.Values.Add("ToastNotification", "Off");
                }
                else
                {
                    localData.Values.Remove("ToastNotification");
                    roamData.Values.Remove("ToastNotification");
                    localData.Values.Add("ToastNotification", "Off");
                    roamData.Values.Add("ToastNotification", "Off");
                }
                ToastToggleSwitchIsOn = false;
                BackgroundProcessRemover();
            }

            RaisePropertyChanged(() => ToastToggleSwitchIsOn);
        }

        private void ThemeChangerToggledButton()
        {
            var localData = ApplicationData.Current.LocalSettings;
            var roamData = ApplicationData.Current.RoamingSettings;

            if (ToggleSwitchIsOn)
            {
                if (!localData.Values.ContainsKey("ThemeColor") && !roamData.Values.ContainsKey("ThemeColor"))
                {
                    localData.Values.Add("ThemeColor", "#18BC9C");
                    roamData.Values.Add("ThemeColor", "#18BC9C");
                }
                else
                {
                    localData.Values.Remove("ThemeColor");
                    roamData.Values.Remove("ThemeColor");
                    localData.Values.Add("ThemeColor", "#18BC9C");
                    roamData.Values.Add("ThemeColor", "#18BC9C");
                }
                ToggleSwitchIsOn = false;
                _themeColor = "#18BC9C";
            }
            else
            {
                if (!localData.Values.ContainsKey("ThemeColor") && !roamData.Values.ContainsKey("ThemeColor"))
                {
                    localData.Values.Add("ThemeColor", "#BA4C63");
                    roamData.Values.Add("ThemeColor", "#BA4C63");
                }
                else
                {
                    localData.Values.Remove("ThemeColor");
                    roamData.Values.Remove("ThemeColor");
                    localData.Values.Add("ThemeColor", "#BA4C63");
                    roamData.Values.Add("ThemeColor", "#BA4C63");
                }
                ToggleSwitchIsOn = true;
                _themeColor = "#BA4C63";
            }
            
            RaisePropertyChanged(()=>ToggleSwitchIsOn);
            MessengerInstance.Send<NotificationMessage>(new NotificationMessage(_themeColor));
        }
    }
}