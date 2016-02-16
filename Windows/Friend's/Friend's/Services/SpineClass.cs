using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Calls;
using Windows.ApplicationModel.Contacts;
using Windows.Devices.Sms;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Newtonsoft.Json;

namespace Friend_s.Services
{
    public class SpineClass
    {
        public static PhoneLine CurrentPhoneLine;
        public static double LocLat ;
        public static double LocLon;
        public static string LocationAddress;
        public SmsDevice2 Device;
        public Views.MainPage RootPage;
        public static Dictionary<Guid, PhoneLine> AllPhoneLines;
        public bool DoesPhoneCallExist;
        public int NoOfLines;
        public delegate void CallingInfoDelegate();
        public event CallingInfoDelegate CellInfoUpdateCompleted;
        public event CallingInfoDelegate ActivePhoneCallStateChanged;
        public CancellationTokenSource Cts = null;
        

        #region phonecallsetters

        public async void InitializeCallingInfoAsync()
        {
            MonitorCallState();

            //Get all phone lines (To detect dual SIM devices)
            var getPhoneLinesTask = GetPhoneLinesAsync();
            AllPhoneLines = await getPhoneLinesTask;

            //Get number of lines
            NoOfLines = AllPhoneLines.Count;

            //Get Default Phone Line
            var getDefaultLineTask = GetDefaultPhoneLineAsync();
            CurrentPhoneLine = await getDefaultLineTask;


        }

        private void MonitorCallState()
        {
            try
            {
                PhoneCallManager.CallStateChanged += (o, args) =>
                {
                    DoesPhoneCallExist = PhoneCallManager.IsCallActive || PhoneCallManager.IsCallIncoming;
                    if (ActivePhoneCallStateChanged != null)
                    {
                        ActivePhoneCallStateChanged();
                    }
                };
            }
            catch(Exception e) { }
        }

        private async Task<Dictionary<Guid, PhoneLine>> GetPhoneLinesAsync()
        {
            PhoneCallStore store = await PhoneCallManager.RequestStoreAsync();

                // Start the PhoneLineWatcher
                var watcher = store.RequestLineWatcher();
                var phoneLines = new List<PhoneLine>();
                var lineEnumerationCompletion = new TaskCompletionSource<bool>();
                watcher.LineAdded += async (o, args) =>
                {
                    var line = await PhoneLine.FromIdAsync(args.LineId);
                    phoneLines.Add(line);
                };
                watcher.Stopped += (o, args) => lineEnumerationCompletion.TrySetResult(false);
                watcher.EnumerationCompleted += (o, args) => lineEnumerationCompletion.TrySetResult(true);
                watcher.Start();

                // Wait for enumeration completion
                if (!await lineEnumerationCompletion.Task)
                {
                    throw new Exception("Phone Line Enumeration failed");
                }

                watcher.Stop();

                Dictionary<Guid, PhoneLine> returnedLines = new Dictionary<Guid, PhoneLine>();

                foreach (PhoneLine phoneLine in phoneLines)
                {
                    if (phoneLine != null && phoneLine.Transport == PhoneLineTransport.Cellular)
                    {
                        returnedLines.Add(phoneLine.Id, phoneLine);
                    }
                }

                return returnedLines;
           
        }
        private async Task<PhoneLine> GetDefaultPhoneLineAsync()
        {
            var phoneCallStore = await PhoneCallManager.RequestStoreAsync();
            var lineId = await phoneCallStore.GetDefaultLineAsync();
            return await PhoneLine.FromIdAsync(lineId);
        }
        #endregion 

        public static string GetContactResult(Contact contact)
        {
            var result = new StringBuilder();
            //result.AppendFormat("Display name: {0}", contact.DisplayName);
            //result.AppendLine();

            //foreach (ContactEmail email in contact.Emails)
            //{
            //    result.AppendFormat("Email address ({0}): {1}", email.Kind, email.Address);
            //    result.AppendLine();
            //}

            foreach (ContactPhone phone in contact.Phones)
            {
                //result.AppendFormat("Phone ({0}): {1}", phone.Kind, phone.Number);
                result.AppendFormat("{0}", phone.Number);
                result.AppendLine();
            }

            return result.ToString();
        }

        public static async void ImagetoIsolatedStorageSaver(IRandomAccessStreamWithContentType stream, string filename)
        {
            try
            {
                //WriteableBitmap is used to save our image to our desired location, which in this case is the LocalStorage of app
                var image = new WriteableBitmap(50, 50);
                image.SetSource(stream);

                //Saving to roaming folder so that it can sync the profile pic to other devices
                var saveAsTarget =
                    await
                        ApplicationData.Current.RoamingFolder.CreateFileAsync(filename,
                            CreationCollisionOption.ReplaceExisting);

                //Encoding with Bitmap Encoder to jpeg format
                var encoder = await BitmapEncoder.CreateAsync(
                    BitmapEncoder.JpegEncoderId,
                    await saveAsTarget.OpenAsync(FileAccessMode.ReadWrite));
                //Saving as a stream
                var pixelStream = image.PixelBuffer.AsStream();
                var pixels = new byte[pixelStream.Length];
                await pixelStream.ReadAsync(pixels, 0, pixels.Length);
                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint) image.PixelWidth,
                    (uint) image.PixelHeight, 96.0, 96.0, pixels);
                await encoder.FlushAsync();
            }
            catch (Exception e)
            {
                Debug.Write(e);
            }
        }

        


    }
}
