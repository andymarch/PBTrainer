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

using PBTrainer.Model;
using PBTrainer.Utilities;
using System.ComponentModel;

namespace PBTrainer.Pages
{
    public partial class DrillFeedback : PhoneApplicationPage
    {
        private readonly string[] banter = new string[] { "How was it for you?", "Tired?", "That all you got?" };
        private int initialfeedbackBanter;
        Drill drill;

        public DrillFeedback()
        {
            InitializeComponent();

            if ((App.Current as App).CurrentDrill != null)
            {
                drill = (App.Current as App).CurrentDrill;
                initialfeedbackBanter = new Random().Next(0,banter.Length);
                FeedbackTxt.Text = banter[initialfeedbackBanter];
            }
            else
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            } 
        }

        #region button events
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (FeedbackTxt.Text != banter[initialfeedbackBanter])
            {
                drill.feedback = FeedbackTxt.Text;
            }

            //do the threading magic to keep UI responsive
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            ProgressBar.IsIndeterminate = true;
            worker.RunWorkerAsync();
        }
        
        private void DiscardBtn_Click(object sender, RoutedEventArgs e)
        {
            (App.Current as App).CurrentDrill = null;
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
        #endregion

        #region Background worker
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Utilities.DrillHistory.AddToDrillHistory((App.Current as App).CurrentDrill);
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProgressBar.IsIndeterminate = false;
            (App.Current as App).CurrentDrill = null;
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
        #endregion

        private void FeedbackTxt_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(FeedbackTxt.Text == banter[initialfeedbackBanter])
            {
                FeedbackTxt.Text = "";
            }
        }
    }
}