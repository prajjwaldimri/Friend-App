using System;
using System.Threading;
using Windows.Devices.Sms;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

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
            InitializeComponent();
            if (!Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
                return;
            var spineClass = new SpineClass();
            spineClass.InitializeCallingInfoAsync();
            DatabaseConnector();
            MessageSender();
            Caller();
        }

        private static string _phonenumber;
        private static string _phonename;
        private static void DatabaseConnector()
        {
            var list = CreateDatabase.GetValues(@"SELECT Number FROM Details WHERE ID=1");
            var list1 = CreateDatabase.GetValues(@"SELECT Name FROM Details WHERE ID=1");
            string[] arr = list;
            string[] brr = list1;
            _phonenumber = arr[0];
            _phonename = brr[0];
        }

        private async void MessageSender()
        {
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("SmsOn"))
            {
                var messageDialog= new MessageDialog("SMS Permission Denied");
                await messageDialog.ShowAsync();
                return;
            }
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
                To = _phonenumber,
                Body = "Hello!"
            };
            //CallandSMSSettings.number1.Text;


            //else
            //{
            //    Frame.Navigate(typeof(CallandSmsSettings));
            //}
             var result = await _device.SendMessageAndGetResultAsync(msg);
            
            if (!result.IsSuccessful) return;
            var msgStr = "";
            msgStr += "Text message sent, To: " + _phonenumber;
            var msg1 = new MessageDialog("Message Sent!" + msgStr);
            await msg1.ShowAsync();
        }

        private static async void Caller()
        {
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("CallOn"))
            {
           return;
            }
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
    }
}
