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
using PBTrainer.Utilities;

namespace PBTrainer.Pages
{
    public partial class Settings : PhoneApplicationPage
    {
        public TimeSpan DrillDuration { get; set; }
        public TimeSpan WarningDuration { get; set; }
        public TimeSpan ResetDuration { get; set; }

        private Utilities.Settings setting;


        public Settings()
        {
            InitializeComponent();
            setting = new Utilities.Settings();
            try
            {
                DrillDuration = setting.GetValueOrDefault(Utilities.Settings.DrillDurationSettingKeyName, setting.DrillDurationSettingDefault);
                WarningDuration = setting.GetValueOrDefault(Utilities.Settings.WarningSettingKeyName, setting.WarningSettingDefault);
                ResetDuration = setting.GetValueOrDefault(Utilities.Settings.ResetSettingKeyName, setting.ResetSettingDefault);
            }
            catch (Exception e)
            {
                MessageBox.Show("Whoops, sorry something went wrong while loading your settings.");
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            //TODO verify exit?
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void Save_Click(object sender, EventArgs e)
        {
            setting.AddOrUpdateValue(Utilities.Settings.DrillDurationSettingKeyName, DrillDuration);
            setting.AddOrUpdateValue(Utilities.Settings.WarningSettingKeyName, WarningDuration);
            setting.AddOrUpdateValue(Utilities.Settings.ResetSettingKeyName, ResetDuration);
            setting.save();
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}