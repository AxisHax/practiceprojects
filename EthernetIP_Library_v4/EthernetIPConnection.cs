//	<copyright file="EthernetIPConnection.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for EthernetIPConnection.
//	</summary>
namespace EthernetIP_Library_v4
{
    using System.Net.Sockets;

    /// <summary>
    /// EthernetIP handler class.
    /// </summary>
    public class EthernetIPConnection
    {
        /// <summary>
        /// The TCP port number to communicate with the server with.
        /// </summary>
        public const int TCPPortNumber = 0xAF12;

        /// <summary>
        /// Establish a connection to the server and register a session.
        /// </summary>
        /// <param name="client">A Socket to communicate with the server.</param>
        /// <returns>An encapsulation packet containing a response from the server.</returns>
        public static EncapsulationPacket RegisterSession(Socket client)
        {
            EncapsulationPacket packet = new EncapsulationPacket(4);

            // Set the field values to register the session.
            packet.Header.Command = Commands.Command.RegisterSession;
            packet.Header.SessionHandle = 0;
            packet.Header.Status = StatusCodes.StatusCode.Success;
            packet.Header.SenderContext = 995424;
            packet.Header.Options = 0;

            // Command specific data.
            ushort requestedProtocolVersion = 1;
            ushort sessionOptions = 0;

            packet.Data.AddData(requestedProtocolVersion, sessionOptions);

            byte[] data = packet.GetSerializedPacket();

            // Because we are using a connection oriented protocol (TCP), this is a blocking call and is guaranteed
            // to send all the bytes in the buffer unless a time-out value was reached.
            client.Send(data);

            data = new byte[packet.size];

            // Just like Send(), this is also a blocking call and will be guaranteed to read all bytes unless a
            // time-out value was reached.
            client.Receive(data);

            packet.DeserializePacket(data);

            return packet;
        }

        /// <summary>
        /// Unregister a session from the server.
        /// </summary>
        /// <param name="client">A Socket to communicate with the server.</param>
        /// <param name="packet">An encapsulation packet.</param>
        public static void UnRegisterSession(Socket client, EncapsulationPacket packet)
        {
            // Set the field values to Unregister the session.
            packet.Header.Command = Commands.Command.UnRegisterSession;
            packet.Header.Length = 0;
            packet.Header.Status = 0;
            packet.Header.Options = 0;

            // Only get the serialized packet since we don't need to send the encapsulated data to unregister the session.
            byte[] data = packet.GetOnlySerializedHeader();

            client.Send(data);
        }
    }
}
