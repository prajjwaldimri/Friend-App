// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;

namespace BeFriend.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Sospage
    {
        public Sospage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs ex)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += (s, e) =>
            {
                Frame.Navigate(typeof(MainPage));
            };
        }
    }
}
