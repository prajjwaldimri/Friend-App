using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Windows.Devices.Geolocation;
using Windows.Devices.Sms;
using Windows.Services.Maps;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Friend_s.Portable.Model;
using Friend_s.Portable.ViewModel;

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
        private CancellationTokenSource _cts = null;


        public HomePage()
        {
            this.InitializeComponent();
            if (!Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
                return;
            try
            {
                var spineClass = new SpineClass();
                spineClass.InitializeCallingInfoAsync();
            }
            catch (Exception e1)
            {
                Debug.WriteLine(e1);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //ProfileSetter();
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
            var stream1 = await storagefile.OpenReadAsync();
            
            // Adding stream as source of the bitmap image object defined above     
            await image1.SetSourceAsync(stream);
            Ellipseimg.ImageSource = image1;

            SpineClass.ImagetoIsolatedStorageSaver(stream1,"profiledefault.jpg");
            
        }

        private void listView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
