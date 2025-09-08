//	<copyright file="Program.cs"  company="Alliant Technologies">
//		Copyright © 2025 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for Program.
//	</summary>
namespace RuntimeTranscriber
{
    /// <summary>
    /// Represents the main entry point and core functionality of the application.
    /// </summary>
    /// <remarks>The <see cref="Program"/> class contains the application's entry point and the main program
    /// loop. It processes user commands, interacts with the <see cref="Transcriber"/> class to perform operations, and
    /// provides feedback to the user. The application supports commands for managing system data, retrieving
    /// information, and terminating the program.</remarks>
    internal class Program
    {
        #region Constants
        /// <summary>
        /// The minimum length of command line args that contain command arguments used by this application.
        /// </summary>
        private const int MinimumLengthWithArguments = 2;

        /// <summary>
        /// Represents the index of the command in a collection or array.
        /// </summary>
        /// <remarks>This constant is used to identify the position of the command element.</remarks>
        private const int CommandIndex = 0;


        // Commands
        /// <summary>
        /// Represents the command string used to display help information.
        /// </summary>
        private const string HelpCommand =                          "help";

        /// <summary>
        /// Represents the command string used to add a new system.
        /// </summary>
        private const string AddNewSystemCommand =                  "add-newsystem";

        /// <summary>
        /// Represents the command used to find the IP address of PLCs.
        /// </summary>
        private const string FindIpCommand =                        "find-ip";

        /// <summary>
        /// Represents the command string used to find all systems.
        /// </summary>
        private const string FindAllSystemsCommand =                "find-allsystems";

        /// <summary>
        /// Represents the command string used to find the parent members of a specified type member.
        /// </summary>
        private const string FindTypeMemberParentsCommand =         "find-memberparents";

        /// <summary>
        /// Represents the command string used to find the children of a specified type member.
        /// </summary>
        private const string FindTypeMemberChildrenCommand =        "find-memberchildren";

        /// <summary>
        /// Represents the command string used to add member deviations.
        /// </summary>
        private const string AddMemberDeviationsCommand =           "add-memberdeviations";

        /// <summary>
        /// Represents the command string used to terminate the application.
        /// </summary>
        private const string QuitCommand =                          "quit";


        // Common messages
        /// <summary>
        /// The default greeting message displayed to the user when the application starts.
        /// </summary>
        /// <remarks>This message provides instructions for the user to begin interacting with the
        /// application, including a reference to the help command for additional guidance.</remarks>
        private const string GreetingMessage =                      $"Search the runtime file! Enter a command to get started. Enter \"{HelpCommand}\" to see the command list.";

        /// <summary>
        /// Represents the error message displayed when an invalid command is entered.
        /// </summary>
        /// <remarks>The message includes a suggestion to use the help command to view the list of
        /// available commands.</remarks>
        private const string InvalidCommandMessage =                $"Invalid command. Enter \"{HelpCommand}\" to see the command list.";

        /// <summary>
        /// Provides a formatted help message listing all available commands and their descriptions.
        /// </summary>
        /// <remarks>This constant contains a multi-line string that describes the purpose and usage of
        /// each command available in the application. It includes details about commands for searching system
        /// information, adding new systems, and quitting the application. The message is intended to be displayed to
        /// users as part of the application's help functionality.</remarks>
        private const string HelpCommandMessage =                   $"Command list:\n\t{FindIpCommand}:\t\t Search for the port number and IP address of both Transport and Sort PLC's for a system. Enter the system name you'd like to search." +
                                                                    $"\n\t{FindAllSystemsCommand}:\t Search for all systems present in the runtime." +
                                                                    $"\n\t{FindTypeMemberParentsCommand}:\t Search for all the parents of a given type member." +
                                                                    $"\n\t{FindTypeMemberChildrenCommand}:\t Search for all the children of a given type member." +
                                                                    $"\n\t{AddNewSystemCommand}:\t\t Add a new system to the runtime. Pass in the new system name, transport primary IP address, and the transport secondary address." +
                                                                    $"You can also pass in two additional IP addresses for the sorter PLCs if they exist. Generates a modified runtime file named \"ModifiedRuntime.json\"" +
                                                                    $"\n\t{AddMemberDeviationsCommand}:\t Add member deviations to the runtime. Pass in the system name, the system type (transport or sort), and the number of NORMAL conveyors." +   
                                                                    $"\n\t{QuitCommand}:\t\t\t Close the application.";

