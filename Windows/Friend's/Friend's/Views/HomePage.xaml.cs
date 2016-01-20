using System;
using System.Diagnostics;
using System.Threading;
using Windows.Devices.Sms;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Friend_s.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage
    {
        public delegate void CallingInfoDelegate();

        public HomePage()
        {
            InitializeComponent();
        }
    }
}
