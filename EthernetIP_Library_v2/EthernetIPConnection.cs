//	<copyright file="EthernetIPConnection.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for EthernetIPConnection.
//	</summary>
namespace EthernetIP_Library_v2
{
    using System.Net.Sockets;

    /// <summary>
    /// Handles connecting to, registering, and unregistering the EtherNet/IP server.
    /// </summary>
    public static class EthernetIPConnection
    {
        /// <summary>
        /// The reserved TCP port number.
        /// </summary>
        public const int TCPPortNumber = 0xaf12;

        /// <summary>
        /// Expected packet field value for Length.
        /// </summary>
        public const int ExpectedLength = 4;

        /// <summary>
        /// Expected packet field value for Protocol Version.
        /// </summary>
        public const int ExpectedProtocolVersion = 1;

        /// <summary>
        /// Command code constants.
        /// </summary>
        public enum CommandCodes : ushort
        {
            /// <summary>
            /// Register session command code.
            /// </summary>
            RegisterSessionCommand = 0x0065,

            /// <summary>
            /// Unregister session command code.
            /// </summary>
            UnregisterSessionCommand = 0x0066
        }

        /// <summary>
        /// Establish a socket connection and register the connection.
        /// </summary>
        /// <param name="stream">A NetworkStream to use for communication.</param>
        /// <param name="packet">An Encapsulation Packet to send.</param>
        /// <returns>An Encapsulation Packet response from the server.</returns>
        public static EncapsulationPacket Connect(NetworkStream stream, EncapsulationPacket packet)
        {
            // Throw an Argument Null Exception if either stream or packet are null.
            ArgumentNullException.ThrowIfNull(stream);
            ArgumentNullException.ThrowIfNull(packet);

            // Set the fields to the proper values for establishing a session/registering a connection.
            packet.Command = (ushort)CommandCodes.RegisterSessionCommand;
            packet.Length = ExpectedLength;
            packet.ProtocolVersion = ExpectedProtocolVersion;
            packet.SessionHandle = 0;
            packet.Status = 0;
            packet.Options = 0;

            EncapsulationPacket response = new EncapsulationPacket(packet.SenderContext);
            byte[] data = DataProcessing.SerializePacket(packet);
            int packetSize = data.Length;

            stream.Write(data, 0, packetSize);

            data = new byte[packetSize];

            stream.Read(data, 0, packetSize);

            DataProcessing.DeserializePacket(response, data);

            return response;
        }

        /// <summary>
        /// Unregister a registered session.
        /// </summary>
        /// <param name="stream">A NetworkStream to use for communication.</param>
        /// <param name="packet">An Encapsulation Packet.</param>
        public static void Disconnect(NetworkStream stream, EncapsulationPacket packet)
        {
            // Throw an Argument Null Exception if either stream or packet are null.
            ArgumentNullException.ThrowIfNull(stream);
            ArgumentNullException.ThrowIfNull(packet);

            // Set the proper field data for unregistering a session.
            packet.Command = (ushort)CommandCodes.UnregisterSessionCommand;
            packet.Length = 0;

            byte[] data = DataProcessing.SerializePacket(packet);
            int packetSize = data.Length;

            stream.Write(data, 0, packetSize);

            return;
        }
    }
}
