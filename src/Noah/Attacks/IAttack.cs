using System;
using System.Threading;

namespace Noah.Attacks
{
    public interface IAttack
    {
        /// <summary>
        /// The attack name.
        /// </summary>
        string AttackName { get; }
        /// <summary>
        /// Starts the attack on a new thread.
        /// </summary>
        void BeginAttack();
    }
}

