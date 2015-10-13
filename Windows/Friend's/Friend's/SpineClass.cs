using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Calls;
using Windows.Devices.Geolocation;
using Windows.Devices.Sms;
using Windows.System;



namespace Friend_s
{
    public class SpineClass
    {
        public static PhoneLine CurrentPhoneLine;
        public SmsDevice2 Device;
        public MainPage RootPage;
        public static Dictionary<Guid, PhoneLine> AllPhoneLines;
        public bool DoesPhoneCallExist;
        public int NoOfLines;
        public delegate void CallingInfoDelegate();
        public event CallingInfoDelegate CellInfoUpdateCompleted;
        public event CallingInfoDelegate ActivePhoneCallStateChanged;
        public CancellationTokenSource Cts = null;
        public static double LocLat;
        public static double LocLon;

        public async void Initializer()
        {
          

            
        }

         

        #region phonecallsetters

        public async void InitializeCallingInfoAsync()
        {
            this.MonitorCallState();

            //Get all phone lines (To detect dual SIM devices)
            Task<Dictionary<Guid, PhoneLine>> getPhoneLinesTask = GetPhoneLinesAsync();
            AllPhoneLines = await getPhoneLinesTask;

            //Get number of lines
            NoOfLines = AllPhoneLines.Count;

            //Get Default Phone Line
            Task<PhoneLine> getDefaultLineTask = GetDefaultPhoneLineAsync();
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
    }
}