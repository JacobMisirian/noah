using System;
using System.IO;
using System.Net;

namespace Noah
{
    /// <summary>
    /// Class for parsing the command line options.
    /// </summary>
    public class ArgumentParser
    {
        private int position;
        private string[] args;
        /// <summary>
        /// Initializes an argument parser.
        /// </summary>
        /// <param name="args">The array to parse.</param>
        public ArgumentParser(string[] args)
        {
            this.args = args;
        }
        /// <summary>
        /// Parses the arguments and returns an ApplicationState.
        /// </summary>
        /// <returns></returns>
        public ApplicationState Parse()
        {
            if (args.Length <= 0)
                displayHelp();
            
            ApplicationState state = new ApplicationState();

            for (position = 0; position < args.Length; position++)
            {
                try
                {
                    switch (args[position].ToLower())
                    {
                        case "-a":
                        case "--attack-type":
                            state.Attack = expectData("Attack Type");
                            break;
                        case "-d":
                        case "--delay":
                            state.Delay = Convert.ToInt32(expectData("Delay"));
                            break;
                        case "-f":
                        case "--show-flood":
                            state.ShowFlood = true;
                            break;
                        case "-h":
                        case "--help":
                            displayHelp();
                            break;
                        case "-l":
                        case "--time-limit":
                            state.TimeAllocated = Convert.ToInt32(expectData("Time limit in seconds")) * 1000;
                            break;
                        case "-m":
                        case "--message":
                            state.Message = File.ReadAllText(expectData("Path"));
                            break;
                        case "-n":
                        case "--host-name":
                            string data = expectData("Host name");
                            try
                            {
                                state.IP = IPAddress.Parse(data).ToString();
                            }
                            catch
                            {
                                try
                                {
                                    state.IP = Dns.GetHostAddresses(data)[0].ToString();
                                }
                                catch
                                {
                                    die("Entry is not an IP nor valid hostname.");
                                }
                            }
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
                catch (IndexOutOfRangeException)
                {
                    die("Index was out of range!");

                }
                catch (IOException)
                {
                    die("File not found!");
                }
                catch (FormatException)
                {
                    die("Int was not in correct format!");
                }
                catch (Exception)
                {
                    die("Something went wrong!");
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
            Console.WriteLine("-a --attack-type [Protocol]\tSets the attack mode to Tcp, Udp, or Icmp. Default Tcp.");
            Console.WriteLine("-d --delay [Milliseconds]\tDelay between attack ticks.");
            Console.WriteLine("-f --show-flood\tShows the flood count at the end of execution.");
            Console.WriteLine("-h --help\tDisplays this help and exits.");
            Console.WriteLine("-l --time-limit [Seconds]\tLimit time of attack in seconds.");
            Console.WriteLine("-m --message [Path]\tChanges the message sent from a default random string to the text in file [Path].");
            Console.WriteLine("-n --host-name [Host]\tSets the hostname to attack. Default 127.0.0.1.");
            Console.WriteLine("-p --port [Port]\tChanges the port from the default 80.");
            Console.WriteLine("-s --show-time\tDisplays the total time and tick per second.");
            Console.WriteLine("-t --threads [ThreadLimit]\tSets the amount of threads used on the attack. Default 1.");
            Environment.Exit(0);
        }

        private string expectData(string expectedType)
        {
            if (!args[++position].ToLower().StartsWith("-"))
                return args[position];
            die("Expected " + expectedType + " after " + args[position - 1]);
            return "";
        }

        private void die(string message)
        {
            Console.WriteLine(message);
            Environment.Exit(0);
        }
    }
}

