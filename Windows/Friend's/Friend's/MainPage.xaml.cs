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


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Friend_s
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private PhoneLine currentPhoneLine;
        
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

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            if (currentPhoneLine != null)
            {
                currentPhoneLine.Dial("7830207022", "Prajjwal");
            }

            else
            {
                PhoneCallManager.ShowPhoneCallUI("7830207022","Prajjwal");
            }
        }
    }
}
