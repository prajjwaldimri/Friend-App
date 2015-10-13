using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Devices.Geolocation;
using Windows.Devices.Sms;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Friend_s
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        private SmsDevice2 _device;
        public delegate void CallingInfoDelegate();
        public event CallingInfoDelegate CellInfoUpdateCompleted;
        public event CallingInfoDelegate ActivePhoneCallStateChanged;
        private CancellationTokenSource _cts = null;
        

        public HomePage()
        {
            this.InitializeComponent();
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                try
                {
                    var spineClass = new SpineClass();
                    spineClass.InitializeCallingInfoAsync();
                }
                catch (Exception e1)
                {
                    String e = e1.ToString();
                }
            }

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LocationAccesser();
        }

        private async void LocationAccesser()
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


                        TextBlockLoc.Text = (pos.Coordinate.Point.Position.Latitude).ToString() + "\n" + (pos.Coordinate.Point.Position.Longitude).ToString();

                        break;
                    case GeolocationAccessStatus.Denied:
                        TextBlockLoc.Text = "Access Denied!";
                        break;

                    case GeolocationAccessStatus.Unspecified:
                        TextBlockLoc.Text = "Unspecified";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception)
            {
                TextBlockLoc.Text = "Error";
            }
        }

        
    }
}
