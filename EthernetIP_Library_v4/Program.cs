//	<copyright file="Program.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for Program.
//	</summary>
namespace EthernetIP_Library
{
    using System.Net;
    using System.Net.Sockets;
    using EthernetIP_Library_v4;

    /// <summary>
    /// Entry point of the application.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Main method.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static void Main(string[] args)
        {
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, EthernetIPConnection.TCPPortNumber);

            client.Connect(endPoint);

            EncapsulationPacket response = EthernetIPConnection.RegisterSession(client);

            EthernetIPConnection.UnRegisterSession(client, response);

            client.Disconnect(false);

            client.Close();
        }
    }
}
