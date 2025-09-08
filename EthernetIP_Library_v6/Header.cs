//	<copyright file="Header.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for Header.
//	</summary>
namespace EthernetIP_Library
{
    /// <summary>
    /// The header class. Inherits from MessageBase.
    /// </summary>
    /// <seealso cref="EthernetIP_Library.MessageBase" />
    internal sealed class Header : MessageBase
    {
        /// <summary>
        /// The command
        /// </summary>
        private Command command;

        /// <summary>
        /// The length
        /// </summary>
        private ushort length;

        /// <summary>
        /// The session handle
        /// </summary>
        private uint sessionHandle;

        /// <summary>
        /// The status
        /// </summary>
        private StatusCode status;

        /// <summary>
        /// The sender context
        /// </summary>
        private long senderContext;

        /// <summary>
        /// The options
        /// </summary>
        private uint options;

        /// <summary>
        /// Initializes a new instance of the <see cref="Header"/> class.
        /// </summary>
        public Header()
        {
            this.Command = Command.Nop;
            this.Status = StatusCode.Success;
        }

        /// <summary>
        /// The size, in bytes, of the header.
        /// </summary>
        public override ushort DataSize => 24;

        /// <summary>
        /// Gets or sets the encapsulation command.
        /// </summary>
        public Command Command { get => this.command; set => this.command = value; }

        /// <summary>
        /// Gets or sets the length, in bytes, of the data portion of the message, i.e, the number of bytes following the header.
        /// </summary>
        public ushort Length { get => this.length; set => this.length = value; }

        /// <summary>
        /// Gets or sets the session identification (application dependent).
        /// </summary>
        public uint SessionHandle { get => this.sessionHandle; set => this.sessionHandle = value; }

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        public StatusCode Status { get => this.status; set => this.status = value; }

        /// <summary>
        /// Gets or sets the sender context. (Information pertinent only to the sender of an encapsulation method)
        /// </summary>
        public long SenderContext { get => this.senderContext; set => this.senderContext = value; }

        /// <summary>
        /// Gets or sets options flags.
        /// </summary>
        public uint Options { get => this.options; set => this.options = value; }

        /// <summary>
        /// Serializes this instance.
        /// </summary>
        /// <returns>A byte array containing the serialized data of this instance.</returns>
        public override byte[] Serialize()
        {
            byte[] serializedData = new byte[this.DataSize];
            int offset = 0;

            MessageBase.Serialize(this.Command, serializedData, ref offset);
            MessageBase.Serialize(this.Length, serializedData, ref offset);
            MessageBase.Serialize(this.SessionHandle, serializedData, ref offset);
            MessageBase.Serialize(this.Status, serializedData, ref offset);
            MessageBase.Serialize(this.SenderContext, serializedData, ref offset);
            MessageBase.Serialize(this.Options, serializedData, ref offset);

            return serializedData;
        }
        
        /// <summary>
        /// Deserialize data from the provided buffer into this instance.
        /// </summary>
        /// <param name="buffer">The data buffer.</param>
        /// <param name="startingOffset">Starting offset to read from in the buffer.</param>
        /// <param name="length">The length of the data to read.</param>
        /// <exception cref="InvalidDataException">Thrown when the provided length, in bytes, of data is too small to possibly represent <see cref="Header"/>.</exception>
        public override void Deserialize(byte[] buffer, int startingOffset, int length)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            if (length < this.DataSize)
            {
                throw new InvalidDataException(String.Format(Properties.Resources.BufferTooSmallInvalidDataException, nameof(buffer), nameof(Header), this.DataSize));
            }

            int offset = startingOffset;

            MessageBase.Deserialize(ref this.command, buffer, ref offset);
            MessageBase.Deserialize(ref this.length, buffer, ref offset);
            MessageBase.Deserialize(ref this.sessionHandle, buffer, ref offset);
            MessageBase.Deserialize(ref this.status, buffer, ref offset);
            MessageBase.Deserialize(ref this.senderContext, buffer, ref offset);
            MessageBase.Deserialize(ref this.options, buffer, ref offset);
        }
    }
}
