//	<copyright file="Program.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for Program.
//	</summary>
using System.Net.Sockets;

namespace EthernetIP_Library
{
    /// <summary>
    /// Handles establishing, registering, and unregistering an Ethernet/IP connection.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The chosen IP Address to connect to.
        /// </summary>
        private const string IpAddress = "192.168.25.70";

        /// <summary>
        /// Port number that allows us to establish a socket connection.
        /// </summary>
        private const Int32 PortNumber = 0xaf12;

        /// <summary>
        /// The register session command.
        /// </summary>
        private const UInt16 RegisterSessionCommand = 0x0065;

        /// <summary>
        /// The unregister session command.
        /// </summary>
        private const UInt16 UnregisterSessionCommand = 0x0066;

        /// <summary>
        /// The size of the byte array in the encapsulation header.
        /// </summary>
        private const Int32 SizeOfByteArray = 8;

        /// <summary>
        /// The byte buffer size to use when communicating to the target.
        /// </summary>
        private const Int32 BufferSize = (sizeof(UInt16) * 2) + (sizeof(UInt32) * 2) + SizeOfByteArray + sizeof(UInt32) + (sizeof(UInt16) * 2);

        /// <summary>
        /// The encapsulation header and command data structure to be sent through the connection.
        /// </summary>
        public struct EncapsulationHeader
        {
            /// <summary>
            /// Encapsulation command.
            /// </summary>
            public UInt16 Command;

            /// <summary>
            /// Length, in bytes, of the data portion of the message, i.e., the number of bytes following the header.
            /// </summary>
            public UInt16 Length;

            /// <summary>
            /// Session identification (application dependent.
            /// </summary>
            public UInt32 SessionHandle;

            /// <summary>
            /// Status code.
            /// </summary>
            public UInt32 Status;

            /// <summary>
            /// Information pertinent only to the sender of the encapsulation command. Length of 8.
            /// </summary>
            public byte[] SenderContext = new byte[8];

            /// <summary>
            /// Options flag.
            /// </summary>
            public UInt32 Options;

            /// <summary>
            /// Requested protocol version.
            /// </summary>
            public UInt16 ProtocolVersion;

            /// <summary>
            /// Session options
            /// </summary>
            public UInt16 OptionFlags;

            /// <summary>
            /// Initializes a new instance of the <see cref="EncapsulationHeader"/> struct.
            /// </summary>
            public EncapsulationHeader()
            {
            }
        }

        /// <summary>
        /// Establish a socket connection and register the session.
        /// </summary>
        /// <param name="header">The encapsulation header and command data to send through the connection.</param>
        /// <param name="stream">The network stream used to send the header through.</param>
        /// <returns>A response, an encapsulation header.</returns>
        private static EncapsulationHeader Connect(ref EncapsulationHeader header, NetworkStream stream)
        {
            EncapsulationHeader response = new ();
            byte[] dataBuffer = new byte[BufferSize];

            dataBuffer = SerializeHeaderToByteArray(header, dataBuffer);

            stream.Write(dataBuffer, 0, dataBuffer.Length);

            dataBuffer = new byte[BufferSize];
            Int32 bytesRead = stream.Read(dataBuffer, 0, dataBuffer.Length);

            DeserializeByteArrayToHeader(dataBuffer, ref response);

            return response;
        }

        /// <summary>
        /// UnRegister the session.
        /// </summary>
        /// <param name="header">The encapsulation header to be used to unregister the session.</param>
        /// <param name="stream">The network stream used to send the header through.</param>
        private static void UnRegisterSession(ref EncapsulationHeader header, NetworkStream stream)
        {
            header.Command = UnregisterSessionCommand;
            header.Length = 0;
            byte[] dataBuffer = SerializeHeaderToByteArray(header, new byte[BufferSize]);

            stream.Write(dataBuffer, 0, dataBuffer.Length);
        }

