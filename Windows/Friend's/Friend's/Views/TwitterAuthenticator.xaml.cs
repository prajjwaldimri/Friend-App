using System;
using System.Diagnostics;
using Windows.Security.Credentials;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Tweetinvi;
using Tweetinvi.Core.Credentials;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Friend_s.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TwitterAuthenticator
    {
        public TwitterAuthenticator()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            TwitterAuthenticatorMethod();
        }

        readonly TwitterCredentials _appCredentials = new TwitterCredentials("CONSUMER_KEY", "CONSUMER_SECRET");

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pinText.Text))
            {
                var msgDialog = new MessageDialog("Please Enter the pin showed below");
                await msgDialog.ShowAsync();
            }
            else
            {
                var pinCode = pinText.Text;
                var userCredentials = CredentialsCreator.GetCredentialsFromVerifierCode(pinCode, _appCredentials);
                Auth.SetCredentials(userCredentials);

                var vault = new PasswordVault();
                vault.Add(new PasswordCredential("Friend","Twitter",userCredentials.ToString()));
                var frame = Window.Current.Content as Frame;
                frame?.Navigate(typeof(MainPage));
            }
        }

        private void TwitterAuthenticatorMethod()
        {
            try
            {
                var url = CredentialsCreator.GetAuthorizationURL(_appCredentials);
                TwitterWebView.Navigate(new Uri(url));

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}
