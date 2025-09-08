//	<copyright file="Program.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for Program.
//	</summary>
namespace TLS_Client
{
    /// <summary>
    /// Program class containing Main.
    /// </summary>
    internal class Program
    {
        /// <summary>Defines the entry point of the application.</summary>
        /// <param name="args">The command line arguments.</param>
        private static void Main(string[] args)
        {
            Client client;

            if (args.Length == 0)
            {
                client = new ();
            }
            else
            {
                string host = args[0];
                client = new (host);
            }

            client.Run();
            client.Stop();
        }
    }
}
