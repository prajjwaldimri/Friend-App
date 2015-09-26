using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Friend_s
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Voice : Page
    {
        public Voice()
        {
            this.InitializeComponent();
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void MenuButton3_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Voice));
        }
    }
}