        /// <summary>
        /// The message displayed when no arguments are provided for locating IP addresses.
        /// </summary>
        private const string FindIpNoArgsMessage =                  "No argument given. Please provide the system name you wish to locate the IP addresses for.";

        /// <summary>
        /// The error message displayed when no arguments are provided for searching the parent members of a type.
        /// </summary>
        private const string FindTypeMemberParentsNoArgsMessage =   "No argument given. Please provide the Type Member name to search the parents of.";

        /// <summary>
        /// The error message displayed when no argument is provided for searching the children of a type member.
        /// </summary>
        private const string FindTypeMemberChildrenNoArgsMessage =  "No argument given. Please provide the Type Member name to search the children of.";

        /// <summary>
        /// The error message displayed when insufficient arguments are provided for adding a new system.
        /// </summary>
        private const string NotEnoughArgsGenericMessage =          "Not enough arguments provided.";

        /// <summary>
        /// The error message displayed when too many arguments are provided while adding a new system.
        /// </summary>
        private const string TooManyArgsGenericMessage =            "Too many arguments.";

        /// <summary>
        /// Represents the success message displayed after successfully writing the modified runtime file to
        /// "ModifiedRuntime.json".
        /// </summary>
        private const string FileWriteSuccess =                     "Successfully wrote the modified runtime file to \"ModifiedRuntime.json\".";

        /// <summary>
        /// Message displayed to the user to verify the correctness of added member deviations.
        /// </summary>
        private const string AddMemberDeviationsWarning =           "Please validate that the member deviations added were correct. This is partially automating a manual process, so there may be errors. PLEASE VERIFY THE OUTPUT.";

        /// <summary>
        /// Represents the message displayed when the application is terminated.
        /// </summary>
        private const string ApplicationTerminationMessage =        "The application has been terminated.";

        // Miscellaneous
        /// <summary>
        /// Represents the symbol used to indicate that the application can now accept a command.
        /// </summary>
        private const string AcceptCommandSymbol =                  "@> ";

        /// <summary>
        /// Represents the runtime instance of the <see cref="Transcriber"/> used for transcription operations.
        /// </summary>
        /// <remarks>This field holds a reference to the current <see cref="Transcriber"/> instance used
        /// at runtime.  It may be null if no transcriber has been initialized.</remarks>
        private static Transcriber? runtimeTranscriber;
        #endregion

