using System;
using System.Diagnostics;
using Windows.ApplicationModel.Contacts;
using GalaSoft.MvvmLight.Command;

namespace Friend_s.ViewModel
{
    public class CallandSettingsPageViewModel : BaseViewModel
    {
        public RelayCommand LocalStorageSettingsRetrieverCommand { get; private set; }
        public RelayCommand<object> EditContactButtonHandlerCommand { get; private set; }

        public CallandSettingsPageViewModel()
        {
            LocalStorageSettingsRetrieverCommand = new RelayCommand(LocalStorageSettingsRetriever);
            EditContactButtonHandlerCommand = new RelayCommand<object>(EditContactButtonHandler);
        }

        private string _facebookConnected;
        private string _twitterConnected;
        private string _firstContactName;
        private string _secondContactName;
        private string _thirdContactName;
        private bool _toggleSwitchIsOn;
        private string _themeColor;


        public string FacebookConnected { get { return _facebookConnected; } }
        public string TwitterConnected { get { return _twitterConnected; } }
        public string FirstContactName { get { return _firstContactName; } }
        public string SecondContactName { get { return _secondContactName; } }
        public string ThirdContactName { get { return _thirdContactName; } }
        public bool ToggleSwitchIsOn { get { return _toggleSwitchIsOn;} }


        private void LocalStorageSettingsRetriever()
        {
            var applicationData = Windows.Storage.ApplicationData.Current;
            var localsettings = applicationData.LocalSettings;
            if (localsettings.Values == null) return;
            if (localsettings.Values.ContainsKey("FirstContactName"))
                _firstContactName = localsettings.Values["FirstContactName"] as string;
            if (localsettings.Values.ContainsKey("SecondContactName"))
                _secondContactName = localsettings.Values["SecondContactName"] as string;
            if (localsettings.Values.ContainsKey("ThirdContactName"))
                _thirdContactName = localsettings.Values["ThirdContactName"] as string;
            if (localsettings.Values.ContainsKey("FacebookConnect"))
                _facebookConnected = localsettings.Values["FacebookConnect"] as string;
            if (localsettings.Values.ContainsKey("TwitterConnect"))
                _twitterConnected= localsettings.Values["TwitterConnect"] as string;
            if (localsettings.Values.ContainsKey("ThemeColor"))
                _themeColor = localsettings.Values["ThemeColor"] as string;

            if (_themeColor == "#22A7F0")
            {
                _toggleSwitchIsOn = false;
            }
            else if(_themeColor== "#E01931")
            {
                _toggleSwitchIsOn = true;
            }

            RaisePropertyChanged(() =>FirstContactName);
            RaisePropertyChanged(() => SecondContactName);
            RaisePropertyChanged(() => ThirdContactName);
            RaisePropertyChanged(()=>ToggleSwitchIsOn);
        }

        private async void EditContactButtonHandler(object parameter)
        {
            var applicationData = Windows.Storage.ApplicationData.Current;
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
                        _firstContactName = contacts.DisplayName;
                        RaisePropertyChanged(()=>FirstContactName);
                    }
                    else
                    {
                        localsettings.Values.Remove("FirstContactName");
                        localsettings.Values.Remove("FirstContactNumber");
                        localsettings.Values.Add("FirstContactName", contacts.DisplayName);
                        localsettings.Values.Add("FirstContactNumber", contacts.YomiDisplayName);
                        _firstContactName = contacts.DisplayName;
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
                        _secondContactName = contacts1.DisplayName;
                        RaisePropertyChanged(() => SecondContactName);
                    }
                    else
                    {
                        localsettings.Values.Remove("SecondContactName");
                        localsettings.Values.Remove("SecondContactNumber");
                        localsettings.Values.Add("SecondContactName", contacts1.DisplayName);
                        localsettings.Values.Add("SecondContactNumber", contacts1.YomiDisplayName);
                        _secondContactName = contacts1.DisplayName;
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
                        _thirdContactName = contacts2.DisplayName;
                        RaisePropertyChanged(() => ThirdContactName);
                    }
                    else
                    {
                        localsettings.Values.Remove("ThirdContactName");
                        localsettings.Values.Remove("ThirdContactNumber");
                        localsettings.Values.Add("ThirdContactName", contacts2.DisplayName);
                        localsettings.Values.Add("ThirdContactNumber", contacts2.YomiDisplayName);
                        _thirdContactName = contacts2.DisplayName;
                        RaisePropertyChanged(() => ThirdContactName);
                    }
                    break;

                default:
                    Debug.WriteLine("Default Case Hit!");
                    break;
            }
        }
    }
}