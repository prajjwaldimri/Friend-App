using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.ApplicationModel.Contacts;
using Windows.ApplicationModel.Resources;
using Windows.Security.Authentication.Web;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using BeFriend.Services;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BeFriend.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FirstTimeTutorial : Page
    {
        public FirstTimeTutorial()
        {
            InitializeComponent();
            DedicatedStoryBoard.Begin();
            BackgroundAnimation.Begin();
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            if (!localSettings.Values.ContainsKey("FirstTimeRunComplete"))
            {
                //MainPivot.SelectedIndex = 1;
            }
        }

        private async void Pivot_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var localsettings = ApplicationData.Current.LocalSettings;
            var romaingsettings = ApplicationData.Current.RoamingSettings;

            switch (MainPivot.SelectedIndex)
            {
                case 0:
                    var roamSettings = ApplicationData.Current.RoamingSettings;
                    if (roamSettings.Values.ContainsKey("UserName"))
                    {
                        UserNameTextBox.Text = roamSettings.Values["UserName"].ToString();
                    }
                    FirstButton.IsChecked = true;
                    TutorialProgressBar.Value = 20;
                    
                    break;

                case 1:
                    SecondButton.IsChecked = true;
                    TutorialProgressBar.Value = 40;
                    if (!localsettings.Values.ContainsKey("UserName") && UserNameTextBox.Text != "")
                    {
                        localsettings.Values.Add("UserName", UserNameTextBox.Text);
                    }
                    else if (localsettings.Values.ContainsKey("UserName") && UserNameTextBox.Text != "")
                    {
                        localsettings.Values.Remove("UserName");
                        localsettings.Values.Add("UserName", UserNameTextBox.Text);
                    }
                    else
                    {
                        var msg = new MessageDialog("Please Enter Your Name!");
                        await msg.ShowAsync();
                        MainPivot.SelectedIndex = 0;
                    }
                    break;

                case 2:
                    ThirdButton.IsChecked = true;
                    TutorialProgressBar.Value = 60;
                    break;

                case 3:
                    FourthButton.IsChecked = true;
                    TutorialProgressBar.Value = 80;
                    NextButton.Visibility = Visibility.Visible;
                    FinishButton.Visibility = Visibility.Collapsed;
                    break;

                case 4:
                    FifthButton.IsChecked = true;
                    TutorialProgressBar.Value = 100;
                    FinishButton.Visibility = Visibility.Visible;
                    NextButton.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void NextButton_OnClick(object sender, RoutedEventArgs e)
        {
            if(MainPivot.SelectedIndex<5)
            MainPivot.SelectedIndex += 1;
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            if(MainPivot.SelectedIndex>0)
            MainPivot.SelectedIndex -= 1;
        }

        private void FinishButton_OnClickButton_OnClick(object sender, RoutedEventArgs e)
        {
            var localsettings = ApplicationData.Current.LocalSettings;
            if (!localsettings.Values.ContainsKey("FirstTimeRunComplete")) localsettings.Values.Add("FirstTimeRunComplete",true);
            Frame.Navigate(typeof (MainPage));
        }

        private async void PictureButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var image1 = new BitmapImage();

                //Initializing a new instance of FileOpenPicker
                var fp = new FileOpenPicker
                {
                    ViewMode = PickerViewMode.Thumbnail,
                    SuggestedStartLocation = PickerLocationId.PicturesLibrary
                };
                fp.FileTypeFilter.Add(".jpeg");
                fp.FileTypeFilter.Add(".png");
                fp.FileTypeFilter.Add(".jpg");
                // Using PickSingleFileAsync() will return one storage file which can be saved into an object of storage file class.          
                var storagefile = await fp.PickSingleFileAsync();

                if (storagefile == null) return;
                // Adding bitmap image object to store the stream provided by the object of StorageFile defined above.BitmapImage bmp = new BitmapImage();           

                DisplaypicPathTextBlock.Text = storagefile.Name;
                // Reading file as a stream and saving it in an object of IRandomAccess.         
                var stream = await storagefile.OpenAsync(FileAccessMode.Read);
                var stream1 = await storagefile.OpenReadAsync();

                // Adding stream as source of the bitmap image object defined above     
                await image1.SetSourceAsync(stream);

                SpineClass.ImagetoIsolatedStorageSaver(stream1, "profiledefault.jpg");

                stream1.Dispose();
                stream.Dispose();
                


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                var messageDialog = new MessageDialog(ex.ToString());
                await messageDialog.ShowAsync();

            }
        }

        private async void FirstContactPanel_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var localsettings = ApplicationData.Current.LocalSettings;
            var roamingsettings = ApplicationData.Current.RoamingSettings;
            var contactPicker = new ContactPicker();
            // Ask the user to pick contact phone numbers.
            contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);
            var contacts = await contactPicker.PickContactAsync();
            if (!localsettings.Values.ContainsKey("FirstContactName"))
            {
                localsettings.Values.Add("FirstContactName", contacts.DisplayName);
                localsettings.Values.Add("FirstContactNumber", contacts.YomiDisplayName);
                FirstContactTextBlock.Text = contacts.DisplayName;
            }
            else
            {
                localsettings.Values.Remove("FirstContactName");
                localsettings.Values.Remove("FirstContactNumber");
                localsettings.Values.Add("FirstContactName", contacts.DisplayName);
                localsettings.Values.Add("FirstContactNumber", contacts.YomiDisplayName);
                FirstContactTextBlock.Text = contacts.DisplayName;
                
            }
        }

        private async void SecondContactPanel_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var localsettings = ApplicationData.Current.LocalSettings;
            var romaingsettings = ApplicationData.Current.RoamingSettings;
            var contactPicker1 = new ContactPicker();
            // Ask the user to pick contact phone numbers.
            contactPicker1.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);
            var contacts1 = await contactPicker1.PickContactAsync();
            if (!localsettings.Values.ContainsKey("SecondContactName"))
            {
                localsettings.Values.Add("SecondContactName", contacts1.DisplayName);
                localsettings.Values.Add("SecondContactNumber", contacts1.YomiDisplayName);
                SecondContactTextBlock.Text = contacts1.DisplayName;
            }
            else
            {
                localsettings.Values.Remove("SecondContactName");
                localsettings.Values.Remove("SecondContactNumber");
                localsettings.Values.Add("SecondContactName", contacts1.DisplayName);
                localsettings.Values.Add("SecondContactNumber", contacts1.YomiDisplayName);
                SecondContactTextBlock.Text = contacts1.DisplayName;
            }
        }

        private async void ThirdContactPanel_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var localsettings = ApplicationData.Current.LocalSettings;
            var romaingsettings = ApplicationData.Current.RoamingSettings;
            var contactPicker2 = new ContactPicker();
            // Ask the user to pick contact phone numbers.
            contactPicker2.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);
            var contacts2 = await contactPicker2.PickContactAsync();
            if (!localsettings.Values.ContainsKey("ThirdContactName"))
            {
                localsettings.Values.Add("ThirdContactName", contacts2.DisplayName);
                localsettings.Values.Add("ThirdContactNumber", contacts2.YomiDisplayName);
                ThirdContactTextBlock.Text = contacts2.DisplayName;
            }
            else
            {
                localsettings.Values.Remove("ThirdContactName");
                localsettings.Values.Remove("ThirdContactNumber");
                localsettings.Values.Add("ThirdContactName", contacts2.DisplayName);
                localsettings.Values.Add("ThirdContactNumber", contacts2.YomiDisplayName);
                ThirdContactTextBlock.Text = contacts2.DisplayName;
            }
        }

        private void AddTwitterButton_OnClick(object sender, RoutedEventArgs e)
        {
            var frame = Window.Current.Content as Frame;
            frame?.BackStack.Add(new PageStackEntry(typeof(FirstTimeTutorial), null, null));
            frame?.Navigate(typeof(TwitterAuthenticator));
        }

        //private async void AddFacebookButton_OnClick(object sender, RoutedEventArgs e)
        //{
        //    var fbMessage = ResourceLoader.GetForCurrentView().GetString("FacebookMessage");
        //    var message = new MessageDialog(fbMessage);
        //    await message.ShowAsync();

        //    var sess = FBSession.ActiveSession;
        //    //Use your FB App ID
        //    sess.FBAppId = AuthTokens.FacebookAppID;
        //    sess.WinAppId = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString();


        //    // Add permissions required by the app
        //    var permissionList = new List<string> {"public_profile", "publish_actions"};
        //    var permissions = new FBPermissions(permissionList);


        //    // Login to Facebook
        //    var result = await sess.LoginAsync(permissions);

        //    if (result.Succeeded)
        //    {
        //        AddFacebookButton.IsEnabled = false;
        //    }
        //}
    }
}
