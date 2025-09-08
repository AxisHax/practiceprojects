//	<copyright file="EthernetIPConnection.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for EthernetIPConnection.
//	</summary>
namespace EthernetIP_Library_v3
{
    using System.Net.Sockets;

    /// <summary>
    /// Class that handles establishing/registering and unregistering a session on EthernetIP.
    /// </summary>
    public class EthernetIPConnection
    {
        /// <summary>
        /// TCP Port number for EthernetIP.
        /// </summary>
        public const int TCPPortNumber = 0xaf12;

        /// <summary>
        /// List of encapsulation commands.
        /// </summary>
        public enum EncapsulationCommands : ushort
        {
            /// <summary>
            /// NOP command. May be set only using TCP.
            /// </summary>
            NOP = 0x0000,

            /// <summary>
            /// ListServices command. May be sent using either UDP or TCP.
            /// </summary>
            ListServices = 0x0004,

            /// <summary>
            /// ListIdentity command. May be sent using either UDP or TCP.
            /// </summary>
            ListIdentity = 0x0063,

            /// <summary>
            /// ListInterfaces command. Optional, may be sent using either UDP or TCP.
            /// </summary>
            ListInterfaces = 0x0064,

            /// <summary>
            /// RegisterSession command. May be set only using TCP.
            /// </summary>
            RegisterSession = 0x0065,

            /// <summary>
            /// UnRegisterSession command. May be sent only using TCP.
            /// </summary>
            UnRegisterSession = 0x0066,

            /// <summary>
            /// SendRRData command. May be sent only using TCP.
            /// </summary>
            SendRRData = 0x006f,

            /// <summary>
            /// SendUnitData command. May be sent only using TCP.
            /// </summary>
            SendUnitData = 0x0070,

            /// <summary>
            /// IndicateStatus command. Optional, may be sent using only TCP.
            /// </summary>
            IndicateStatus = 0x0072,

            /// <summary>
            /// Cancel command. Optional, may only be sent using TCP.
            /// </summary>
            Cancel = 0x0073,
        }

        /// <summary>
        /// Register session on EthernetIP.
        /// </summary>
        /// <param name="client">Socket client to establish a connection.</param>
        /// <returns>Response from the server.</returns>
        public static EncapsulationPacket? RegisterSession(Socket client)
        {
            // Check if client object is null so we don't have problems.
            ArgumentNullException.ThrowIfNull(client, nameof(client));

            // Command specific data for registering session
            ushort protocolVerion = 1;
            ushort optionsflags = 0;
            ushort dataLength = sizeof(ushort) * 2; // Size in bytes of the command specific data, which is a known value.

            EncapsulationPacket packet = new EncapsulationPacket(dataLength);

            packet.Header.Command = (ushort)EncapsulationCommands.RegisterSession;
            packet.Header.Length = dataLength;
            packet.Header.SenderContext = 978634; // Arbitrarily chosen sender context.
            packet.Header.Options = 0;
            packet.EncapsulatedData.AddData(protocolVerion);
            packet.EncapsulatedData.AddData(optionsflags);

            byte[] serializedPacket = packet.GetSerializedPacket();
            int expectedNumberOfBytes = EncapsulationHeader.HeaderSize + dataLength;
            int i = client.Send(serializedPacket);

            // If we don't send the expected amount of bytes, we have a problem.
            if (i < expectedNumberOfBytes)
            {
                Console.WriteLine($"The expected number of bytes were not sent. \n\tExpected: {expectedNumberOfBytes}.\n\tSent: {i}");
                return null;
            }

            byte[] response = new byte[expectedNumberOfBytes];
            i = client.Receive(response);

            // If we don't read the same amount of bytes for some reason, again, we have a problem.
            if (i < expectedNumberOfBytes)
            {
                Console.WriteLine($"The expected number of bytes were not received. \n\tExpected: {expectedNumberOfBytes}.\n\tReceived: {i}");
                return null;
            }

            packet.DeserializeBuffer(response);

            return packet;
        }

        /// <summary>
        /// UnRegister a registered session.
        /// </summary>
        /// <param name="client">Socket client to handle the connection.</param>
        /// <param name="packet">Encapsulation packet containing the session handle for the currently registered session.</param>
        public static void UnRegisterSession(Socket client, EncapsulationPacket packet)
        {
            // Check if client object is null so we don't have problems.
            ArgumentNullException.ThrowIfNull(client, nameof(client));
            ArgumentNullException.ThrowIfNull(packet, nameof(packet));

            // Field data values for unregistering a session.
            packet.Header.Command = (ushort)EncapsulationCommands.UnRegisterSession;
            packet.Header.Length = 0;
            packet.Header.Status = 0;
            packet.Header.Options = 0;

            byte[] serializedHeader = packet.GetSerializedHeader();
            int expectedNumberOfBytes = EncapsulationHeader.HeaderSize;
            int i = client.Send(serializedHeader);

            if (i < expectedNumberOfBytes)
            {
                Console.WriteLine($"The expected number of bytes were not sent. \n\tExpected: {expectedNumberOfBytes}.\n\tSent: {i}");
                return;
            }
        }
    }
}
