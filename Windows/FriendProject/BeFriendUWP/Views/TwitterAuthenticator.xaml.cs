using System;
using System.Diagnostics;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Tweetinvi;
using Tweetinvi.Core.Authentication;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BeFriend.Views
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

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await AuthTokens.KeyRetriever();
            _appCredentials = new TwitterCredentials(AuthTokens.TwitterConsumerKey, AuthTokens.TwitterConsumerSecret);
            TwitterAuthenticatorMethod();

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += (s, ex) =>
            {
                var frame = Window.Current.Content as Frame;
                if (frame != null)
                {

                    frame.Navigate(typeof(MainPage));
                }
            };

        }
        //Use your consumerKey and ConsumerSecret

        private  TwitterCredentials _appCredentials;

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
                var authenticationContext = AuthFlow.InitAuthentication(_appCredentials);
                var userCredentials = AuthFlow.CreateCredentialsFromVerifierCode(pinCode, authenticationContext);
                Auth.SetCredentials(userCredentials);

                var vault = new PasswordVault();
                vault.Add(new PasswordCredential("Friend","TwitterAccessToken",userCredentials.AccessToken));
                vault.Add(new PasswordCredential("Friend","TwitterAccessTokenSecret",userCredentials.AccessTokenSecret));
                var localSettings = ApplicationData.Current.LocalSettings;
                var frame = Window.Current.Content as Frame;

                if (localSettings.Values.ContainsKey("FirstTimeRunComplete"))
                {
                    frame?.Navigate(typeof (MainPage));
                }
                else
                {
                    frame?.Navigate(typeof (FirstTimeTutorial));
                }
            }
        }

        private void TwitterAuthenticatorMethod()
        {
            try
            {
                var authenticationContext = AuthFlow.InitAuthentication(_appCredentials);
                TwitterWebView.Navigate(new Uri(authenticationContext.AuthorizationURL));

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            var frame = Window.Current.Content as Frame;
            if (localSettings.Values.ContainsKey("FirstTimeRunComplete"))
            {
                frame?.Navigate(typeof(MainPage));
            }
            else
            {
                frame?.Navigate(typeof(FirstTimeTutorial));
            }
        }
    }
}
