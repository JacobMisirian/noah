using System;
using System.Threading;

using Noah.Attacks;

namespace Noah
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			if (args.Length <= 0)
				displayHelp();
			
			ApplicationState state = new ApplicationState();
			IAttack attack = null;

			if (args.Length >= 3)
				switch (args[2].ToLower())
				{
					case "-d":
					case "--delay":
						state.Delay = Convert.ToInt32(args[3]);
						break;
					case "-h":
					case "--help":
						displayHelp();
						break;
					case "-s":
					case "--show-time":
						state.ShowTime = true;
						break;
					case "-t":
					case "--time-limit":
						state.TimeAllocated = Convert.ToInt32(args[3]) * 1000;
						break;
				}

			switch (args[0].ToLower())
			{
				case "tcp":
					attack = new HTTPAttackPlugin(args[1], state);
					break;
			}

			attack.BeginAttack();
			Thread.Sleep(state.TimeAllocated);
			state.State = States.Done;

			if (state.ShowTime)
				Console.WriteLine("Total elapsed time: " + state.StopWatch.Elapsed.Seconds);
		}

		private static void displayHelp()
		{
			Console.WriteLine("Noah.exe [Attack_Type] [Host] [Options]");
			Console.WriteLine("Attacks:");
			Console.WriteLine("Tcp\tUdp");
			Console.WriteLine("Options:");
			Console.WriteLine("-d --delay [Milliseconds]\tDelay between attack ticks.");
			Console.WriteLine("-h --help\tDisplays this help and exits.");
			Console.WriteLine("-s --show-time\tDisplays the total time and tick per second.");
			Console.WriteLine("-t --time [LIMIT]\tLimit time of attack in seconds.");

			Environment.Exit(0);
		}
	}
}