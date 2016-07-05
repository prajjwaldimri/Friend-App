using System;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            var uri = new Uri("ms-windows-store://review/?ProductId=9nblggh4q864");
            await Launcher.LaunchUriAsync(uri);
        }


        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var msg = new MessageDialog("Coming Soon!");
            await msg.ShowAsync();
        }
    }
}

       