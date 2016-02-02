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
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace BackgroundProcesses
{
    public sealed class SpineClass
    {
        internal static PhoneLine CurrentPhoneLine;
        internal static double LocLat;
        internal static double LocLon;
        internal static string LocationAddress;
        internal SmsDevice2 Device;
        internal static Dictionary<Guid, PhoneLine> AllPhoneLines;
        internal bool DoesPhoneCallExist;
        internal int NoOfLines;

        internal delegate void CallingInfoDelegate();

        private event CallingInfoDelegate CellInfoUpdateCompleted;
        private event CallingInfoDelegate ActivePhoneCallStateChanged;
        internal CancellationTokenSource Cts = null;


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
            catch (Exception e)
            {
            }
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
