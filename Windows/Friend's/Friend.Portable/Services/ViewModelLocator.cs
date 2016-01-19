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

using Friend_s.Portable.Design;
using Friend_s.Portable.Model;
using Friend_s.Portable.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace Friend_s.Portable.Services
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

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
                SimpleIoc.Default.Register<IDatabaseService,DesignDatabaseService>();
               
            }
            else
            {
                SimpleIoc.Default.Register<IDatabaseService, DatabaseService>();
            }

            SimpleIoc.Default.Register<HomePageViewModel>();
            SimpleIoc.Default.Register<CallandSettingsPageViewModel>();
        }

        public HomePageViewModel HomePage
        {
            get { return ServiceLocator.Current.GetInstance<HomePageViewModel>(); }
        }

        public CallandSettingsPageViewModel CallandSettingsPage
        {
            get { return ServiceLocator.Current.GetInstance<CallandSettingsPageViewModel>(); }
        }
        
    }
}