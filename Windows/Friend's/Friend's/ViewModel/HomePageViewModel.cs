using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Friend_s.Portable.Model;
using Friend_s.Portable.ViewModel;
using GalaSoft.MvvmLight.Command;

namespace Friend_s.ViewModel
{
    public class HomePageViewModel : BaseViewModel
    {
        public HomePageViewModel()
        {
           LocationAccesserCommand = new RelayCommand(LocationAccesser);
           ProfileImageSetterCommand = new RelayCommand(ProfileSetter);
        }
        //Variables

        private string _gpsLocationPoint;
        private string _gpsLocation;
        private BitmapImage _profileBitmapImage;


        //Commands and Services Declaration

        private readonly IDatabaseService _databaseService;

        public RelayCommand LocationAccesserCommand { get; private set; }
        public RelayCommand ProfileImageSetterCommand { get; private set; }

        public BitmapImage ProfileImageSource
        {
            get {return _profileBitmapImage;}
            
        }
        

        public const string TestItemPropertyName = "TestItem";

        private ObservableCollection<TestItem> _testItems = new ObservableCollection<TestItem>();


        //String Values
        public ObservableCollection<TestItem> TestItems
        {
            get { return _testItems; }
            set { Set(TestItemPropertyName, ref _testItems, value); }
        }

        public string GpsLocationPoint
        {
            get { return _gpsLocationPoint; }
        }

        public string GpsLocation
        {
            get { return _gpsLocation;}
        }

        //public HomePageViewModel()
        //{
        //    this.LocationAccesserCommand = new RelayCommand(this.LocationAccesser);
        //}

        //public HomePageViewModel(IDatabaseService databaseService)
        //{
        //    _databaseService = databaseService;
        //    Initialize();
        //}

        private async Task Initialize()
        {
            try
            {
                TestItems = new ObservableCollection<TestItem>(await _databaseService.GetTestItems());
            }
            catch (Exception) { }
        }


        //Methods

        public async void LocationAccesser()
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
    }
}