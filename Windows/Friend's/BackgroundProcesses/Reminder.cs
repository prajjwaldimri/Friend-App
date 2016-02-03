using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation;
using Windows.Devices.Sms;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;



namespace BackgroundProcesses
{
    public sealed class Reminder : IBackgroundTask
    {
        private static SmsDevice2 _device;

        private delegate void CallingInfoDelegate();

        private event CallingInfoDelegate CellInfoUpdateCompleted;
        private event CallingInfoDelegate ActivePhoneCallStateChanged;
        private CancellationTokenSource _cts = null;
        private static string _latitude;
        private static string _longitude;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            var details = taskInstance.TriggerDetails as ToastNotificationActionTriggerDetail;

            if (!localSettings.Values.ContainsKey("EmergencyOn"))
            {
                localSettings.Values.Add("EmergencyOn",true);
            }

            if (details != null)
            {
                if (localSettings.Values.ContainsKey("EmergencyOn"))
                {
                    localSettings.Values.Remove("EmergencyOn");
                }
                return;
            }

            if (localSettings.Values.ContainsKey("EmergencyOn"))
            {
                TimerStarter();
            }
        }


        private static void TimerStarter()
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(20);
            timer.Tick += timer_Tick;
            timer.Start();

            if (!Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
                return;
            var spineClass = new SpineClass();
            spineClass.InitializeCallingInfoAsync();
        }

        private static void timer_Tick(object sender, object e)
        {
            LocationAccesser();

            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
                return;
            MessageSender();
            Caller();
        }


        private static string _phonenumber;
        private static string _phonename;


        private static async void LocationAccesser()
        {
            try
            {
                var accessStatus = await Geolocator.RequestAccessAsync();

                switch (accessStatus)
                {
                    case GeolocationAccessStatus.Allowed:

                        // If DesiredAccuracy or DesiredAccuracyInMeters are not set (or value is 0), DesiredAccuracy.Default is used.
                        var geolocator = new Geolocator { DesiredAccuracy = PositionAccuracy.High };
                        // Carry out the operation
                        var pos = await geolocator.GetGeopositionAsync();
                        _latitude = pos.Coordinate.Point.Position.Latitude.ToString();
                        _longitude = pos.Coordinate.Point.Position.Longitude.ToString();
                        var location = new BasicGeoposition
                        {
                            Latitude = Math.Round(pos.Coordinate.Point.Position.Latitude, 4),
                            Longitude = Math.Round(pos.Coordinate.Point.Position.Longitude, 4)
                        };

                        break;
                    case GeolocationAccessStatus.Denied:
                        Debug.WriteLine("Access Denied!");
                        break;

                    case GeolocationAccessStatus.Unspecified:
                        Debug.WriteLine("Unspecified");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        private static async void MessageSender()
        {
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("SmsOn"))
            {
                //var messageDialog= new MessageDialog("SMS Permission Denied");
                //await messageDialog.ShowAsync();
                return;
            }
            if (_device == null)
            {
                try
                {
                    _device = SmsDevice2.GetDefault();
                }
                catch (Exception ex)
                {
                    //textBox.Text = ex.Message;
                    return;
                }

            }
            //if (_device == null) return;

            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("FirstContactNumber")) return;
            _phonenumber = ApplicationData.Current.LocalSettings.Values["FirstContactNumber"] as string;
            var msg = new SmsTextMessage2
            {
                To = _phonenumber,
                Body = "I am in need of help. My coordinates are\n Latitude:" + _latitude + "Longitude \n" + _longitude
            };
            var result = await _device.SendMessageAndGetResultAsync(msg);

            if (!result.IsSuccessful) return;
            var msgStr = "";
            msgStr += "Text message sent, To: " + _phonenumber;
            var msg1 = new MessageDialog("Message Sent!" + msgStr);
            await msg1.ShowAsync();
        }

        private static async void Caller()
        {
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("CallOn"))
            {
                return;
            }
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("FirstContactNumber")) return;
            _phonenumber = ApplicationData.Current.LocalSettings.Values["FirstContactNumber"] as string;

            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("FirstContactName")) return;
            _phonename = ApplicationData.Current.LocalSettings.Values["FirstContactName"] as string;

            if ((SpineClass.CurrentPhoneLine != null))
            {
                SpineClass.CurrentPhoneLine.Dial(_phonenumber, _phonename);
            }
            else
            {
                var dialog = new MessageDialog("No line found to place the call");
                await dialog.ShowAsync();
            }
        }


    }
}
