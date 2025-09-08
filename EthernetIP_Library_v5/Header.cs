//	<copyright file="Header.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for Header.
//	</summary>
namespace EthernetIP_Library
{
    /// <summary>
    /// The <see cref="Header"/> class. Inherits from <see cref="MessageBase"/>.
    /// </summary>
    internal sealed class Header : MessageBase
    {
        /// <summary>
        /// The size, in bytes, of the header.
        /// </summary>
        public const int HeaderSize = 24;

        /// <summary>
        /// Initializes a new instance of the <see cref="Header"/> class.
        /// </summary>
        public Header()
        {
            this.Command = Commands.NOP;
            this.Length = 0;
            this.SessionHandle = 0;
            this.Status = StatusCodes.Success;
            this.SenderContext = 0;
            this.Options = 0;
        }

        /// <summary>
        /// Gets or sets the encapsulation command.
        /// </summary>
        public Commands Command { get; set; }

        /// <summary>
        /// Gets or sets the length, in bytes, of the data portion of the message, i.e, the number of bytes following the header.
        /// </summary>
        public ushort Length { get; set; }

        /// <summary>
        /// Gets or sets the session identification (application dependent).
        /// </summary>
        public uint SessionHandle { get; set; }

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        public StatusCodes Status { get; set; }

        /// <summary>
        /// Gets or sets the sender context. (Information pertinent only to the sender of an encapsulation method)
        /// </summary>
        public long SenderContext { get; set; }

        /// <summary>
        /// Gets or sets options flags.
        /// </summary>
        public uint Options { get; set; }

        /// <summary>
        /// Get the serialized header data.
        /// </summary>
        /// <returns>A byte array containing the header data.</returns>
        public byte[] GetSerializedHeader()
        {
            return this.Serialize(this);
        }

        /// <summary>
        /// Deserialize and store the given data into this header.
        /// </summary>
        /// <param name="buffer">A byte array that contains the serialized packet data.</param>
        /// <returns>The end position of the header data in the byte buffer.</returns>
        public int DeserializeHeader(byte[] buffer)
        {
            return this.Deserialize(this, buffer);
        }
    }
}
