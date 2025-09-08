//	<copyright file="EthernetIPConnection.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for EthernetIPConnection.
//	</summary>
namespace EthernetIP_Library
{
    using System.Net;
    using System.Net.Sockets;

    /// <summary>
    /// Handles CIP connections to the server.
    /// </summary>
    internal sealed class EthernetIPConnection
    {
        /// <summary>
        /// The TCP port number.
        /// </summary>
        private const int TcpPortNumber = 0xAF12;

        /// <summary>
        /// The chosen sender context.
        /// </summary>
        private const int ChosenSenderContext = 976442; // Keeping this for testing purposes.

        /// <summary>The TCP socket client.</summary>
        private readonly Socket client;

        /// <summary>
        /// The session handle.
        /// </summary>
        private uint? sessionHandle; // Keeping as nullable int because it's cool and it works.

        /// <summary>
        /// Initializes a new instance of the <see cref="EthernetIPConnection"/> class.
        /// </summary>
        public EthernetIPConnection()
        {
            this.client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// Connect via EthernetIP and register a session with the server.
        /// </summary>
        /// <param name="address">The device to connect to.</param>
        public void Connect(IPAddress address)
        {
            // Connect to the server and register the session.
            IPEndPoint endPoint = new IPEndPoint(address, TcpPortNumber);
            this.client.Connect(endPoint);

            this.RegisterSession();
        }

        /// <summary>
        /// Unregister the session and disconnect from the server.
        /// </summary>
        public void Disconnect()
        {
            this.UnregisterSession();
            this.client.Disconnect(false);
            this.client.Close();
        }

        /// <summary>Builds the packet.</summary>
        /// <param name="header">The header.</param>
        /// <param name="sessionData">The session data.</param>
        /// <returns>A byte array containing the header and encapsulated data.</returns>
        private static byte[] BuildPacket(Header header, MessageBase sessionData)
        {
            byte[] headerBytes = header.Serialize();
            byte[] encapsulatedData = sessionData.Serialize();
            byte[] packet = new byte[header.DataSize + sessionData.DataSize];

            Array.Copy(headerBytes, packet, header.DataSize);
            Array.Copy(encapsulatedData, 0, packet, header.DataSize, sessionData.DataSize);

            return packet;
        }

        /// <summary>Sends the bytes to the server.</summary>
        /// <param name="buffer">The buffer to send.</param>
        /// <param name="startingOffset">The starting offset to send the data from in the buffer.</param>
        /// <param name="length">The length of the data to send.</param>
        /// <returns>True if all bytes were sent, false otherwise.</returns>
        private bool SendBytes(byte[] buffer, int startingOffset, int length)
        {
            int totalSent = 0;

            while (totalSent != length)
            {
                int sentBytes;
                try
                {
                    sentBytes = this.client.Send(buffer, startingOffset + totalSent, length - totalSent, SocketFlags.None);
                }
                catch (SocketException)
                {
                    return false;
                }

                totalSent += sentBytes;
            }

            return true;
        }

        /// <summary>Receives bytes from the server.</summary>
        /// <param name="buffer">The buffer to receive data into.</param>
        /// <param name="startingOffset">The starting offset we want to insert data into.</param>
        /// <param name="length">The length of the data to receive.</param>
        /// <returns> True if all bytes were received, false otherwise.</returns>
        private bool ReceiveBytes(byte[] buffer, int startingOffset, int length)
        {
            int totalReceived = 0;

            while (totalReceived != length)
            {
                int receivedBytes;
                try
                {
                    receivedBytes = this.client.Receive(buffer, startingOffset + totalReceived, length - totalReceived, SocketFlags.None);
                }
                catch (SocketException)
                {
                    return false;
                }

                if (receivedBytes == 0)
                {
                    return false;
                }

                totalReceived += receivedBytes;
            }

            return true;
        }

        /// <summary>Registers the session.</summary>
        /// <exception cref="System.IO.IOException">
        /// Could not send data to the server.
        /// or
        /// Could not read data from the server.
        /// or
        /// \nAn error occurred." + $"\n\tStatus code:\t{header.Status:X}." + $"\n\tHighest supported protocol version:\t{sessionData.ProtocolVersion}. \nSession not created.
        /// or
        /// \nAn error occurred." + $"\n\tStatus code:\t{header.Status:X}." + "\nSession not created.
        /// </exception>
        /// <exception cref="System.FormatException">
        /// The data received was of an invalid length.
        /// or
        /// Header data is invalid or not in the correct format.
        /// or
        /// The command specific data returned wasn't in the correct format.
        /// </exception>
        private void RegisterSession()
        {
            RegisterSessionData sessionData = new RegisterSessionData();
            Header header = new Header()
            {
                Command = Command.RegisterSession,
                Length = sessionData.DataSize,
                SenderContext = ChosenSenderContext,
                SessionHandle = this.sessionHandle != null ? this.sessionHandle.Value : 0
            };

            // Build the encapsulation packet.
            byte[] buffer = BuildPacket(header, sessionData);

            // Check to see if all bytes were sent to the server.
            if (!this.SendBytes(buffer, 0, buffer.Length))
            {
                throw new IOException(Properties.Resources.UnableToSendDataIOException);
            }

            Array.Clear(buffer);

            // Check to see if we read all bytes from the server.
            if (!this.ReceiveBytes(buffer, 0, buffer.Length))
            {
                throw new IOException(Properties.Resources.UnableToReadDataIOException);
            }

            int expectedResponseLength = header.DataSize + sessionData.DataSize;

            // Checking to see if the buffer is of the proper length. If not then it means the data is invalid.
            if (buffer.Length != expectedResponseLength)
            {
                throw new FormatException(Properties.Resources.InvalidDataLengthFormatException);
            }

            // Deserialize the header portion.
            byte[] headerData = new byte[header.DataSize];
            Array.Copy(buffer, 0, headerData, 0, header.DataSize);

            header.Deserialize(headerData, 0, headerData.Length);

            // Check to see if the command and sender context is the same. If not, then the data is invalid.
            // The check for sender context is to see if we're even playing with the right data.
            if (header.Command != Command.RegisterSession && header.SenderContext != ChosenSenderContext)
            {
                throw new FormatException(Properties.Resources.InvalidHeaderFormatException);
            }
            else if ((buffer.Length - header.DataSize) != sessionData.DataSize)
            {
                // If the command specific data region size doesn't match what we expect, throw an exception.
                throw new FormatException(Properties.Resources.IncorrectCommandSpecificDataFormatException);
            }

            byte[] csd = new byte[sessionData.DataSize];
            Array.Copy(buffer, header.DataSize, csd, 0, sessionData.DataSize);

            sessionData.Deserialize(csd, 0, csd.Length);

            // Throw exception if we don't have the correct protocol version.
            if (header.Status != StatusCode.Success)
            {
                if (header.Status == StatusCode.UnsupportedEncapsulationProtocolRevision)
                {
                    throw new IOException(String.Format(Properties.Resources.ErrorCodeUnsupportedProtocolVersionException, header.Status, sessionData.ProtocolVersion));
                }
                else
                {
                    throw new IOException(String.Format(Properties.Resources.GeneralIOException, header.Status));
                }
            }

            // Update the current session handle.
            this.sessionHandle = header.SessionHandle;
        }

        /// <summary>
        /// Unregisters the session.
        /// </summary>
        /// <exception cref="System.IO.IOException">Could not send data to the server.</exception>
        private void UnregisterSession()
        {
            Header header = new Header()
            {
                Command = Command.UnRegisterSession,
                Length = 0,
                SenderContext = ChosenSenderContext,
                SessionHandle = this.sessionHandle != null ? this.sessionHandle.Value : 0
            };

            byte[] buffer = header.Serialize();

            if (!this.SendBytes(buffer, 0, buffer.Length))
            {
                throw new IOException(Properties.Resources.UnableToSendDataIOException);
            }
        }
    }
}
