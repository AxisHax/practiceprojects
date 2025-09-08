//	<copyright file="Program.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for Program.
//	</summary>
namespace CIP_EthernetIP_Library
{
    using System.Net;

    /// <summary>
    /// Entry point of the application.
    /// </summary>
    internal class Program
    {
        /// <summary>Entry point of the application.</summary>
        /// <param name="args">Command line arguments. Expects an IP address.</param>
        /// <returns>0 if the application ran successfully, 1 if an error occurred during execution.</returns>
        public static int Main(string[] args)
        {
            // For now let's only accept the IP address of the device that you wish to connect to as an argument.
            // Let's also always expect it as the first argument.
            if (args.Length == 0)
            {
                Console.WriteLine(Properties.Resources.MalformedCommandLineArgs);
                return 1;
            }

            EthernetIPConnection connection = new ();
            string host = args[0];

            try
            {
                connection.Connect(host);
                connection.Disconnect();
            }
            catch (Exception e)
            {
                Console.WriteLine(Properties.Resources.GeneralException, e, e.Message);
                return 1;
            }

            return 0;
        }
    }
}
