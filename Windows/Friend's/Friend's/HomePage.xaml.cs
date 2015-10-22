using System;
using System.Threading;
using Windows.Devices.Geolocation;
using Windows.Devices.Sms;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
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
                    var e = e1.ToString();
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
                        var geolocator = new Geolocator { DesiredAccuracyInMeters = 0 };

                        // Carry out the operation
                        var pos = await geolocator.GetGeopositionAsync();


                        TextBlockLoc.Text = (pos.Coordinate.Point.Position.Latitude) + "\n" + (pos.Coordinate.Point.Position.Longitude);

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


        private async void EllipseButton_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var image = new BitmapImage();
            var fp = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            fp.FileTypeFilter.Add(".jpeg");
            fp.FileTypeFilter.Add(".png");
            fp.FileTypeFilter.Add(".jpg");
            // Using PickSingleFileAsync() will return a storage file which can be saved into an object of storage file class.          
            var sf = await fp.PickSingleFileAsync();

            if (sf == null) return;
            // Adding bitmap image object to store the stream provided by the object of StorageFile defined above.BitmapImage bmp = new BitmapImage();           
            // Reading file as a stream and saving it in an object of IRandomAccess.         
            var stream = await sf.OpenAsync(FileAccessMode.Read);
            // Adding stream as source of the bitmap image object defined above     

            await image.SetSourceAsync(stream);

            CreateDatabase.InsertData("7",stream.ToString(),null);

            Ellipseimg.ImageSource = image;
        }
    }
}
