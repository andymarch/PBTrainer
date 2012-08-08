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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using PBTrainer.Model;

namespace PBTrainer.Pages
{
    public partial class DrillRunning : PhoneApplicationPage
    {
        readonly Uri buzzer = new Uri("../Resources/buzzer.mp3", UriKind.Relative);
        readonly Uri warn = new Uri("../Resources/double.mp3", UriKind.Relative);

        #region Dependency Props
        public static readonly DependencyProperty timeProperty = DependencyProperty.Register("timeval", typeof(string), typeof(DrillRunning),null);
        public string timeval
        {
            get { return (string)GetValue(timeProperty); }
            set { SetValue(timeProperty, value); }
        }

        public static readonly DependencyProperty totalrepsProperty = DependencyProperty.Register("totalrepsval", typeof(string), typeof(DrillRunning), null);
        public string totalrepsval
        {
            get { return (string)GetValue(totalrepsProperty); }
            set { SetValue(totalrepsProperty, value); }
        }

        public static readonly DependencyProperty currentrepsProperty = DependencyProperty.Register("currentrepsval", typeof(string), typeof(DrillRunning), null);
        public string currentrepsval
        {
            get { return (string)GetValue(currentrepsProperty); }
            set { SetValue(currentrepsProperty, value); }
        }

        public static readonly DependencyProperty currentStateProperty = DependencyProperty.Register("currentStateval", typeof(string), typeof(DrillRunning), null);
        public string currentStateval
        {
            get { return (string)GetValue(currentStateProperty); }
            set {
                    SetValue(currentStateProperty, value);
                    if (currentStateval == "Paused")
                    {
                        PauseBtn.Content = "Resume";
                    }
                    else
                    {
                        PauseBtn.Content = "Pause";
                    }
                }
        }
        #endregion

        Drill drill;

        public DrillRunning()
        {
            InitializeComponent();
            drill = (App.Current as App).CurrentDrill;
            if (drill != null)
            {
                drill.UI = this;
                currentrepsval = drill.repPosition.ToString();
                totalrepsval = drill.Reps.Count.ToString();
                drill.go(); 
                
            }
            else
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }

        #region sound effects
        public void playBuzzer()
        {
            var stream = TitleContainer.OpenStream("Resources/buzzer.wav");
            var effect = SoundEffect.FromStream(stream);
            FrameworkDispatcher.Update();
            effect.Play();
        }

        public void playWarning()
        {
            var stream = TitleContainer.OpenStream("Resources/double.wav");
            var effect = SoundEffect.FromStream(stream);
            FrameworkDispatcher.Update();
            effect.Play();
        }
        #endregion

        public void Completed()
        {
            NavigationService.Navigate(new Uri("/Pages/DrillFeedback.xaml", UriKind.Relative));
        }

        #region User events
        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            drill.pause();
            NavigationService.Navigate(new Uri("/Pages/DrillFeedback.xaml", UriKind.Relative));
        }
        #endregion

        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            drill.pause();
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            drill.stop();
            (App.Current as App).CurrentDrill = null;
        }
    }
}