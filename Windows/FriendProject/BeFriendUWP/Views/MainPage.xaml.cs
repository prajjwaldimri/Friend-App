using System;
using System.Collections.Generic;
using System.Linq;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Band;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BeFriend.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void StoreRateButton_OnClick(object sender, RoutedEventArgs e)
        {
            //TODO: Replace with correct Product ID
            var uri = new Uri("ms-windows-store://review/?ProductId=9NBLGGH4Q8NG");
            await Launcher.LaunchUriAsync(uri);
        }

        
    }
}

       