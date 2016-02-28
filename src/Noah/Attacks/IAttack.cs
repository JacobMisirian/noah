using System;
using System.Threading;

namespace Noah.Attacks
{
    public interface IAttack
    {
        string AttackName { get; }
        Thread AttackThread { get; }

        void BeginAttack();
    }
}

