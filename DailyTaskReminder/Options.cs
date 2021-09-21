using System;
using System.IO;
using System.Collections.Generic;

namespace DailyTaskReminder
{
    /// <summary>
    /// Holds the options for program behavior.
    /// Provides method for parsing arguments from the command line.
    /// </summary>
    class Options
    {

        /// <summary>
        /// Path to file that holds the tasks.
        /// Default: Tasks.txt
        /// </summary>
        public string TasksPath { get; private set; } = "Tasks.txt";

        /// <summary>
        /// Path to json file that holds the reminders.
        /// Default: Reminders.json
        /// </summary>
        public string RemindersPath { get; private set; } = "Reminders.json";

        /// <summary>
        /// Specifies the port of the HttpListener server to which the user can send requests.
        /// Default: 25566
        /// </summary>
        public string Port { get; private set; } = "25566";

        /// <summary>
        /// Specifies the Access-Control-Allow-Origin HTTP header.
        /// The server will not respond to requests made by clients with different origin.
        /// Default: *
        /// </summary>
        public string Access { get; private set; } = "*";

        /// <summary>
        /// If true then reminders will not be send for the next hour, to allow user to mark
        /// tasks as finished when starting the program without getting spammed by reminders.
        /// Default: false
        /// </summary>
        public bool Hush { get; private set; } = false;

        /// <summary>
        /// Entry point for the program.
        /// </summary>
        /// <param name="args">Raw arguments from the command line</param>
        static void Main(string[] args)
        {
            Options options = ParseArguments(args);
            Program.Start(options);
        }

        /// <summary>
        /// Parses arguments from the command line into the Option instance and returns it.
        /// </summary>
        /// <exception cref="ArgumentException">Arguments are not valid.</exception>
        /// <param name="args">Arguments from command line</param>
        /// <returns>Parsed options</returns>
        private static Options ParseArguments(string[] args)
        {
            Options options = new Options();

            int index = 0;
            while (args.Length > index)
            {
                string arg = args[index];

                if (arg.StartsWith("--"))
                {
                    arg = arg[2..];
                }
                else if (arg.StartsWith("-"))
                {
                    arg = arg[1..];
                }

                switch (arg)
                {
                    case "tasks":
                    case "task":
                    case "t":
                        index++;
                        if (args.Length <= index)
                        {
                            throw new ArgumentException($"Argument ${arg} needs a parameter");
                        }
                        else
                        {
                            string param = args[index];
                            if (File.Exists(param))
                            {
                                options.TasksPath = param;
                            }
                            else
                            {
                                throw new ArgumentException($"File at ${param} does not exist");
                            }
                        }
                        break;

                    case "reminders":
                    case "reminder":
                    case "r":
                        index++;
                        if (args.Length <= index)
                        {
                            throw new ArgumentException($"Argument ${arg} needs a parameter");
                        }
                        else
                        {
                            string param = args[index];
                            if (File.Exists(param))
                            {
                                options.RemindersPath = param;
                            }
                            else
                            {
                                throw new ArgumentException($"File at ${param} does not exist");
                            }
                        }
                        break;

                    case "port":
                    case "p":
                        index++;
                        if (args.Length <= index)
                        {
                            throw new ArgumentException($"Argument ${arg} needs a parameter");
                        }
                        else
                        {
                            string param = args[index];
                            if (int.TryParse(param, System.Globalization.NumberStyles.None, null, out int port))
                            {
                                if (port >= 0 && port <= 65535)
                                {
                                    options.Port = param;
                                }
                                else
                                {
                                    throw new ArithmeticException($"Argument ${arg} should be between 0 and 65535");
                                }    
                            }
                            else
                            {
                                throw new ArgumentException($"Argument ${arg} should be a number");
                            }
                        }
                        break;

                    case "access":
                    case "a":
                        index++;
                        if (args.Length <= index)
                        {
                            throw new ArgumentException($"Argument ${arg} needs a parameter");
                        }
                        else
                        {
                            string param = args[index];
                            options.Access = param; 
                        }
                        break;

                    case "hush":
                    case "h":
                        options.Hush = true;
                        break;

                    default:
                        throw new ArgumentException($"Unknown argument: {args[index]}");
                };
            }
            return options;

        }
    }
}
