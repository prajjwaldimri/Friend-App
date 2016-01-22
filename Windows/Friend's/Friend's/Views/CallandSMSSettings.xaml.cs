using System;
using Windows.Storage;
using Windows.UI.Popups;
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
    public sealed partial class CallandSmsSettings
    {
        public CallandSmsSettings()
        {
            InitializeComponent();
        }


        //Code manipulates UI Elements directly, hence placed here!

        private void UserNameEditIcon_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            UserNameTextBox.Visibility = Visibility.Visible;
            UserNameTextBlock.Visibility = Visibility.Collapsed;
            UserNameEditIcon.Visibility = Visibility.Collapsed;
            UserNameSaveIcon.Visibility = Visibility.Visible;
            UserNameTextBox.Focus(FocusState.Pointer);
        }

        private async void UserNameSaveIcon_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var localData = ApplicationData.Current.LocalSettings;
            var roamData = ApplicationData.Current.RoamingSettings;
            if (!localData.Values.ContainsKey("UserName") || !roamData.Values.ContainsKey("UserName"))
            {
                localData.Values.Add("UserName", UserNameTextBox.Text);
                roamData.Values.Add("UserName", UserNameTextBox.Text);
            }
            else
            {
                localData.Values.Remove("UserName");
                roamData.Values.Remove("UserName");
                localData.Values.Add("UserName", UserNameTextBox.Text);
                roamData.Values.Add("UserName", UserNameTextBox.Text);
            }
            
            UserNameEditIcon.Visibility = Visibility.Visible;
            UserNameSaveIcon.Visibility = Visibility.Collapsed;
            UserNameTextBox.Visibility = Visibility.Collapsed;
            UserNameTextBlock.Visibility = Visibility.Visible;
            var bvm = new BaseViewModel();
            bvm.UniversalSettingsRetriever();
            bvm.RaisePropertyChangedBase();
            var msg = new MessageDialog("Profile will be updated at next restart!");
            await msg.ShowAsync();
            UserNameTextBlock.Text = UserNameTextBox.Text;
        }


        //TODO: Transfer to ViewModel

        private async void ToggleSwitch_OnToggled(object sender, RoutedEventArgs e)
        {
            var localData = ApplicationData.Current.LocalSettings;
            var roamData = ApplicationData.Current.RoamingSettings;
           

            if (!ThemeToggleSwitch.IsOn)
            {
                if (!localData.Values.ContainsKey("ThemeColor"))
                {
                    localData.Values.Add("ThemeColor", "#22A7F0");
                    roamData.Values.Add("ThemeColor", "#22A7F0");
                }
                else
                {
                    localData.Values.Remove("ThemeColor");
                    roamData.Values.Remove("ThemeColor");
                    localData.Values.Add("ThemeColor", "#22A7F0");
                    roamData.Values.Add("ThemeColor", "#22A7F0");
                }
            }
            else
            {
                if (!localData.Values.ContainsKey("ThemeColor"))
                {
                    localData.Values.Add("ThemeColor", "#E01931");
                    roamData.Values.Add("ThemeColor", "#E01931");
                }
                else
                {
                    localData.Values.Remove("ThemeColor");
                    roamData.Values.Remove("ThemeColor");
                    localData.Values.Add("ThemeColor", "#E01931");
                    roamData.Values.Add("ThemeColor", "#E01931");
                }
            }

            var msg = new MessageDialog("Theme will be updated at next restart!");
            await msg.ShowAsync();

        }
    }
}
