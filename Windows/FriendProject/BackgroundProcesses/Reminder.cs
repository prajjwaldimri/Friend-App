using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation;
using Windows.Devices.Sms;
using Windows.Foundation.Collections;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Newtonsoft.Json;
using Tweetinvi;
using winsdkfb;
using winsdkfb.Graph;

namespace BackgroundProcesses
{
    public sealed class Reminder : IBackgroundTask
    {
        private static SmsDevice2 _device;

        private delegate void CallingInfoDelegate();

        private event CallingInfoDelegate CellInfoUpdateCompleted;
        private event CallingInfoDelegate ActivePhoneCallStateChanged;
        private CancellationTokenSource _cts = null;
        private static string _latitude;
        private static string _longitude;
        private static string _message="";

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            var localsettings = ApplicationData.Current.LocalSettings;
            if (localsettings.Values.ContainsKey("MessageToSend"))
            {
                _message = localsettings.Values["MessageToSend"] as string;
            }
            else
            {
                _message = "Help Me at";
            }

            var details = taskInstance.TriggerDetails as ToastNotificationActionTriggerDetail;

            if (!localSettings.Values.ContainsKey("EmergencyOn"))
            {
                localSettings.Values.Add("EmergencyOn",true);
            }

            if (details != null)
            {
                if (localSettings.Values.ContainsKey("EmergencyOn"))
                {
                    localSettings.Values.Remove("EmergencyOn");
                }
                return;
            }

            if (localSettings.Values.ContainsKey("EmergencyOn"))
            {
                TimerStarter();
                Caller();
            }
        }


        private static async void TimerStarter()
        {
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
            if (!Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
                return;
            var spineClass = new SpineClass();
            spineClass.InitializeCallingInfoAsync();
           
        }

        
        private static string _phonenumber;
        private static string _phonename;
        


        private static async Task LocationAccesser()
        {
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

        private static async void MessageSender(string contactNumber)
        {
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
                To = contactNumber,
                Body = "I am in need of help. My coordinates are\n Latitude:" + _latitude + "Longitude \n" + _longitude
            };
            var result = await _device.SendMessageAndGetResultAsync(msg);

            if (!result.IsSuccessful) return;
            var msgStr = "";
            msgStr += "Text message sent, To: " + _phonenumber;
            var msg1 = new MessageDialog("Message Sent!" + msgStr);
            await msg1.ShowAsync();
        }

        private static async void Caller()
        {
            if (!Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.ApplicationModel.Calls"))
                return;

            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("CallOn"))
            {
                return;
            }
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("FirstContactNumber")) return;
            _phonenumber = ApplicationData.Current.LocalSettings.Values["FirstContactNumber"] as string;

            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("FirstContactName")) return;
            _phonename = ApplicationData.Current.LocalSettings.Values["FirstContactName"] as string;

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

        private static async void TwitterPoster()
        {
            Debug.WriteLine("Checking credentials... \n");
            var vault = new PasswordVault();
            try
            {
                var credentialList = vault.FindAllByUserName("TwitterAccessToken");
                if (credentialList.Count <= 0)
                {
                    Debug.WriteLine("Twitter not configured \n");
                    return;
                }
                var twitteraccesstoken = vault.Retrieve("Friend", "TwitterAccessToken");
                var twitteraccesstokensecret = vault.Retrieve("Friend", "TwitterAccessTokenSecret");
                Debug.WriteLine("Credentials Retrieved \n");

                // Set up your credentials (https://apps.twitter.com)
                //Use your own consumerKey and consumerSecret below!
                await AuthTokens.KeyRetriever();
                Auth.SetUserCredentials(AuthTokens.TwitterConsumerKey, AuthTokens.TwitterConsumerSecret,
                    twitteraccesstoken.Password, twitteraccesstokensecret.Password);

                await LocationAccesser();
                //TODO: Publish the Tweet with location on your Timeline
                Tweet.PublishTweet(_message + " \n" + _latitude + "\n" + _longitude);

               Debug.WriteLine("Publishing Tweet... \n");
            }
            catch (Exception e)
            {
                Debug.WriteLine("Twitter not configured \n");
                Debug.WriteLine(e);
            }

            
        }

        private static async void FacebookPoster()
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
                parameters.Add("message", _message + "\n" + "\n" + _latitude + "\n" + _longitude);

                // Set Graph api path
                var path = "/" + user.Id + "/feed";

                var factory = new FBJsonClassFactory(s => {
                    return JsonConvert.DeserializeObject<FBReturnObject>(s);
                });

                var singleValue = new FBSingleValue(path, parameters, factory);
                var result = await singleValue.PostAsync();
                if (result.Succeeded)
                {
                    Debug.WriteLine("Succeed");
                }
                else
                {
                    Debug.WriteLine("Failed");
                }
            }
        }

    }
    public sealed class FBReturnObject
    {
        public string Id { get; set; }
        public string Post_Id { get; set; }
    }
}
