using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Noah
{
    /// <summary>
    /// Class that contains the state of the application and command line options.
    /// </summary>
    public class ApplicationState
    {
        /// <summary>
        /// The state the attacker is in.
        /// </summary>
        public States State { get; set; }
        /// <summary>
        /// The IP to attack.
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// The port to attack on.
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// The delay between ticks.
        /// </summary>
        public int Delay { get; set; }
        /// <summary>
        /// The amount of packets sent so far.
        /// </summary>
        public long FloodCount { get; set; }
        /// <summary>
        /// The execution time limit.
        /// </summary>
        public int TimeAllocated { get; set; }
        /// <summary>
        /// The message to send in your packet.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// The amount of threads to attack on.
        /// </summary>
        public int Threads { get; set; }
        /// <summary>
        /// Shows the execution time and every second.
        /// </summary>
        public bool ShowTime { get; set; }
        /// <summary>
        /// Shows the FloodCount at end of execution.
        /// </summary>
        public bool ShowFlood { get; set; }
        /// <summary>
        /// The running stopwatch of execution time.
        /// </summary>
        public Stopwatch StopWatch { get; private set; }
        /// <summary>
        /// Initializes a new Application State.
        /// </summary>
        public ApplicationState()
        {
            State = States.Ready;
            Port = 80;
            Delay = 0;
            FloodCount = 0;
            TimeAllocated = Timeout.Infinite;
            Threads = 1;
            ShowTime = false;
            ShowFlood = false;
            StopWatch = new Stopwatch();
            StopWatch.Start();

            new Thread(() => checkState()).Start();
        }

        private void checkState()
        {
            while (true)
            {
                if (State == States.Done)
                    OnAttackCompleted(new AttackCompletedEventArgs { StopWatch = StopWatch });
                if (StopWatch.ElapsedMilliseconds % 1000 == 0 && ShowTime && State == States.Attacking)
                {
                    Console.WriteLine(StopWatch.Elapsed.Seconds);
                    Thread.Sleep(10);
                }
            }
        }

     /*   private string randomString(int length)
        {
            Random random = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
                sb.Append((char)random.Next(48, 122));
            return sb.ToString();
        } */
        /// <summary>
        /// Thrown when an attack is completed.
        /// </summary>
        public event EventHandler<AttackCompletedEventArgs> AttackCompleted;
        protected virtual void OnAttackCompleted(AttackCompletedEventArgs e)
        {
            EventHandler<AttackCompletedEventArgs> handler = AttackCompleted;
            if (handler != null)
                handler(this, e);
        }
    }
    /// <summary>
    /// Class for the attack completed event args.
    /// </summary>
    public class AttackCompletedEventArgs : EventArgs
    {
        public Stopwatch StopWatch { get; set; }
        public int Time { get { return StopWatch.Elapsed.Seconds; } }
    }
    /// <summary>
    /// The states.
    /// </summary>
    public enum States
    {
        Ready,
        Attacking,
        Done
    }
}