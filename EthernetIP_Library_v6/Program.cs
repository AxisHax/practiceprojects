//	<copyright file="Program.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for Program.
//	</summary>
namespace EthernetIP_Library
{
    using System.Net;

    /// <summary>
    /// Entry point of the application.
    /// </summary>
    internal class Program
    {
        /// <summary>Defines the entry point of the application.</summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            EthernetIPConnection connection = new ();

            try
            {
                connection.Connect(IPAddress.Loopback);
                connection.Disconnect();
            }
            catch (Exception e)
            {
                Console.WriteLine(Properties.Resources.GeneralException, e, e.Message);
            }
        }
    }
}
