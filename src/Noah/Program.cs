using System;
using System.Threading;

using Noah.Attacks;

namespace Noah
{
	class MainClass
	{
		public static void Main(string[] args)
        {	
            ApplicationState state = new ArgumentParser(args).Parse();
			IAttack attack = null;

			switch (args[0].ToLower())
			{
				case "tcp":
					attack = new HTTPAttackPlugin(args[1], state);
					break;
			}

            for (int i = 0; i < state.Threads; i++)
                attack.BeginAttack();
			Thread.Sleep(state.TimeAllocated);
			state.State = States.Done;

			if (state.ShowTime)
				Console.WriteLine("Total elapsed time: " + state.StopWatch.Elapsed.Seconds);
		}


	}
}