        /// <summary>
        /// Convert encapsulation header fields into a byte buffer.
        /// </summary>
        /// <param name="header">The encapsulation header we wish to convert.</param>
        /// <param name="buffer">A byte array to be written to. </param>
        /// <returns>A byte array.</returns>
        private static byte[] SerializeHeaderToByteArray(EncapsulationHeader header, byte[] buffer)
        {
            int offset = 0;

            byte[] commandBytes = BitConverter.GetBytes(header.Command);
            WriteToBuffer(commandBytes, buffer, ref offset);

            byte[] lengthBytes = BitConverter.GetBytes(header.Length);
            WriteToBuffer(lengthBytes, buffer, ref offset);

            byte[] sessionHandleBytes = BitConverter.GetBytes(header.SessionHandle);
            WriteToBuffer(sessionHandleBytes, buffer, ref offset);

            byte[] statusBytes = BitConverter.GetBytes(header.Status);
            WriteToBuffer(statusBytes, buffer, ref offset);

            WriteToBuffer(header.SenderContext, buffer, ref offset);

            byte[] optionsBytes = BitConverter.GetBytes(header.Options);
            WriteToBuffer(optionsBytes, buffer, ref offset);

            byte[] protocolVersionBytes = BitConverter.GetBytes(header.ProtocolVersion);
            WriteToBuffer(protocolVersionBytes, buffer, ref offset);

            byte[] optionFlagsBytes = BitConverter.GetBytes(header.OptionFlags);
            WriteToBuffer(optionFlagsBytes, buffer, ref offset);

            return buffer;
        }

        /// <summary>
        /// Write the contents of a byte buffer into the encapsulation header.
        /// </summary>
        /// <param name="buffer">The byte buffer to be read from.</param>
        /// <param name="header">The encapsulation header to write to.</param>
        private static void DeserializeByteArrayToHeader(byte[] buffer, ref EncapsulationHeader header)
        {
            int offset = 0;

            header.Command = BitConverter.ToUInt16(buffer, offset);
            offset += sizeof(Int16);

            header.Length = BitConverter.ToUInt16(buffer, offset);
            offset += sizeof(UInt16);

            header.SessionHandle = BitConverter.ToUInt32(buffer, offset);
            offset += sizeof(UInt32);

            header.Status = BitConverter.ToUInt32(buffer, offset);
            offset += sizeof(UInt32);

            for (int i = 0; i < SizeOfByteArray; i++)
            {
                header.SenderContext[i] = buffer[offset + i];
            }

            offset += SizeOfByteArray;

            header.Options = BitConverter.ToUInt32(buffer, offset);
            offset += sizeof(UInt32);

            header.ProtocolVersion = BitConverter.ToUInt16(buffer, offset);
            offset += sizeof(UInt16);

            header.OptionFlags = BitConverter.ToUInt16(buffer, offset);
            offset += sizeof(UInt16);
        }

        /// <summary>
        /// Write byte data from values into a byte buffer.
        /// </summary>
        /// <param name="values">The byte buffer to read from.</param>
        /// <param name="buffer">The byte buffer to write to.</param>
        /// <param name="offset">Offset to begin from in the byte buffer.</param>
        private static void WriteToBuffer(byte[] values, byte[] buffer, ref int offset)
        {
            for (int i = 0; i < values.Length; i++)
            {
                buffer[offset + i] = values[i];
            }

            offset += values.Length;
        }

        /// <summary>
        /// Initialize values of an encapsulation header struct.
        /// </summary>
        /// <param name="header">Encapsulation header struct to initialize.</param>
        /// <returns>An EncapsulationHeader struct.</returns>
        private static EncapsulationHeader BuildEncapsulationHeader(EncapsulationHeader header)
        {
            header.Command = RegisterSessionCommand;
            header.Length = 4;
            header.SessionHandle = 0;
            header.Status = 0;
            header.SenderContext = new byte[8];
            header.Options = 0;

            header.ProtocolVersion = 1;
            header.OptionFlags = 0;

            return header;
        }

        /// <summary>
        /// Main method.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            TcpClient client = new TcpClient(IpAddress, PortNumber);
            NetworkStream stream = client.GetStream();

            // Create the encapsulation header.
            EncapsulationHeader header = new ();
            header = BuildEncapsulationHeader(header);

            // Establish the connection and read the response.
            EncapsulationHeader response = Connect(ref header, stream);

            // Unregister the session.
            header = response;
            UnRegisterSession(ref header, stream);
        }
    }
}
