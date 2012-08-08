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

using System.Xml;
using System.Xml.Linq;

using PBTrainer.Utilities;
using System.ComponentModel;

namespace PBTrainer.Pages
{
    public partial class DrillHistory : PhoneApplicationPage
    {
        public DrillHistory()
        {
            InitializeComponent();

            //do the threading magic to keep UI responsive
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_LoadHistory);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            ProgressBar.IsIndeterminate = true;
            worker.RunWorkerAsync();
        }

        void worker_LoadHistory(object sender, DoWorkEventArgs e)
        {
            e.Result = Utilities.DrillHistory.ReadDrillHistory();
        }

        public delegate void noItemDelegate();

        private void DisplayNoItemInfo()
        {
            noItemBlock1.Visibility = System.Windows.Visibility.Visible;
            noItemBlock2.Visibility = System.Windows.Visibility.Visible;
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            XDocument doc = (XDocument)e.Result;
            if (doc != null)
            {
                //sort this for time
                foreach (XElement item in doc.Root.Descendants("Drill"))
                {
                    StackPanel sp;

                    //determine age
                    DateTime dt = DateTime.Parse(item.Descendants("Timestamp").First().Value);
                    TimeSpan ts = DateTime.Now.Subtract(dt);
                    if (ts.TotalDays < 1)
                    {
                        sp = TodayStack;
                    }
                    else
                    {
                        if (ts.TotalDays > 1 && ts.TotalDays <= 7)
                        {
                            sp = WeekStack;
                        }
                        else
                        {
                            sp = OlderStack;
                        }
                    }

                    TextBlock desc = new TextBlock() { Text = "Description", Style = (Style)Application.Current.Resources["PhoneTextExtraLargeStyle"] };
                    TextBlock descVal = new TextBlock() { Text = item.Descendants("Description").First().Value, Style = (Style)Application.Current.Resources["PhoneTextNormalStyle"] };
                    sp.Children.Add(desc);
                    sp.Children.Add(descVal);

                    TextBlock feedback = new TextBlock() { Text = "Feedback", Style = (Style)Application.Current.Resources["PhoneTextExtraLargeStyle"] };
                    TextBlock feedbackVal = new TextBlock() { Text = item.Descendants("Feedback").First().Value, Style = (Style)Application.Current.Resources["PhoneTextNormalStyle"] };
                    sp.Children.Add(feedback);
                    sp.Children.Add(feedbackVal);

                    TextBlock reps = new TextBlock() { Text = "Repetitions Completed", Style = (Style)Application.Current.Resources["PhoneTextExtraLargeStyle"] };

                    TextBlock repVal = new TextBlock() { Text = item.Descendants("RepsCompleted").First().Value + " of " + item.Descendants("Reps").First().Value, Style = (Style)Application.Current.Resources["PhoneTextNormalStyle"] };
                    sp.Children.Add(reps);
                    sp.Children.Add(repVal);

                    Separator sep = new Separator();
                    sep.Background = new SolidColorBrush(Colors.DarkGray);
                    sp.Children.Add(sep);
                }
            }
            else
            {
                Dispatcher.BeginInvoke(new noItemDelegate(this.DisplayNoItemInfo), null);
            }
            ProgressBar.IsIndeterminate = false;
        }
    }
}