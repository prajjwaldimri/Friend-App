using System;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Friend_s.ViewModel;
using GalaSoft.MvvmLight.Messaging;

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

        private void UserNameSaveIcon_OnTapped(object sender, TappedRoutedEventArgs e)
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
            UserNameTextBlock.Text = UserNameTextBox.Text;

           CallandSettingsPageViewModel cspvm = new CallandSettingsPageViewModel();
            cspvm.UserNameSaver(UserNameTextBlock.Text);
        }


        
    }
}
