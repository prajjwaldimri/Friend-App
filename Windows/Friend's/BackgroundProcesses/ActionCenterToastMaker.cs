using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.WebUI;
using Microsoft.QueryStringDotNET;
using NotificationsExtensions.Toasts;


namespace BackgroundProcesses
{
    public sealed class ActionCenterToastMaker: IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            ToastGenerator();
            
            
        }

        private void ToastGenerator()
        {
            const string title = "Click on time of Emergency!";

            var visual = new ToastVisual()
            {
                TitleText = new ToastText()
                {
                    Text = title
                }

            };

            const int conversationId = 177777;

            var toastContent = new ToastContent()
            {
                Visual = visual,
                Launch = new QueryString()
                {
                    {"conversationId", conversationId.ToString()}
                }.ToString()
            };

            var toast = new ToastNotification(toastContent.GetXml())
            {
                ExpirationTime = DateTime.Now.AddHours(10),
                SuppressPopup = true,
                Tag = "Friends"
            };
            if (ToastNotificationManager.History != null) ToastNotificationManager.History.Remove("Friends");
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