        /// <summary>
        /// Serves as the entry point for the application.
        /// </summary>
        /// <remarks>If no arguments are provided or the first argument is null, the application will
        /// terminate immediately. The method initializes a <see cref="Transcriber"/> instance using the provided
        /// runtime file path  and enters the main program loop. Any unhandled exceptions during execution are caught
        /// and logged to the console.</remarks>
        /// <param name="args">An array of command-line arguments. The first argument specifies the path to the runtime file.  Additional
        /// arguments may include a command and its associated parameters.</param>
        static void Main(string[] args)
        {
            // 1st argument is the path to the runtime file, 2nd argument is the command, 3rd argument is... the argument.
            string? path = args.Length > 0 ? args[0] : null;

            // If nothing is entered, exit the application.
            if (path is null)
            {
                return;
            }

            // Initialize the transcriber.
            runtimeTranscriber = new(path);

            try
            {
                ProgramLoop();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Executes the main program loop, handling user input and executing commands.
        /// </summary>
        /// <remarks>This method continuously prompts the user for input, processes commands, and executes
        /// the corresponding actions. Supported commands include displaying help, finding IP addresses, retrieving
        /// system information, and adding new systems. The loop runs until the user issues the quit command.</remarks>
        /// <param name="runtimeTranscriber">An instance of the <see cref="Transcriber"/> class used to process commands and perform operations.</param>
        static void ProgramLoop()
        {
            if (runtimeTranscriber is null)
            {
                throw new InvalidOperationException("The runtime transcriber has not been initialized.");
            }

            // Handle inputs.
            string command;
            string? commandArgs;
            const char commandDelimiter = ' ';

            Console.WriteLine(GreetingMessage);

            while (true)
            {
                Console.Write(AcceptCommandSymbol);
                string? input = Console.ReadLine()?.Trim();

                string[]? arguments = input?.Split(commandDelimiter);

                if (arguments is not null && arguments.Length > 0)
                {
                    command = arguments[CommandIndex];
                    commandArgs = arguments.Length >= MinimumLengthWithArguments ? arguments[CommandIndex + 1] : null;

                    // Run command
                    switch (command)
                    {
                        case HelpCommand:
                            Console.WriteLine(HelpCommandMessage);
                            break;
                        case $"{FindIpCommand}":
                        {
                            // Argument should be the system name.
                            if (commandArgs is null)
                            {
                                Console.WriteLine(FindIpNoArgsMessage);
                            }
                            else
                            {
                                Console.Write(runtimeTranscriber.FindIP(commandArgs));
                            }
                            break;
                        }

                        case $"{FindAllSystemsCommand}":
                             Console.WriteLine(runtimeTranscriber.FindAllSystems());
                        break;

                        case $"{FindTypeMemberParentsCommand}":
                        {
                            if (commandArgs is null)
                            {
                                    Console.WriteLine(FindTypeMemberParentsNoArgsMessage);
                            }
                            else
                            {
                                Console.WriteLine($"\n{runtimeTranscriber.GetDirectParentsOfTypemember(commandArgs)}");
                            }
                            break;
                        }

                        case $"{FindTypeMemberChildrenCommand}":
                        {
                            if (commandArgs is null)
                            {
                                Console.WriteLine(FindTypeMemberChildrenNoArgsMessage);
                            }
                            else
                            {
                                Console.WriteLine($"\n{runtimeTranscriber.GetDirectChildrenOfTypemember(commandArgs)}");
                            }
                            break;
                        }

                        case $"{AddNewSystemCommand}":
                        {
                            // Check arguments.
                            if (arguments.Length < 4)
                            {
                                Console.WriteLine(NotEnoughArgsGenericMessage);
                                break;
                            }
                            else if (arguments.Length > 6)
                            {
                                Console.WriteLine(TooManyArgsGenericMessage);
                                break;
                            }

                            if (arguments.Length == 6)
                            {
                                runtimeTranscriber.AddNewBaseSystemData(arguments[1], arguments[2], arguments[3], arguments[4], arguments[5]);
                            }
                            else
                            {
                                runtimeTranscriber.AddNewBaseSystemData(arguments[1], arguments[2], arguments[3]);
                            }


                            Console.WriteLine(FileWriteSuccess);
                            break;
                        }

                        case $"{AddMemberDeviationsCommand}":
                        {
                            // Check arguments.
                            if (arguments.Length < 4)
                            {
                                Console.WriteLine(NotEnoughArgsGenericMessage);
                                break;
                            }
                            else if (arguments.Length > 4)
                            {
                                Console.WriteLine(TooManyArgsGenericMessage);
                                break;
                            }

                            runtimeTranscriber.AddNewMemberDeviations(arguments[1], arguments[2], Int32.Parse(arguments[3]));

                            Console.WriteLine(FileWriteSuccess);
                            Console.WriteLine(AddMemberDeviationsWarning);
                                break;
                        }

                        case $"{QuitCommand}":
                            Console.WriteLine(ApplicationTerminationMessage);
                            return;

                        default:
                            Console.WriteLine(InvalidCommandMessage);
                            break;
                    }
                }
                else
                {
                    Console.WriteLine(InvalidCommandMessage);
                }
            }
        }
    }
}
