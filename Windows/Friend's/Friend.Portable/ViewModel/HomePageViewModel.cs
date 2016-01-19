using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Friend_s.Portable.Model;

using GalaSoft.MvvmLight.Command;

namespace Friend_s.Portable.ViewModel
{
    public class HomePageViewModel : BaseViewModel
    {

        private readonly IDatabaseService _databaseService;

        public RelayCommand LocationAccesserCommand { get; private set; }

        public const string TestItemPropertyName = "TestItem";

        private ObservableCollection<TestItem> _testItems = new ObservableCollection<TestItem>();

        public ObservableCollection<TestItem> TestItems
        {
            get { return _testItems; }
            set { Set(TestItemPropertyName, ref _testItems, value); }
        }

        public HomePageViewModel()
        {
            this.LocationAccesserCommand = new RelayCommand(this.LocationAccesser);
        }

        public HomePageViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            Initialize();
        }

        private async Task Initialize()
        {
            try
            {
                TestItems = new ObservableCollection<TestItem>(await _databaseService.GetTestItems());
            }
            catch (Exception) { }
        }

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
                        TextBlockLoc.Text = (pos.Coordinate.Point.Position.Latitude) + "\n" +
                                            (pos.Coordinate.Point.Position.Longitude);
                        var location = new BasicGeoposition();

                        location.Latitude = Math.Round(pos.Coordinate.Point.Position.Latitude, 4);

                        location.Longitude = Math.Round(pos.Coordinate.Point.Position.Longitude, 4);
                        var pointToReverseGeopoint = new Geopoint(location);

                        var result =
                            await MapLocationFinder.FindLocationsAtAsync(pointToReverseGeopoint);

                        if (result.Status == MapLocationFinderStatus.Success)
                        {
                            LocationTextBox.Text =
                                  result.Locations[0].Address.Town;
                        }


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
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}