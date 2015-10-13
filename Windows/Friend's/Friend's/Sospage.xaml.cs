using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Devices.Sms;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Friend_s
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Sospage : Page
    {
        private SmsDevice2 _device;
        public delegate void CallingInfoDelegate();
        public event CallingInfoDelegate CellInfoUpdateCompleted;
        public event CallingInfoDelegate ActivePhoneCallStateChanged;
        private CancellationTokenSource _cts = null;

        public Sospage()
        {
            this.InitializeComponent();
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                var spineClass = new SpineClass();
                spineClass.InitializeCallingInfoAsync();
                MessageSender();
                Caller();
            }

        }

        public async void MessageSender()
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
            string msgStr = "";
            if (_device != null)
            {
                SmsTextMessage2 msg = new SmsTextMessage2();
                if (CallandSmsSettings.Number1 != "")
                {
                    msg.To = "7830207022"; //CallandSMSSettings.number1.Text;
                    msg.Body = "Hello!";
                }

                else
                {
                    Frame.Navigate(typeof(CallandSmsSettings));
                }

                SmsSendMessageResult result = await _device.SendMessageAndGetResultAsync(msg);

                if (result.IsSuccessful)
                {
                    msgStr = "";
                    msgStr += "Text message sent, cellularClass: " + result.CellularClass;
                    MessageDialog msg1= new MessageDialog("Message Sent!");
                    await msg1.ShowAsync();

                }
            }
        }

        public static async void Caller()
        {
            if ((SpineClass.CurrentPhoneLine != null))
            {
                SpineClass.CurrentPhoneLine.Dial("7830207022", "Prajjwal Dimri");
            }
            else
            {
                var dialog = new MessageDialog("No line found to place the call");
                await dialog.ShowAsync();
            }
        }
    }
}
