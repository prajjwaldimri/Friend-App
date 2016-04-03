using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using BeFriend.Views;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Friend_s.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FirstTimeTutorial : Page
    {
        public FirstTimeTutorial()
        {
            this.InitializeComponent();
            BackgroundAnimation.Begin();
            
        }

        private void Pivot_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (MainPivot.SelectedIndex)
            {
                case 0:
                    FirstButton.IsChecked = true;
                    TutorialProgressBar.Value = 20;
                    break;

                case 1:
                    SecondButton.IsChecked = true;
                    TutorialProgressBar.Value = 40;
                    break;

                case 2:
                    ThirdButton.IsChecked = true;
                    TutorialProgressBar.Value = 60;
                    break;

                case 3:
                    FourthButton.IsChecked = true;
                    TutorialProgressBar.Value = 80;
                    NextButton.Visibility = Visibility.Visible;
                    FinishButton.Visibility = Visibility.Collapsed;
                    break;

                case 4:
                    FifthButton.IsChecked = true;
                    TutorialProgressBar.Value = 100;
                    FinishButton.Visibility = Visibility.Visible;
                    NextButton.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void NextButton_OnClick(object sender, RoutedEventArgs e)
        {
            if(MainPivot.SelectedIndex<5)
            MainPivot.SelectedIndex += 1;
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            if(MainPivot.SelectedIndex>0)
            MainPivot.SelectedIndex -= 1;
        }

        private void FinishButton_OnClickButton_OnClick(object sender, RoutedEventArgs e)
        {
            var localsettings = ApplicationData.Current.LocalSettings;
            if (!localsettings.Values.ContainsKey("FirstTimeRunComplete")) localsettings.Values.Add("FirstTimeRunComplete",true);
            Frame.Navigate(typeof (MainPage));
        }
    }
}
