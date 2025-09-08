//	<copyright file="Program.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for Program.
//	</summary>
namespace EthernetIP_Library_v2
{
    using System.IO;
    using System.Net;
    using System.Net.Sockets;

    /// <summary>
    /// Driver code.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Main method.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            // Define encapsulation packet so we can establish a connection.
            EncapsulationPacket header = new EncapsulationPacket(544219); // Arbitrary sender context.

            TcpClient client = new TcpClient(IPAddress.Loopback.ToString(), EthernetIPConnection.TCPPortNumber);
            NetworkStream stream = client.GetStream();

            header = EthernetIPConnection.Connect(stream, header);

            EthernetIPConnection.Disconnect(stream, header);
        }
    }
}
