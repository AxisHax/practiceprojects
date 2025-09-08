//	<copyright file="Header.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for Header.
//	</summary>
namespace EthernetIP_Library_v4
{
    using static EthernetIP_Library_v4.Commands;
    using static EthernetIP_Library_v4.StatusCodes;

    /// <summary>
    /// Class representing the header segment of the encapsulation packet.
    /// </summary>
    public sealed class Header : MessageBase
    {
        /// <summary>
        /// The size, in bytes, of the header.
        /// </summary>
        public const int HeaderSize = 24;

        /// <summary>
        /// The encapsulation command.
        /// </summary>
        public Command Command;

        /// <summary>
        /// The length, in bytes, of the data portion of the message, i.e, the number of bytes following the header.
        /// </summary>
        public ushort Length;

        /// <summary>
        /// Session identification (application dependent).
        /// </summary>
        public uint SessionHandle;

        /// <summary>
        /// Status code.
        /// </summary>
        public StatusCode Status;

        /// <summary>
        /// Information pertinent only to the sender of an encapsulation method.
        /// </summary>
        public long SenderContext;

        /// <summary>
        /// Options flags.
        /// </summary>
        public uint Options;

        /// <summary>
        /// Initializes a new instance of the <see cref="Header"/> class.
        /// </summary>
        /// <param name="lengthOfDataPortion">Length, in bytes, of the command specific data region following the header.</param>
        public Header(ushort lengthOfDataPortion) : base(HeaderSize)
        {
            // Set default field values.
            this.Command = Commands.Command.NOP;
            this.Length = lengthOfDataPortion;
            this.SessionHandle = 0;
            this.Status = 0;
            this.SenderContext = 0;
            this.Options = 0;
        }

        /// <summary>
        /// Serialize the header to a byte array.
        /// </summary>
        /// <returns>A byte array containing the serialized header.</returns>
        public byte[] GetSerializedHeader()
        {
            byte[] headerData = new byte[HeaderSize];
            int offset = 0;

            this.Serialize((ushort)this.Command, headerData, ref offset);
            this.Serialize(this.Length, headerData, ref offset);
            this.Serialize(this.SessionHandle, headerData, ref offset);
            this.Serialize((uint)this.Status, headerData, ref offset);
            this.Serialize(this.SenderContext, headerData, ref offset);
            this.Serialize(this.Options, headerData, ref offset);

            return headerData;
        }

        /// <summary>
        /// Deserialize the header portion of the buffer and store it into the current instance of the object.
        /// </summary>
        /// <param name="buffer">The byte buffer we wish to read from.</param>
        /// <returns>An offset which indicates the end of the header data and the start of the command specific data region.</returns>
        public int DeserializeHeader(byte[] buffer)
        {
            int offset = 0;

            this.Deserialize(ref this.Command, buffer, ref offset);
            this.Deserialize(ref this.Length, buffer, ref offset);
            this.Deserialize(ref this.SessionHandle, buffer, ref offset);
            this.Deserialize(ref this.Status, buffer, ref offset);
            this.Deserialize(ref this.SenderContext, buffer, ref offset);
            this.Deserialize(ref this.Options, buffer, ref offset);

            return offset;
        }
    }
}
