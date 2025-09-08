//	<copyright file="Program.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for Program.
//	</summary>
namespace EthernetIP_Library
{
    /// <summary>
    /// Entry point of the application.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Main method.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            EthernetIPConnection connection = new EthernetIPConnection();

            try
            {
                connection.Connect();
                connection.Disconnect();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}\n\t{e.Message}");
            }
        }
    }
}
