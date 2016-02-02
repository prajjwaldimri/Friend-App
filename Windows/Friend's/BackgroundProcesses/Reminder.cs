using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation;
using Windows.Devices.Sms;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;



namespace BackgroundProcesses
{
    public sealed class Reminder : IBackgroundTask
    {
       public void Run(IBackgroundTaskInstance taskInstance)
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            var details = taskInstance.TriggerDetails as ToastNotificationActionTriggerDetail;

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
                //TODO: Fireup SoSpage's Methods
            }
        }

        
    }
}
