using System;

namespace Noah
{
    public class ArgumentParser
    {
        private int position;
        private string[] args;
        public ArgumentParser(string[] args)
        {
            this.args = args;
        }

        public ApplicationState Parse()
        {
            if (args.Length <= 0)
                displayHelp();
            
            ApplicationState state = new ApplicationState();

            for (position = 0; position < args.Length; position++)
            {
                switch (args[position].ToLower())
                {
                    case "-d":
                    case "--delay":
                        state.Delay = Convert.ToInt32(expectData("Delay"));
                        break;
                    case "-h":
                    case "--help":
                        displayHelp();
                        break;
                    case "-l":
                    case "--time-limit":
                        state.TimeAllocated = Convert.ToInt32(expectData("Time limit in seconds")) * 1000;
                        break;
                    case "-p":
                    case "--port":
                        state.Port = Convert.ToInt32(expectData("Port"));
                        break;
                    case "-s":
                    case "--show-time":
                        state.ShowTime = true;
                        break;
                    case "-t":
                    case "--threads":
                        state.Threads = Convert.ToInt32(expectData("Number of Threads"));
                        break;
                }
            }
            return state;
        }

        private void displayHelp()
        {
            Console.WriteLine("Noah.exe [Attack_Type] [Host] [Options]");
            Console.WriteLine("Attacks:");
            Console.WriteLine("Tcp\tUdp");
            Console.WriteLine("Options:");
            Console.WriteLine("-d --delay [Milliseconds]\tDelay between attack ticks.");
            Console.WriteLine("-h --help\tDisplays this help and exits.");
            Console.WriteLine("-l --time-limit [LIMIT]\tLimit time of attack in seconds.");
            Console.WriteLine("-p --port [Port]\tChanges the port from the default 80.");
            Console.WriteLine("-s --show-time\tDisplays the total time and tick per second.");
            Console.WriteLine("-t --threads [ThreadLimit]\tSets the amount of threads used on the attack. Default 1.");
            Environment.Exit(0);
        }

        private string expectData(string expectedType)
        {
            if (!args[++position].ToLower().StartsWith("-"))
                return args[position];
            Console.WriteLine("Expected " + expectedType + " after " + args[position - 1]);
            Environment.Exit(0);
            return "";
        }
    }
}

