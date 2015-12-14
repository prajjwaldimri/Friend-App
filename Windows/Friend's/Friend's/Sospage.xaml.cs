using System;
using System.Diagnostics;
using System.Threading;
using Windows.Devices.Geolocation;
using Windows.Devices.Sms;
using Windows.Services.Maps;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Friend_s
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Sospage : Page
    {
        private SmsDevice2 _device;
        public delegate void CallingInfoDelegate();
        public event CallingInfoDelegate CellInfoUpdateCompleted;
        public event CallingInfoDelegate ActivePhoneCallStateChanged;
        private CancellationTokenSource _cts = null;
        private static string latitude;
        private static string longitude;
        
        public Sospage()
        {
            InitializeComponent();
            LocationAccesser();
            if (!Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
                return;
            var spineClass = new SpineClass();
            spineClass.InitializeCallingInfoAsync();
            DatabaseConnector();
            MessageSender();
            Caller();
        }

        private static string _phonenumber;
        private static string _phonename;
        private static void DatabaseConnector()
        {
            var list = CreateDatabase.GetValues(@"SELECT Number FROM Details WHERE ID=1");
            var list1 = CreateDatabase.GetValues(@"SELECT Name FROM Details WHERE ID=1");
            string[] arr = list;
            string[] brr = list1;
            _phonenumber = arr[0];
            _phonename = brr[0];
        }

        private static async void LocationAccesser()
        {
            try
            {
                var accessStatus = await Geolocator.RequestAccessAsync();

                switch (accessStatus)
                {
                    case GeolocationAccessStatus.Allowed:

                        // If DesiredAccuracy or DesiredAccuracyInMeters are not set (or value is 0), DesiredAccuracy.Default is used.
                        var geolocator = new Geolocator {DesiredAccuracy = PositionAccuracy.High};
                        // Carry out the operation
                        var pos = await geolocator.GetGeopositionAsync();
                        latitude = pos.Coordinate.Point.Position.Latitude.ToString();
                        longitude= pos.Coordinate.Point.Position.Longitude.ToString();
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

        private async void MessageSender()
        {
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("SmsOn"))
            {
                var messageDialog= new MessageDialog("SMS Permission Denied");
                await messageDialog.ShowAsync();
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
            var msg = new SmsTextMessage2
            {
                To = _phonenumber,
                Body = "I am in need of help. My coordinates are\n Latitude:"+ latitude+ "Longitude \n" + longitude
            };
            //CallandSMSSettings.number1.Text;


            //else
            //{
            //    Frame.Navigate(typeof(CallandSmsSettings));
            //}
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
