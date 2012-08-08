using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;

namespace PBTrainer.Model
{
    public delegate void ReadyTickEventHandler(object sender, TickEventArgs e);
    public delegate void DrillTickEventHandler(object sender, TickEventArgs e);

    public delegate void WarnEventHandler(object sender, EventArgs e);
    public delegate void HornEventHandler(object sender, EventArgs e);
    public delegate void DrillEndEventHandler(object sender, EventArgs e);

    public enum State { Paused, Resetting, Warning, Running, Complete };

    public class Rep
    {
        public TimeSpan ReadyTime  {get; set; }
        public TimeSpan WarningTime { get; set; }
        public TimeSpan Duration { get; set; }
        DispatcherTimer repTimer;
        public State RepState { get; private set; }

        public event WarnEventHandler WarnEvent;
        public event HornEventHandler HornEvent;
        public event DrillEndEventHandler DrillEndEvent;
        public event ReadyTickEventHandler ReadyTickEvent;
        public event DrillTickEventHandler DrillTickEvent;

        public void Start()
        {
            DrillTickEvent(this, new TickEventArgs(ReadyTime));
            repTimer = new DispatcherTimer();
            repTimer.Tick += repTimer_Tick;
            repTimer.Interval = new TimeSpan(0, 0, 1);
            repTimer.Start();
            RepState = State.Resetting;
        }

        public void pause()
        {
            if (repTimer.IsEnabled)
            {
                RepState = State.Paused;
                repTimer.Stop();
            }
            else
            {
                if (ReadyTime == new TimeSpan(0, 0, 0, 0))
                {
                    RepState = State.Running;
                }
                else
                {
                    if(ReadyTime.CompareTo(WarningTime) <=0)
                    {
                        RepState = State.Warning;
                    }
                    else
                    {
                        RepState = State.Resetting;
                    }
                }
                repTimer.Start();
            }            
        }

        public void repTimer_Tick(object sender, EventArgs e)
        {
            if (RepState == State.Running)
            {
                Duration = Duration.Subtract(new TimeSpan(0, 0, 0, 1));
                DrillTickEvent(this, new TickEventArgs(Duration));
                if (Duration <= new TimeSpan(0, 0, 0, 0))
                {
                    DrillEndEvent(this, EventArgs.Empty);
                    RepState = State.Complete;
                    repTimer.Stop();
                }
            }
            else
            {
                ReadyTime = ReadyTime.Subtract(new TimeSpan(0, 0, 0, 1));
                ReadyTickEvent(this, new TickEventArgs(ReadyTime));
                if (ReadyTime == WarningTime)
                {
                    RepState = State.Warning;
                    WarnEvent(this, EventArgs.Empty);
                }
                if (ReadyTime <= new TimeSpan(0, 0, 0, 0))
                {
                    RepState = State.Running;
                    HornEvent(this, EventArgs.Empty);
                    ReadyTickEvent(this, new TickEventArgs(Duration));
                }
            }
        }
    }

    public class TickEventArgs : EventArgs
    {
        public TickEventArgs(TimeSpan remaining)
        {
            this.duration = remaining;
        }
        public TimeSpan duration;
    }
}
