//	<copyright file="Program.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for Program.
//	</summary>
namespace EthernetIP_Library_v3
{
    using System.Net;
    using System.Net.Sockets;

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
            // Setup socket.
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            client.Connect(IPAddress.Loopback.ToString(), EthernetIPConnection.TCPPortNumber);

            // Establish connection and register the session.
            EncapsulationPacket? packet = EthernetIPConnection.RegisterSession(client);

            // Since RegisterSession returns a null packet when encountering an error, we should check for that.
            if (packet != null)
            {
                // Unregister the session.
                EthernetIPConnection.UnRegisterSession(client, packet);
            }
            else
            {
                Console.WriteLine($"{nameof(packet)} is null. Cannot unregister session.");
            }

            client.Disconnect(true);
            client.Close();
        }
    }
}
