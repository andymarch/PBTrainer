using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using PBTrainer.Pages;

namespace PBTrainer.Model
{
    public class Drill
    {
        public string description { get; set; }
        public string feedback { get; set; }
        public List<Rep> Reps { get; set; }
        public TimeSpan ResetTime { get; set; }

        //template values
        public int Template_RepNumber { get; set; }
        public TimeSpan Template_DrillDuration { get; set; }
        public TimeSpan Template_ReadyTime { get; set; }
        public TimeSpan Template_WarningTime { get; set; }

        public DrillRunning UI { get; set; }
        public int repPosition { get; private set; }

        public Model.State State { get { return (Model.State)Reps[repPosition].RepState; } }

        
        public Drill()
        {
            repPosition = 0;
        }

        public Drill(DrillRunning ui)
        {
            repPosition = 0;
            UI = ui;
            if (Template_RepNumber != -1)
            {
                UI.totalrepsval = Template_RepNumber.ToString();
            }
            else
            {
                ui.totalrepsval = "Infinite";
            }
        }

        public void go()
        {
            CreateRep();
            Reps[repPosition].WarnEvent += new WarnEventHandler(Drill_WarnEvent);
            Reps[repPosition].HornEvent += new HornEventHandler(Drill_HornEvent);
            Reps[repPosition].DrillEndEvent += new DrillEndEventHandler(Drill_DrillEndEvent);
            Reps[repPosition].ReadyTickEvent += new ReadyTickEventHandler(Drill_ReadyTickEvent);
            Reps[repPosition].DrillTickEvent += new DrillTickEventHandler(Drill_DrillTickEvent);
            Reps[repPosition].Start();
            UpdateUIRunState();
        }

        private string FormatTimeForDisplay(TimeSpan ts)
        {
            int min = ts.Minutes;
            string min_s = min.ToString();
            if (min < 10)
            {
                min_s = "0" + min_s;
            }
            int sec = ts.Seconds;
            string sec_s = sec.ToString();
            if (sec < 10)
            {
                sec_s = "0" + sec_s;
            }
            return string.Format("{0}:{1}", min_s, sec_s);
        }

        void Drill_DrillTickEvent(object sender, TickEventArgs e)
        {
            UI.timeval = FormatTimeForDisplay(e.duration);
        }

        void Drill_ReadyTickEvent(object sender, TickEventArgs e)
        {
            UI.timeval = FormatTimeForDisplay(e.duration);
        }

        void Drill_DrillEndEvent(object sender, EventArgs e)
        {
            UI.playBuzzer();
            repPosition++;
            UI.currentrepsval = repPosition.ToString();
            if (Template_RepNumber == -1 || repPosition < Template_RepNumber)
            {
                go();
            }
            else
            {
                UI.currentStateval = "...and we're done";
                UI.Completed();
            }
        }

        void Drill_HornEvent(object sender, EventArgs e)
        {
            UpdateUIRunState();
            UI.playBuzzer();
        }

        void Drill_WarnEvent(object sender, EventArgs e)
        {
            UpdateUIRunState();
            UI.playWarning();
        }

        public void pause()
        {
            if (State != State.Complete)
            {
                Reps[repPosition].pause();
                UpdateUIRunState();
            }
        }

        public void stop()
        {
            if (State != State.Complete)
            {
                Reps[repPosition].pause();
            }
        }

        private void UpdateUIRunState()
        {
            switch (Reps[repPosition].RepState)
            {
                case State.Paused:
                    UI.currentStateval = "Paused";
                    break;
                case State.Resetting:
                    UI.currentStateval = "Resetting";
                    break;
                case State.Warning:
                    UI.currentStateval = "Warning";
                    break;
                case State.Running:
                    UI.currentStateval = "Running";
                    break;
                default:
                    UI.currentStateval = "";
                    break;
            }
        }

        private void CreateRep()
        {
            Rep r = new Rep();
            r.WarningTime = Template_WarningTime;
            r.ReadyTime = Template_ReadyTime.Add(Template_WarningTime);
            r.Duration = Template_DrillDuration;
            this.Reps.Add(r);
        }
    }
}
