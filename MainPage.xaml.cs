using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Coding4Fun.Phone.Controls;

namespace PBTrainer
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            TiltEffect.TiltableItems.Add(typeof(StackPanel));
            TiltEffect.TiltableItems.Add(typeof(TextBlock));
        }

        #region Navigation Presses
        private void NewDrill_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //goto new drill
            NavigationService.Navigate(new Uri("/Pages/NewDrill.xaml", UriKind.Relative));
        }

        private void DrillHistory_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //goto drill history
            NavigationService.Navigate(new Uri("/Pages/DrillHistory.xaml", UriKind.Relative));
        }

        private void Settings_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //goto settings
            NavigationService.Navigate(new Uri("/Pages/Settings.xaml", UriKind.Relative));
        }
        
        private void About_Click(object sender, EventArgs e)
        {
            //goto about page
            NavigationService.Navigate(new Uri("/Pages/About.xaml", UriKind.Relative));
        }
        #endregion

       
    }
}