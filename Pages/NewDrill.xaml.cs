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
using PBTrainer;
using PBTrainer.Model;
using PBTrainer.Utilities;

namespace PBTrainer.Pages
{
    public partial class NewDrill : PhoneApplicationPage
    {
        private readonly string[] banter = new string[] { "Just keep grinding.", "Go there, shoot, win.", "Run, gun, win." };
        private int initialfeedbackBanter;

        public NewDrill()
        {
            InitializeComponent();
            initialfeedbackBanter = new Random().Next(0, banter.Length);
            descTxtBox.Text = banter[initialfeedbackBanter];

            Utilities.Settings set = new Utilities.Settings();

            //init drill duration
            DrillDuration = set.GetValueOrDefault(Utilities.Settings.DrillDurationSettingKeyName, set.DrillDurationSettingDefault);
            DrillMaxDuration = TimeSpan.FromMinutes(5);
            DrillStepDuration = TimeSpan.FromSeconds(5);
            //init warning duration
            WarningDuration = set.GetValueOrDefault(Utilities.Settings.WarningSettingKeyName, set.WarningSettingDefault);
            WarningMaxDuration = TimeSpan.FromSeconds(30);
            WarningStepDuration = TimeSpan.FromSeconds(5);
            //init reset duration
            ResetDuration = set.GetValueOrDefault(Utilities.Settings.ResetSettingKeyName, set.ResetSettingDefault);
            ResetMaxDuration = TimeSpan.FromSeconds(55);
            ResetStepDuration = TimeSpan.FromSeconds(5);
        }

        #region Drill duration
        public TimeSpan DrillDuration { get; set; }
        public TimeSpan DrillMaxDuration { get; set; }
        public TimeSpan DrillStepDuration { get; set; }
        #endregion

        #region Warning duration
        public TimeSpan WarningDuration { get; set; }
        public TimeSpan WarningMaxDuration { get; set; }
        public TimeSpan WarningStepDuration { get; set; }
        #endregion

        #region Reset duration
        public TimeSpan ResetDuration { get; set; }
        public TimeSpan ResetMaxDuration { get; set; }
        public TimeSpan ResetStepDuration { get; set; }
        #endregion

        private void GoBtn_Click(object sender, RoutedEventArgs e)
        {
            Drill d = new Drill();
            d.description = descTxtBox.Text;
            d.ResetTime = ResetDuration;
            List<Rep> replist = new List<Rep>();
            ListPickerItem selectedItem = this.Repitions.ItemContainerGenerator.ContainerFromItem(this.Repitions.SelectedItem) as ListPickerItem;
            if (selectedItem.Content.ToString() != "Infinite")
            {
                d.Template_RepNumber = int.Parse(selectedItem.Content.ToString());
            }
            else
            {
                d.Template_RepNumber = -1;
            }
            d.Reps = replist;
            d.Template_DrillDuration = DrillDuration;
            d.Template_ReadyTime = ResetDuration.Add(WarningDuration);
            d.Template_WarningTime = WarningDuration;
            (App.Current as App).CurrentDrill = d;
            //goto drill running 
            NavigationService.Navigate(new Uri("/Pages/DrillRunning.xaml", UriKind.Relative));
        }

        private void descTxtBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (descTxtBox.Text == banter[initialfeedbackBanter])
            {
                descTxtBox.Text = "";
            }
        }
    }
}