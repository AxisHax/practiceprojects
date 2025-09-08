//	<copyright file="EthernetIPConnection.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for EthernetIPConnection.
//	</summary>
namespace EthernetIP_Library
{
    using System.Diagnostics;
    using System.Net;
    using System.Net.Sockets;

    /// <summary>
    /// Handles CIP connections to the server.
    /// </summary>
    internal sealed class EthernetIPConnection
    {
        /// <summary>
        /// The TCP port number to be used for CIP connections.
        /// </summary>
        private const int TcpPortNumber = 0xAF12;

        /// <summary>
        /// Arbitrary sender context for now.
        /// </summary>
        private const int ChosenSenderContext = 970056;

        /// <summary>
        /// Session handle for the current connection.
        /// </summary>
        private uint? sessionHandle;

        /// <summary>
        /// TCP socket client.
        /// </summary>
        private Socket client;

        /// <summary>
        /// Initializes a new instance of the <see cref="EthernetIPConnection"/> class.
        /// </summary>
        public EthernetIPConnection()
        {
            this.client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// Connect via EthernetIP and register a session.
        /// </summary>
        public void Connect()
        {
            // Connect to the server and register the session.
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, TcpPortNumber);
            this.client.Connect(endPoint);

            this.RegisterSession();
        }

        /// <summary>
        /// Unregister session and disconnect from the server.
        /// </summary>
        public void Disconnect()
        {
            // Unregister the session then end the connection to the server.
            this.UnRegisterSession();
            this.client.Disconnect(false);
            this.client.Close();
        }

        /// <summary>
        /// Construct the packet data to be sent to the server.
        /// </summary>
        /// <param name="header">The header portion of the data.</param>
        /// <param name="encapsulatedData">The command specific data.</param>
        /// <returns>A byte array containing the header and encapsulation data.</returns>
        private static byte[] BuildPacket(Header header, RegisterSessionData encapsulatedData)
        {
            byte[] headerBytes = header.GetSerializedHeader();
            byte[] encapsulatedDataBytes = encapsulatedData.GetSerializedData();
            byte[] packet = new byte[headerBytes.Length + encapsulatedDataBytes.Length];

            Array.Copy(headerBytes, packet, header.Length);
            Array.Copy(encapsulatedDataBytes, 0, packet, headerBytes.Length, encapsulatedDataBytes.Length);

            return packet;
        }

        /// <summary>
        /// Read the response from the server.
        /// </summary>
        /// <param name="buffer">The buffer to read from.</param>
        /// <param name="startingOffset">Starting position to read from in the buffer.</param>
        /// <param name="length">The length of the data we want to read from the buffer.</param>
        /// <returns>True if all bytes were read, false otherwise.</returns>
        private bool ReceiveBytes(byte[] buffer, int startingOffset, int length)
        {
            int totalRead = 0;

            while (totalRead != length)
            {
                int readBytes = 0;

                try
                {
                    readBytes = this.client.Receive(buffer, startingOffset + totalRead, length - totalRead, SocketFlags.None);
                }
                catch (SocketException e)
                {
                    Debug.WriteLine($"Socket Exception: {e}");
                }

                if (readBytes == 0)
                {
                    Debug.WriteLine("Nothing was read from the server.");

                    return false;
                }

                totalRead += readBytes;
            }

            return true;
        }

        /// <summary>
        /// Send bytes to the server.
        /// </summary>
        /// <param name="buffer">The buffer to send.</param>
        /// <param name="startingOffset">Starting position to send from in the buffer.</param>
        /// <param name="length">The length of the data we want to send.</param>
        /// <returns>True if all bytes were sent, false otherwise.</returns>
        private bool SendBytes(byte[] buffer, int startingOffset, int length)
        {
            int totalSent = 0;

            while (totalSent != length)
            {
                int sentBytes = 0;

                try
                {
                    sentBytes = this.client.Send(buffer, startingOffset + totalSent, length - totalSent, SocketFlags.None);
                }
                catch (SocketException e)
                {
                    Debug.WriteLine($"Socket Exception: {e}");
                }

                if (sentBytes == 0)
                {
                    Debug.WriteLine("Nothing was sent to the server.");
                    return false;
                }

                totalSent += sentBytes;
            }

            return true;
        }

        /// <summary>
        /// Set the appropriate header data.
        /// </summary>
        /// <param name="command">A session command.</param>
        /// <returns>A <see cref="Header"/> object.</returns>
        private Header SetSession(Commands command)
        {
            Header header = new ()
            {
                Command = command
            };

            switch (header.Command)
            {
                case Commands.RegisterSession:

                    header.Length = 4;
                    header.SessionHandle = this.sessionHandle != null ? this.sessionHandle.Value : 0;
                    header.Status = StatusCodes.Success;

                    // This is a temporary sender context for testing purposes and until a mechanism for making one is made.
                    header.SenderContext = ChosenSenderContext;
                    header.Options = 0;

                    break;

                case Commands.UnRegisterSession:

                    header.Length = 0;
                    header.SessionHandle = this.sessionHandle != null ? this.sessionHandle.Value : 0;
                    header.Status = 0;
                    header.Options = 0;

                    break;
            }

            return header;
        }

        /// <summary>
        /// Registers the session with the server.
        /// </summary>
        private void RegisterSession()
        {
            Header header = this.SetSession(Commands.RegisterSession);
            int expectedResponseLength = header.Length;
            RegisterSessionData encapsulatedData = new ();

            byte[] buffer = BuildPacket(header, encapsulatedData);

            if (!this.SendBytes(buffer, 0, buffer.Length))
            {
                Debug.WriteLine("Could not send data to the server.");
                return;
            }

            Array.Clear(buffer);

            if (!this.ReceiveBytes(buffer, 0, buffer.Length))
            {
                Debug.WriteLine("Could not read data from the server.");

                return;
            }

            // Deserialize the data and save it.
            int dataRegionOffset = header.DeserializeHeader(buffer);

            if (header.Command == Commands.RegisterSession && header.Length == expectedResponseLength)
            {
                encapsulatedData.DeserializeData(buffer, dataRegionOffset);
            }
            else
            {
                throw new InvalidDataException("The data received from the server was of an invalid format.");
            }

            this.sessionHandle = header.SessionHandle;

            // Report any errors.
            if (header.Status != StatusCodes.Success)
            {
                Debug.WriteLine($"\nAn error occurred." + "\n\tStatus code:\t{0:X}.", header.Status);

                if (header.Status == StatusCodes.UnsupportedEncapsulationProtocolRevision)
                {
                    Debug.WriteLine($"\n\tHighest supported protocol version:\t{encapsulatedData.GetProtocolVersion()}. \nSession not created.");
                }
                else
                {
                    Debug.WriteLine("Session not created.");
                }
            }
        }

        /// <summary>
        /// Unregister the current session.
        /// </summary>
        private void UnRegisterSession()
        {
            Header header = this.SetSession(Commands.UnRegisterSession);

            byte[] buffer = header.GetSerializedHeader();

            if (!this.SendBytes(buffer, 0, buffer.Length))
            {
                Debug.WriteLine("Could not send data to the server.");

                return;
            }
        }
    }
}
