using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
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

        private void UserNameSaveIcon_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var localData = ApplicationData.Current.LocalSettings;
            var roamData = ApplicationData.Current.RoamingSettings;
            if (!localData.Values.ContainsKey("UserName") && !roamData.Values.ContainsKey("UserName"))
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

            var cspvm = new CallandSettingsPageViewModel();
            cspvm.UserNameSaver(UserNameTextBlock.Text);
        }


        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var frame = Window.Current.Content as Frame;
            frame?.BackStack.Add(new PageStackEntry(typeof(CallandSmsSettings),null,null));
            frame?.Navigate(typeof (TwitterAuthenticator));
            
        }


        private void MessageSaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var localData = ApplicationData.Current.LocalSettings;
            var roamData = ApplicationData.Current.RoamingSettings;
            if (!localData.Values.ContainsKey("MessageToSend") && !roamData.Values.ContainsKey("MessageToSend"))
            {
                localData.Values.Add("MessageToSend", UserNameTextBox.Text);
                roamData.Values.Add("MessageToSend", UserNameTextBox.Text);
            }
            else
            {
                localData.Values.Remove("MessageToSend");
                roamData.Values.Remove("MessageToSend");
                localData.Values.Add("MessageToSend", UserNameTextBox.Text);
                roamData.Values.Add("MessageToSend", UserNameTextBox.Text);
            }

            

        }
    }
}
