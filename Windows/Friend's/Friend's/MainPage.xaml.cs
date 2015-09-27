using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Calls;
using Windows.ApplicationModel.Chat;
using Windows.Devices.Sms;
using Windows.UI.Xaml.Automation.Peers;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Friend_s
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private PhoneLine currentPhoneLine;
        private SmsDevice2 device;
        private MainPage rootPage;

        public MainPage()
        {
            this.InitializeComponent();

        }

        

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void MenuButton3_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (Voice));
        }

        private async void Button_OnClick(object sender, RoutedEventArgs e)
        {
            
            if (device == null)
            {
                try
                {
                   device = SmsDevice2.GetDefault();
                }
                catch (Exception ex)
                {
                    textBox.Text = ex.Message;
                   return;
                }

            }
            string msgStr = "";
            if (device != null)
            {
                SmsTextMessage2 msg = new SmsTextMessage2();
                msg.To = "7830207022";
                msg.Body = "Hello!";



                SmsSendMessageResult result = await device.SendMessageAndGetResultAsync(msg);

                if (result.IsSuccessful)
                {
                    msgStr = "";
                    msgStr += "Text message sent, cellularClass: " + result.CellularClass.ToString();
                    textBox.Text = msgStr;

                }
            }
        }
    }
}
