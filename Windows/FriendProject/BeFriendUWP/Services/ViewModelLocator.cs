/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Friend.Portable"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using BeFriend.ViewModel;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace BeFriend.Services
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<HomePageViewModel>();
            SimpleIoc.Default.Register<CallandSettingsPageViewModel>();
            SimpleIoc.Default.Register<TimerPageViewModel>();
            SimpleIoc.Default.Register<ReminderPageViewModel>();
            SimpleIoc.Default.Register<SosPageViewModel>();
            SimpleIoc.Default.Register<BaseViewModel>();
            SimpleIoc.Default.Register<ChatBotPageViewModel>();
            SimpleIoc.Default.Register<AboutPageViewModel>();

        }

        public HomePageViewModel HomePage
        {
            get { return ServiceLocator.Current.GetInstance<HomePageViewModel>(); }
        }

        public CallandSettingsPageViewModel CallandSettingsPage
        {
            get { return ServiceLocator.Current.GetInstance<CallandSettingsPageViewModel>(); }
        }

        public TimerPageViewModel TimerPage
        {
            get { return ServiceLocator.Current.GetInstance<TimerPageViewModel>(); }
        }

        public ReminderPageViewModel ReminderPage
        {
            get { return ServiceLocator.Current.GetInstance<ReminderPageViewModel>(); }
        }

        public SosPageViewModel SosPage
        {
            get { return ServiceLocator.Current.GetInstance<SosPageViewModel>(); }
        }

        public BaseViewModel BaseView
        {
            get { return ServiceLocator.Current.GetInstance<BaseViewModel>(); }
        }

        public ChatBotPageViewModel ChatBotView
        {
            get { return ServiceLocator.Current.GetInstance<ChatBotPageViewModel>(); }
        }

        public AboutPageViewModel AboutPageView
        {
            get { return ServiceLocator.Current.GetInstance<AboutPageViewModel>(); }
        }
        
    }
}