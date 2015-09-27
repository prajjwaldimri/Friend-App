using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Calls;
using Windows.ApplicationModel.Chat;
using Windows.Devices.Geolocation;
using Windows.Devices.Sms;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml.Automation.Peers;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Friend_s
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private PhoneLine currentPhoneLine;
        private SmsDevice2 device;
        private MainPage rootPage;
        private Dictionary<Guid, PhoneLine> allPhoneLines;
        private bool doesPhoneCallExist;
        private int noOfLines;
        public delegate void CallingInfoDelegate();
        public event CallingInfoDelegate CellInfoUpdateCompleted;
        public event CallingInfoDelegate ActivePhoneCallStateChanged;
        private CancellationTokenSource _cts = null;

        public MainPage()
        {
            this.InitializeComponent();
            InitializeCallingInfoAsync();
        }

        

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void MenuButton3_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (Voice));
        }

        private async void Button_OnClick(object sender, RoutedEventArgs e)
        {
            
            if (device == null)
            {
                try
                {
                   device = SmsDevice2.GetDefault();
                }
                catch (Exception ex)
                {
                    textBox.Text = ex.Message;
                   return;
                }

            }
            string msgStr = "";
            if (device != null)
            {
                SmsTextMessage2 msg = new SmsTextMessage2();
                msg.To = "7830207022";
                msg.Body = "Hello!";



                SmsSendMessageResult result = await device.SendMessageAndGetResultAsync(msg);

                if (result.IsSuccessful)
                {
                    msgStr = "";
                    msgStr += "Text message sent, cellularClass: " + result.CellularClass.ToString();
                    textBox.Text = msgStr;

                }
            }
        }

        private async void Button1_OnClick(object sender, RoutedEventArgs e)
        {
            

            if ((currentPhoneLine != null))
            {
                currentPhoneLine.Dial("7830207022", "Prajjwal Dimri");
            }
            else
            {
                var dialog = new MessageDialog("No line found to place the call");
                await dialog.ShowAsync();
            }
        }

        #region phonecallsetters
        private async void InitializeCallingInfoAsync()
        {
            this.MonitorCallState();

            //Get all phone lines (To detect dual SIM devices)
            Task<Dictionary<Guid, PhoneLine>> getPhoneLinesTask = GetPhoneLinesAsync();
            allPhoneLines = await getPhoneLinesTask;

            //Get number of lines
            noOfLines = allPhoneLines.Count;

            //Get Default Phone Line
            Task<PhoneLine> getDefaultLineTask = GetDefaultPhoneLineAsync();
            currentPhoneLine = await getDefaultLineTask;

            
        }

        private void MonitorCallState()
        {
            PhoneCallManager.CallStateChanged += (o, args) =>
            {
                doesPhoneCallExist = PhoneCallManager.IsCallActive || PhoneCallManager.IsCallIncoming;
                if (ActivePhoneCallStateChanged != null)
                {
                    ActivePhoneCallStateChanged();
                }
            };
        }

        private async Task<Dictionary<Guid, PhoneLine>> GetPhoneLinesAsync()
        {
            PhoneCallStore store = await PhoneCallManager.RequestStoreAsync();

            // Start the PhoneLineWatcher
            var watcher = store.RequestLineWatcher();
            var phoneLines = new List<PhoneLine>();
            var lineEnumerationCompletion = new TaskCompletionSource<bool>();
            watcher.LineAdded += async (o, args) => { var line = await PhoneLine.FromIdAsync(args.LineId); phoneLines.Add(line); };
            watcher.Stopped += (o, args) => lineEnumerationCompletion.TrySetResult(false);
            watcher.EnumerationCompleted += (o, args) => lineEnumerationCompletion.TrySetResult(true);
            watcher.Start();

            // Wait for enumeration completion
            if (!await lineEnumerationCompletion.Task)
            {
                throw new Exception("Phone Line Enumeration failed");
            }

            watcher.Stop();

            Dictionary<Guid, PhoneLine> returnedLines = new Dictionary<Guid, PhoneLine>();

            foreach (PhoneLine phoneLine in phoneLines)
            {
                if (phoneLine != null && phoneLine.Transport == PhoneLineTransport.Cellular)
                {
                    returnedLines.Add(phoneLine.Id, phoneLine);
                }
            }

            return returnedLines;
        }
        private async Task<PhoneLine> GetDefaultPhoneLineAsync()
        {
            PhoneCallStore phoneCallStore = await PhoneCallManager.RequestStoreAsync();
            Guid lineId = await phoneCallStore.GetDefaultLineAsync();
            return await PhoneLine.FromIdAsync(lineId);
        }
        #endregion

        private async void Button2_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var accessStatus = await Geolocator.RequestAccessAsync();

                switch (accessStatus)
                {
                    case GeolocationAccessStatus.Allowed:

                        // If DesiredAccuracy or DesiredAccuracyInMeters are not set (or value is 0), DesiredAccuracy.Default is used.
                        Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 0 };

                        // Carry out the operation
                        Geoposition pos = await geolocator.GetGeopositionAsync();


                        textBox1.Text = (pos.Coordinate.Point.Position.Latitude).ToString() +"\n"+ (pos.Coordinate.Point.Position.Longitude).ToString();
                        
                        break;
                    case GeolocationAccessStatus.Denied:
                        textBox1.Text = "Access Denied!";
                        break;

                    case GeolocationAccessStatus.Unspecified:
                        textBox1.Text = "Unspecified";
                        break;
                }
            }
            catch (Exception ex)
            {
                textBox1.Text = "Error";
            }
        }
    }
}
