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
            enterCli(args);
        }

        private static void enterCli(string[] args)
        {
            state = new ArgumentParser(args).Parse();
            state.AttackCompleted += state_OnAttackCompletedCli;
            IAttack attack = null;

            switch (state.Attack.ToLower())
            {
                case "tcp":
                    attack = new HTTPAttack(state, state.IP, state.Port);
                    break;
                case "udp":
                    attack = new UDPAttack(state, state.IP, state.Port);
                    break;
                case "icmp":
                    attack = new ICMPAttack(state, state.IP);
                    break;
                default:
                    Console.WriteLine("Unknown protocol name " + state.Attack);
                    Environment.Exit(0);
                    break;
            }

            for (int i = 0; i < state.Threads; i++)
                attack.BeginAttack();
            Thread.Sleep(state.TimeAllocated);
            state.State = States.Done;
        }

        private static void state_OnAttackCompletedCli(object sender, AttackCompletedEventArgs e)
        {
            if (state.ShowTime)
                Console.WriteLine("Total elapsed time: " + state.StopWatch.Elapsed.Seconds);
            if (state.ShowFlood)
                Console.WriteLine("Total flood count: " + state.FloodCount.ToString());
            Environment.Exit(0);
        }
    }
}