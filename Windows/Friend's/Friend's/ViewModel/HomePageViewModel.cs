using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Autofac.Core.Registration;
using Friend_s.Services;
using Friend_s.Views;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

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
            ButtonClickEventHome = new RelayCommand<object>(ButtonClickEventMethod);
        }

        
        //Variables

        //Commands and Services Declaration


        public RelayCommand LocationAccesserCommand { get; private set; }
        public RelayCommand ProfileImageSetterCommand { get; private set; }
        public RelayCommand ProfileEllipseButtonTapCommand { get; private set; }
        public RelayCommand SpineInitializerCommand { get; private set; }
        public RelayCommand<object> ButtonClickEventHome { get; private set; }

        public BitmapImage ProfileImageSource { get; private set; }
        
        //String Values
        private string GpsLocationPoint { get; set; }
        public string GpsLocation { get; private set; }


        //Methods
        private async void LocationAccesser()
        {
            try
            {
                MessengerInstance.Send(new NotificationMessage("ProgressBarEnable"));
                var accessStatus = await Geolocator.RequestAccessAsync();

                switch (accessStatus)
                {
                    case GeolocationAccessStatus.Allowed:

                        // If DesiredAccuracy or DesiredAccuracyInMeters are not set (or value is 0), DesiredAccuracy.Default is used.
                        var geolocator = new Geolocator();
                        geolocator.DesiredAccuracy = PositionAccuracy.High;
                        // Carry out the operation
                        var pos = await geolocator.GetGeopositionAsync();
                        GpsLocationPoint = (pos.Coordinate.Point.Position.Latitude) + "\n" +
                                            (pos.Coordinate.Point.Position.Longitude);
                        var location = new BasicGeoposition();

                        location.Latitude = Math.Round(pos.Coordinate.Point.Position.Latitude, 4);

                        location.Longitude = Math.Round(pos.Coordinate.Point.Position.Longitude, 4);
                        var pointToReverseGeopoint = new Geopoint(location);

                        var result =
                            await MapLocationFinder.FindLocationsAtAsync(pointToReverseGeopoint);

                        if (result.Status == MapLocationFinderStatus.Success)
                        {
                            GpsLocation =
                                  result.Locations[0].Address.Town;
                        }


                        break;
                    case GeolocationAccessStatus.Denied:
                        GpsLocation = "Access Denied!";
                        break;

                    case GeolocationAccessStatus.Unspecified:
                        GpsLocation = "Unspecified";
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
            MessengerInstance.Send(new NotificationMessage("ProgressBarDisable"));
        }

        private async void ProfileSetter()
        {
            try
            {
                var image1 = new BitmapImage();
                var storagefile =
                    await ApplicationData.Current.RoamingFolder.GetFileAsync("profiledefault.jpg");
                if (storagefile == null)
                {
                    ProfileImageSource = new BitmapImage(new Uri("ms-appx:/Assets/Generic-Profile-Image.jpg"));
                    RaisePropertyChanged(() => ProfileImageSource);
                }
                else
                {
                    var stream = await storagefile.OpenAsync(FileAccessMode.Read);
                    //Converts to a IRandomAccessStream that can be set to an image
                    await image1.SetSourceAsync(stream);
                    ProfileImageSource = image1;
                    RaisePropertyChanged(() => ProfileImageSource);
                }
            }
            catch (FileNotFoundException)
            {
                ProfileImageSource = new BitmapImage(new Uri("ms-appx:/Assets/Generic-Profile-Image.jpg"));
                RaisePropertyChanged(() => ProfileImageSource);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                ProfileImageSource = new BitmapImage(new Uri("ms-appx:/Assets/Generic-Profile-Image.jpg"));
                RaisePropertyChanged(() => ProfileImageSource);
            }
        }

        private async void EllipseTapped()
        {
            try
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
                ProfileImageSource = image1;

                SpineClass.ImagetoIsolatedStorageSaver(stream1, "profiledefault.jpg");
                RaisePropertyChanged(() => ProfileImageSource);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                var messageDialog = new MessageDialog(ex.ToString());
                await messageDialog.ShowAsync();

            }
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

        private void ButtonClickEventMethod(object obj)
        {
            var parameter = Convert.ToInt32(obj);
            switch (parameter)
            {
                case 1:
                    var frame = Window.Current.Content as Frame;
                    if (frame != null)
                        frame.Navigate(typeof(Sospage));
                    break;

                case 2:
                    var frame1 = Window.Current.Content as Frame;
                    break;

                default:
                    Debug.WriteLine("Default case hit on HomePage");
                    break;
            }
        }

    }
}