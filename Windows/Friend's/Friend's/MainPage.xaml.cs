using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Friend_s
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            SplitListBox.SelectedItem = HomeListItem;
            MainFrame.Navigate(typeof (HomePage));
            HomeListItem.IsSelected = true;

        }

        

        private void HamburgerButton_OnClick(object sender, RoutedEventArgs e)
        {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
        }

        private void SplitListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HomeListItem.IsSelected)
            {
                MainFrame.Navigate(typeof (HomePage));
            }
            else if (SettingsListItem.IsSelected)
            {
                MainFrame.Navigate(typeof (CallandSmsSettings));
            }
            else if (SosListItem.IsSelected)
            {
                MainFrame.Navigate(typeof (Sospage));
            }
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
        }
    }
}
