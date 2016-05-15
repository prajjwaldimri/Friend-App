using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BeFriend.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AboutPage : UserControl
    {
        public AboutPage()
        {
            this.InitializeComponent();
            VersionTextBlock.Text = GetAppVersion();
        }

        private static string GetAppVersion()
        {

            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";

        }
    }
}
