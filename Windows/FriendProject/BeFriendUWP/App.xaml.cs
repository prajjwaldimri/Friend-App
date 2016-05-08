using System;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.Networking.PushNotifications;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using BeFriend.Views;
using Microsoft.WindowsAzure.Messaging;

namespace BeFriend
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);
            Microsoft.HockeyApp.HockeyClient.Current.Configure("7aa2a6b96c05425886f27178cbd678a6");
            InitializeComponent();
            Suspending += OnSuspending;
            InitNotificationsAsync();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;

            var localsettings = ApplicationData.Current.LocalSettings;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                //await CopyDatabase();
                
                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;

                try
                {

                    var status = await BackgroundExecutionManager.RequestAccessAsync();

                    var builder = new BackgroundTaskBuilder()
                    {
                        Name = "MyReminder",
                        TaskEntryPoint = "BackgroundProcesses.Reminder"
                    };

                    builder.SetTrigger(new ToastNotificationActionTrigger());

                    var registration = builder.Register();

                    
                }
                catch (Exception ex)
                {
                    Debug.Write(ex);
                }
            }
            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (localsettings.Values.ContainsKey("FirstTimeRunComplete"))
                {
                    rootFrame.Navigate(typeof (MainPage), e.Arguments);
                }

                else
                {
                    rootFrame.Navigate(typeof (FirstTimeTutorial), e.Arguments);
                }
            }
            // Ensure the current window is active
            Window.Current.Activate();

            if (!Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
                return;
            try
            {
                var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                statusBar.BackgroundColor = Color.FromArgb(255, 62, 70, 81);
                statusBar.BackgroundOpacity = 1;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        protected override void OnActivated(IActivatedEventArgs e)
        {
            // Get the root frame
            var rootFrame = Window.Current.Content as Frame;

            // TODO: Initialize root frame just like in OnLaunched

            // Handle toast activation
            if (e is ToastNotificationActivatedEventArgs)
            {
                if (rootFrame == null)
                {
                    // Create a Frame to act as the navigation context and navigate to the first page
                    rootFrame = new Frame();

                    rootFrame.NavigationFailed += OnNavigationFailed;

                    //await CopyDatabase();

                    if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                    {
                        //TODO: Load state from previously suspended application
                    }

                    // Place the frame in the current Window
                    Window.Current.Content = rootFrame;
                    
                }



                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigatio  n
                    // parameter
                    rootFrame.Navigate(typeof (Sospage));
                }
                else
                {
                    rootFrame.Navigate(typeof (Sospage));
                }

                if (rootFrame.BackStack.Count == 0)
                    rootFrame.BackStack.Add(new PageStackEntry(typeof(HomePage), null, null));
            }

            // TODO: Handle other types of activation

            // Ensure the current window is active
            Window.Current.Activate();
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);

        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private static async void InitNotificationsAsync()
        {
            try
            {
                var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

                var hub = new NotificationHub("BeFriendBetaNotificationHub",
                    "Endpoint=sb://befriendbetanotificationhub.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=GQjnn4xAR07sdjAYLrmZPCjrw5OzWKY3AB5fhaLZPdw=");
                var result = await hub.RegisterNativeAsync(channel.Uri);

                // Displays the registration ID so you know it was successful
                if (result.RegistrationId != null)
                {
                    Debug.WriteLine("Registration successful: " + result.RegistrationId);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

        }

    }
}
