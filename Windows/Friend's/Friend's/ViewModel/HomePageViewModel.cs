using System;
using System.Diagnostics;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media.Imaging;
using Friend_s.Services;
using GalaSoft.MvvmLight.Command;

namespace Friend_s.ViewModel
{
    public class HomePageViewModel : BaseViewModel
    {
        public HomePageViewModel()
        {
           LocationAccesserCommand = new RelayCommand(LocationAccesser);
           ProfileImageSetterCommand = new RelayCommand(ProfileSetter);
           ProfileEllipseButtonTapCommand = new RelayCommand(EllipseTapped);
           SpineInitializerCommand = new RelayCommand(SpineInitializer);
           ThemeRetrieverCommand = new RelayCommand(ThemeInitializer);
        
        }
        //Variables

        private string _gpsLocationPoint;
        private string _gpsLocation;
        private BitmapImage _profileBitmapImage;
        
        //Commands and Services Declaration


        public RelayCommand LocationAccesserCommand { get; private set; }
        public RelayCommand ProfileImageSetterCommand { get; private set; }
        public RelayCommand ProfileEllipseButtonTapCommand { get; private set; }
        public RelayCommand SpineInitializerCommand { get; private set; }
        public RelayCommand ThemeRetrieverCommand { get; private set; }

        public BitmapImage ProfileImageSource => _profileBitmapImage;




        //String Values

        private string GpsLocationPoint => _gpsLocationPoint;

        public string GpsLocation => _gpsLocation;

        
        //Methods

        private async void LocationAccesser()
        {
            try
            {
                var accessStatus = await Geolocator.RequestAccessAsync();

                switch (accessStatus)
                {
                    case GeolocationAccessStatus.Allowed:

                        // If DesiredAccuracy or DesiredAccuracyInMeters are not set (or value is 0), DesiredAccuracy.Default is used.
                        var geolocator = new Geolocator();
                        geolocator.DesiredAccuracy = PositionAccuracy.High;
                        // Carry out the operation
                        var pos = await geolocator.GetGeopositionAsync();
                        _gpsLocationPoint = (pos.Coordinate.Point.Position.Latitude) + "\n" +
                                            (pos.Coordinate.Point.Position.Longitude);
                        var location = new BasicGeoposition();

                        location.Latitude = Math.Round(pos.Coordinate.Point.Position.Latitude, 4);

                        location.Longitude = Math.Round(pos.Coordinate.Point.Position.Longitude, 4);
                        var pointToReverseGeopoint = new Geopoint(location);

                        var result =
                            await MapLocationFinder.FindLocationsAtAsync(pointToReverseGeopoint);

                        if (result.Status == MapLocationFinderStatus.Success)
                        {
                            _gpsLocation =
                                  result.Locations[0].Address.Town;
                        }


                        break;
                    case GeolocationAccessStatus.Denied:
                        _gpsLocation = "Access Denied!";
                        break;

                    case GeolocationAccessStatus.Unspecified:
                        _gpsLocation = "Unspecified";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            RaisePropertyChanged(()=> GpsLocationPoint);
            RaisePropertyChanged(()=>GpsLocation);
        }

        private async void ProfileSetter()
        {
            try
            {
                var image1 = new BitmapImage();
                var storagefile =
                    await ApplicationData.Current.RoamingFolder.GetFileAsync("profiledefault.jpg");
                if (storagefile == null) return;
                var stream = await storagefile.OpenAsync(FileAccessMode.Read);
                //Converts to a IRandomAccessStream that can be set to an image
                await image1.SetSourceAsync(stream);
                _profileBitmapImage = image1;
                RaisePropertyChanged(()=> ProfileImageSource);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        private async void EllipseTapped()
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
            _profileBitmapImage = image1;
            
            SpineClass.ImagetoIsolatedStorageSaver(stream1, "profiledefault.jpg");
            RaisePropertyChanged(() => ProfileImageSource);
        }

        private static void SpineInitializer()
        {
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

        private static void ThemeInitializer()
        {
            var applicationData = ApplicationData.Current;
            var localsettings = applicationData.LocalSettings;
            var bvm = new BaseViewModel();
            if (localsettings.Values != null) bvm.ThemeColor = localsettings.Values["ThemeColor"] as string;
            bvm.RaisePropertyChangedBase();
        }
    }
}