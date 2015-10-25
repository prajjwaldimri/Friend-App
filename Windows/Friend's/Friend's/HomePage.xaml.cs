using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Devices.Sms;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

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
            ProfileSetter();
        }

        private async void ProfileSetter()
        {
            try
            {
                var image1 = new BitmapImage();
                var storagefile =
                    await Windows.Storage.ApplicationData.Current.RoamingFolder.GetFileAsync("profiledefault.jpg");
                if (storagefile == null) return;
                var stream = await storagefile.OpenAsync(FileAccessMode.Read);
                //Converts to a IRandomAccessStream that can be set to an image
                await image1.SetSourceAsync(stream);
                Ellipseimg.ImageSource = image1;
            }
            catch(Exception)
            {
                
            }
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
                        var geolocator = new Geolocator {DesiredAccuracyInMeters = 0};

                        // Carry out the operation
                        var pos = await geolocator.GetGeopositionAsync();


                        TextBlockLoc.Text = (pos.Coordinate.Point.Position.Latitude) + "\n" +
                                            (pos.Coordinate.Point.Position.Longitude);

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
            
            // Reading file as a stream and saving it in an object of IRandomAccess.         
            var stream = await storagefile.OpenAsync(FileAccessMode.Read);
            
            // Adding stream as source of the bitmap image object defined above     
            await image1.SetSourceAsync(stream);
            Ellipseimg.ImageSource = image1;

            //WriteableBitmap is used to save our image to our desired location, which in this case is the LocalStorage of app
            var image = new WriteableBitmap(50, 50);
            var stream1 = await storagefile.OpenReadAsync();
            image.SetSource(stream1);

            //Saving to roaming folder so that it can sync the profile pic to other devices
            var saveAsTarget =
                    await ApplicationData.Current.RoamingFolder.CreateFileAsync("profiledefault.jpg",CreationCollisionOption.ReplaceExisting);
            
            //Encoding with Bitmap Encoder to jpeg format
            var encoder = await BitmapEncoder.CreateAsync(
                BitmapEncoder.JpegEncoderId,
                await saveAsTarget.OpenAsync(FileAccessMode.ReadWrite));
            //Saving as a stream
            var pixelStream = image.PixelBuffer.AsStream();
            var pixels = new byte[pixelStream.Length];
            await pixelStream.ReadAsync(pixels, 0, pixels.Length);
            encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)image.PixelWidth, (uint)image.PixelHeight, 96.0, 96.0, pixels);
            await encoder.FlushAsync();

        }
    }
}
