using System;
using System.Diagnostics;
using System.Threading;

namespace Noah
{
    public class ApplicationState
    {
        public States State { get; set; }
        public int Delay { get; set; }
        public int TimeAllocated { get; set; }
        public bool ShowTime { get; set; }
        public Stopwatch StopWatch { get; private set; }

        public ApplicationState()
        {
            State = States.Ready;
            Delay = 0;
            TimeAllocated = Timeout.Infinite;
            ShowTime = false;
            StopWatch = new Stopwatch();
            StopWatch.Start();
        }

        private void checkState()
        {
            while (true)
            {
                if (State == States.Done)
                    OnAttackCompleted(new AttackCompletedEventArgs { StopWatch = StopWatch });
                Thread.Sleep(100);
            }
        }

        public event EventHandler<AttackCompletedEventArgs> AttackCompleted;
        protected virtual void OnAttackCompleted(AttackCompletedEventArgs e)
        {
            EventHandler<AttackCompletedEventArgs> handler = AttackCompleted;
            if (handler != null)
                handler(this, e);
        }
    }

    public enum States
    {
        Ready,
        Attacking,
        Done
    }

    public class AttackCompletedEventArgs : EventArgs
    {
        public Stopwatch StopWatch { get; set; }
        public int Time { get { return StopWatch.Elapsed.Seconds; } }
    }
}