using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Devices.Sms;
using Windows.Foundation.Collections;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using BeFriend.Services;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using Tweetinvi;
using Tweetinvi.Credentials;
using winsdkfb;
using winsdkfb.Graph;

namespace BeFriend.ViewModel
{
    public class SosPageViewModel : BaseViewModel
    {
        private static SmsDevice2 _device;
        public delegate void CallingInfoDelegate();
        public event CallingInfoDelegate CellInfoUpdateCompleted;
        public event CallingInfoDelegate ActivePhoneCallStateChanged;
        private CancellationTokenSource _cts = null;
        private static string _latitude;
        private static string _longitude;

        public string SosPageText { get; set; }

        private string Message;

        public SosPageViewModel()
        {
            TimerStarterCommand = new RelayCommand(TimerStarter);
            SosCommand = new RelayCommand(SosCommandMethod);
        }

        public RelayCommand TimerStarterCommand { get; set; }
        public RelayCommand SosCommand { get; set; }

        private async void TimerStarter()
        {
            SosPageText += "Timer Started \n";
            RaisePropertyChanged(()=>SosPageText);
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(300);
            timer.Tick += timer_Tick;
            timer.Start();

            if (!Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
                return;
            var spineClass = new SpineClass();
            spineClass.InitializeCallingInfoAsync();
            
        }

        private async void timer_Tick(object sender, object e)
        {
            await LocationAccesser();

            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
                return;

            /*Message Sending Methods */
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("FirstContactNumber")) return;
            _phonenumber = ApplicationData.Current.LocalSettings.Values["FirstContactNumber"] as string;
            MessageSender(_phonenumber);
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("SecondContactNumber")) return;
            _phonenumber = ApplicationData.Current.LocalSettings.Values["SecondContactNumber"] as string;
            MessageSender(_phonenumber);
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("ThirdContactNumber")) return;
            _phonenumber = ApplicationData.Current.LocalSettings.Values["ThirdContactNumber"] as string;
            MessageSender(_phonenumber);

        }


        private static string _phonenumber;
        private static string _phonename;

        private async void SosCommandMethod()
        {
            var localsettings = ApplicationData.Current.LocalSettings;
            if (localsettings.Values.ContainsKey("MessageToSend"))
            {
                Message = localsettings.Values["MessageToSend"] as string;
            }
            else
            {
                Message = "Help Me at";
            }
            await LocationAccesser();

            /*Message Sending Methods */
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("FirstContactNumber")) return;
            _phonenumber = ApplicationData.Current.LocalSettings.Values["FirstContactNumber"] as string;
            MessageSender(_phonenumber);
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("SecondContactNumber")) return;
            _phonenumber = ApplicationData.Current.LocalSettings.Values["SecondContactNumber"] as string;
            MessageSender(_phonenumber);
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("ThirdContactNumber")) return;
            _phonenumber = ApplicationData.Current.LocalSettings.Values["ThirdContactNumber"] as string;
            MessageSender(_phonenumber);

            
            TwitterPoster();
            FacebookPoster();
            Caller();
        }

        private async Task LocationAccesser()
        {
            SosPageText += "Trying to get location... \n";
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
                            Latitude = pos.Coordinate.Point.Position.Latitude,
                            Longitude = pos.Coordinate.Point.Position.Longitude
                        };
                        SosPageText += "Location accessed... \n" + _latitude + "\n" + _longitude ;

                        break;
                    case GeolocationAccessStatus.Denied:
                        Debug.WriteLine("Access Denied!");
                        SosPageText += "Access Denied \n";
                        break;

                    case GeolocationAccessStatus.Unspecified:
                        Debug.WriteLine("Unspecified");
                        SosPageText += "Unknown error \n";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            RaisePropertyChanged(() => SosPageText);
        }

        private async void MessageSender(string contactNumber)
        {
            if (_device == null)
            {
                try
                {
                    _device = SmsDevice2.GetDefault();
                    
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    return;
                }

            }
            //if (_device == null) return;

            
            var msg = new SmsTextMessage2
            {
                To = contactNumber,
                Body = Message+"\t My coordinates are\n Latitude:" + _latitude + "Longitude \n" + _longitude
            };
            var result = await _device.SendMessageAndGetResultAsync(msg);
            SosPageText += "Sending Message.... \n";
            RaisePropertyChanged(() => SosPageText);

            if (!result.IsSuccessful)
            {
                if (result.NetworkCauseCode.Equals(50))
                {
                    SosPageText += "\n Network Error in sending SMS. Possibly no balance!";
                }
                SosPageText += "Message Sending Failed \n";
                RaisePropertyChanged(() => SosPageText);
                return;
            }
            var msgStr = "";
            msgStr += "Text message sent, To: " + _phonenumber;
            SosPageText += msgStr+"\n";
            RaisePropertyChanged(()=>SosPageText);
        }

        private void Caller()
        {
            //TODO: Not working on phones too
            //if (!Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.ApplicationModel.Calls"))
            //    return;

            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("FirstContactNumber"))
            {
                SosPageText += "Contacts not assigned \n";
                return;
            }
            _phonenumber = ApplicationData.Current.LocalSettings.Values["FirstContactNumber"] as string;

            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("FirstContactName"))
            {
                return;
            }
            _phonename = ApplicationData.Current.LocalSettings.Values["FirstContactName"] as string;

            if ((SpineClass.CurrentPhoneLine != null))
            {
                SosPageText += "Calling... \n";
                SpineClass.CurrentPhoneLine.Dial(_phonenumber, _phonename);
            }
            else
            {
                SosPageText += "No line found to place the call. No SIM or system not a mobile phone \n";
            }
            RaisePropertyChanged(()=>SosPageText);
        }

        private async void TwitterPoster()
        {
            SosPageText += "Checking credentials... \n";
            var vault = new PasswordVault();
            try
            {
                var credentialList = vault.FindAllByUserName("TwitterAccessToken");
                if (credentialList.Count <= 0)
                {
                    SosPageText += "Twitter not configured \n";
                    return;
                }
                var twitteraccesstoken = vault.Retrieve("Friend", "TwitterAccessToken");
                var twitteraccesstokensecret = vault.Retrieve("Friend", "TwitterAccessTokenSecret");
                SosPageText += "Credentials Retrieved \n";

                // Set up your credentials (https://apps.twitter.com)
                //Use your own consumerKey and consumerSecret below!
                await AuthTokens.KeyRetriever();
                Auth.SetUserCredentials(AuthTokens.TwitterConsumerKey, AuthTokens.TwitterConsumerSecret,
                    twitteraccesstoken.Password, twitteraccesstokensecret.Password);

                
                //TODO: Publish the Tweet with location on your Timeline
                Tweet.PublishTweet(Message+" \n" + _latitude + "\n" + _longitude);

                SosPageText += "Publishing Tweet... \n";
            }
            catch (Exception e)
            {
                SosPageText += "Twitter not configured \n";
                Debug.WriteLine(e);
            }

            RaisePropertyChanged(()=>SosPageText);
        }

        private async void FacebookPoster()
        {
            // Get active session
            FBSession sess = FBSession.ActiveSession;


            if (sess.LoggedIn)
            {
                var user = sess.User;
                // Set caption, link and description parameters
                var parameters = new PropertySet();
                
                // Add post message
                await LocationAccesser();
                parameters.Add("message", Message+"\n"+"\n"+_latitude+"\n"+_longitude);

                // Set Graph api path
                var path = "/" + user.Id + "/feed";

                var factory = new FBJsonClassFactory(s => {
                                                              return JsonConvert.DeserializeObject<FBReturnObject>(s);
                });

                var singleValue = new FBSingleValue(path, parameters, factory);
                var result = await singleValue.PostAsync();
                if (result.Succeeded)
                {
                    SosPageText += "\n Posted to Facebook Wall";
                }
                else
                {
                    SosPageText += "\n Can't post to Facebook Wall";
                }
            }
            RaisePropertyChanged(()=>SosPageText);
        }

        
    }

    public class FBReturnObject
    {
        public string Id { get; set; }
        public string Post_Id { get; set; }
    }

}
