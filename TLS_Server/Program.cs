//	<copyright file="Program.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for Program.
//	</summary>
namespace TLS_Server
{
    /// <summary>
    /// Program class that contains Main.
    /// </summary>
    internal class Program
    {
        /// <summary>Defines the entry point of the application.</summary>
        /// <param name="args">The command line arguments.</param>
        private static void Main(string[] args)
        {
            Server server = new Server();

            server.Run();
            server.Stop();
        }
    }
}
