using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using Friend_s.Views;
using GalaSoft.MvvmLight.Command;
using Microsoft.QueryStringDotNET;
using NotificationsExtensions.Toasts;

namespace Friend_s.ViewModel
{
    public class ReminderPageViewModel : BaseViewModel
    {
        public ReminderPageViewModel()
        {
            ReminderRegisterCommand = new RelayCommand(ReminderRegister);
            ReminderRemoverCommand = new RelayCommand(ReminderRemover);
        }

        public RelayCommand ReminderRegisterCommand { get; set; }
        public RelayCommand ReminderRemoverCommand { get; set; }

        public TimeSpan Time { get; set; }


        private void ReminderRegister()
        {

            const string title = "Are you there?";
            const string content = "Click the button below if you are alright!";

            var visual = new ToastVisual()
            {
                TitleText = new ToastText()
                {
                    Text = title
                },

                BodyTextLine1 = new ToastText()
                {
                    Text = content
                }

            };

            var actions = new ToastActionsCustom()
            {
                Buttons =
                {
                    new ToastButton("Dismiss", new QueryString()
                    {
                        {"action", "dismiss"}
                    }.ToString())
                    {
                        ActivationType = ToastActivationType.Background
                    }
                }

            };

            const int conversationId = 177777;

            var toastContent = new ToastContent
            {
                Visual = visual,
                Actions = actions,
                Scenario = ToastScenario.Reminder,
                Launch = new QueryString
                {
                    {"action","viewConversation" },
                    {"conversationId", conversationId.ToString()}
                }.ToString()
            };

            if (Time > DateTime.Now.TimeOfDay)
            {
                try
                {
                    var scheduled = new ScheduledToastNotification(toastContent.GetXml(),
                        new DateTimeOffset(DateTime.Today + Time)) {Id = "scheduledtoast"};

                    var timeDifference = Time - DateTime.Now.TimeOfDay;
                    timeDifference = timeDifference.Add(new TimeSpan(0, 0, 30, 0));
                    const string taskName = "Reminder";
                    var taskBuilder = new BackgroundTaskBuilder();
                    taskBuilder.Name = taskName;
                    taskBuilder.TaskEntryPoint = typeof (BackgroundProcesses.Reminder).FullName;
                    taskBuilder.SetTrigger(new TimeTrigger(Convert.ToUInt32(timeDifference.Minutes.ToString()), true));

                    taskBuilder.Register();
                    ToastNotificationManager.CreateToastNotifier().AddToSchedule(scheduled);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
            else
            {
                try
                {
                    var scheduled = new ScheduledToastNotification(toastContent.GetXml(),
                        new DateTimeOffset(DateTime.Today.AddDays(1) + Time))
                    {Id = "scheduledtoast"};
                    
                    var timeDifference = Time.Add(new TimeSpan(1,0,0,0)) ;
                    timeDifference = timeDifference.Add(new TimeSpan(0, 0, 30, 0));
                    const string taskName = "Reminder";
                    var taskBuilder = new BackgroundTaskBuilder();
                    taskBuilder.Name = taskName;
                    taskBuilder.TaskEntryPoint = typeof (BackgroundProcesses.Reminder).FullName;
                    taskBuilder.SetTrigger(new TimeTrigger(Convert.ToUInt32(timeDifference.Minutes.ToString()), true));

                    ToastNotificationManager.CreateToastNotifier().AddToSchedule(scheduled);
                    taskBuilder.Register();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
            
        }

        private static void ReminderRemover()
        {
            var scheduled = ToastNotificationManager.CreateToastNotifier().GetScheduledToastNotifications();

            const string id = "scheduledtoast";

            foreach (var notify in scheduled.Where(notify => notify.Id == id))
            {
                ToastNotificationManager.CreateToastNotifier().RemoveFromSchedule(notify);
            }
        }
    }
}
