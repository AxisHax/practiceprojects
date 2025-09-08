//	<copyright file="RegisterSessionData.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for RegisterSessionData.
//	</summary>
namespace EthernetIP_Library
{
    /// <summary>
    /// The <see cref="RegisterSessionData"/> class represents the RegisterSession request and reply data.
    /// </summary>
    /// <seealso cref="EthernetIP_Library.MessageBase" />
    internal sealed class RegisterSessionData : MessageBase
    {
        /// <summary>
        /// The requested protocol version
        /// </summary>
        private const ushort DefaultProtocolVersion = 1;

        /// <summary>
        /// The requested session options
        /// </summary>
        private const ushort DefaultSessionOptions = 0;

        /// <summary>
        /// The protocol version
        /// </summary>
        private ushort protocolVersion;

        /// <summary>
        /// The options flags
        /// </summary>
        private ushort optionsFlags;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterSessionData"/> class.
        /// </summary>
        public RegisterSessionData()
        {
            this.protocolVersion = DefaultProtocolVersion;
            this.optionsFlags = DefaultSessionOptions;
        }

        /// <summary>
        /// Gets the size of the data.
        /// </summary>
        /// <value>
        /// The size of the data.
        /// </value>
        public override ushort DataSize => sizeof(ushort) + sizeof(ushort);

        /// <summary>
        /// The requested protocol version
        /// </summary>
        public ushort ProtocolVersion => this.protocolVersion;

        /// <summary>
        /// Session options.
        /// </summary>
        public ushort OptionsFlags => this.optionsFlags;

        /// <summary>
        /// Serializes this instance.
        /// </summary>
        /// <returns>A byte array containing the serialized data of this instance.</returns>
        public override byte[] Serialize()
        {
            byte[] serializedData = new byte[this.DataSize];
            int offset = 0;

            MessageBase.Serialize(this.ProtocolVersion, serializedData, ref offset);
            MessageBase.Serialize(this.OptionsFlags, serializedData, ref offset);

            return serializedData;
        }

        /// <summary>
        /// Deserialize data from the provided buffer into this instance.
        /// </summary>
        /// <param name="buffer">The data buffer.</param>
        /// <param name="startingOffset">Starting offset to read from in the buffer.</param>
        /// <param name="length">The length of the data to read.</param>
        /// <exception cref="InvalidDataException">Thrown when the provided length, in bytes, of data is too small to possibly represent <see cref="RegisterSessionData"/>.</exception>
        public override void Deserialize(byte[] buffer, int startingOffset, int length)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            if (length < this.DataSize)
            {
                throw new InvalidDataException(String.Format(Properties.Resources.BufferTooSmallInvalidDataException, nameof(buffer), nameof(RegisterSessionData), this.DataSize));
            }

            int offset = startingOffset;

            MessageBase.Deserialize(ref this.protocolVersion, buffer, ref offset);
            MessageBase.Deserialize(ref this.optionsFlags, buffer, ref offset);
        }
    }
}
