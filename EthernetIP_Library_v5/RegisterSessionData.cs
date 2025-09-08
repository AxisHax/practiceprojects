//	<copyright file="RegisterSessionData.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>   
//	<summary>
//		Class file for RegisterSessionData.
//	</summary>
namespace EthernetIP_Library
{
    /// <summary>
    ///  The <see cref="RegisterSessionData"/> class represents the RegisterSession request and reply data.
    /// </summary>
    internal sealed class RegisterSessionData : MessageBase
    {
        /// <summary>
        /// Size of this data segment.
        /// </summary>
        private const int DataSize = sizeof(ushort) + sizeof(ushort);

        /// <summary>
        /// The requested protocol version
        /// </summary>
        private ushort protocolVersion;

        /// <summary>
        /// Session options.
        /// </summary>
        private ushort optionsFlags;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterSessionData"/> class.
        /// </summary>
        public RegisterSessionData()
        {
            this.protocolVersion = 1;
            this.optionsFlags = 0;
        }

        /// <summary>
        /// Get the protocol version.
        /// </summary>
        /// <returns>The currently set/supported protocol version by the server, a <see cref="ushort"/> value.</returns>
        public ushort GetProtocolVersion()
        {
            return this.protocolVersion;
        }

        /// <summary>
        /// Get the serialized Register Session Data.
        /// </summary>
        /// <returns>A byte array containing the serialized Register Session Data.</returns>
        public byte[] GetSerializedData()
        {
            byte[] serializedData = new byte[DataSize];
            int offset = 0;

            this.Serialize(this.protocolVersion, serializedData, ref offset);
            this.Serialize(this.optionsFlags, serializedData, ref offset);

            return serializedData;
        }

        /// <summary>
        /// Deserialize the RegisterSession reply and store it into the current instance of the object.
        /// </summary>
        /// <param name="buffer">The byte buffer we wish to read from.</param>
        /// <param name="offset">An offset which indicates the end of the header data and the start of the encapsulated data region.</param>
        /// <exception cref="InvalidDataException">Exception thrown when the buffer size is larger than expected.</exception>
        public void DeserializeData(byte[] buffer, int offset)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            int dataRegion = buffer.Length - offset;

            if (dataRegion > DataSize)
            {
                throw new InvalidDataException($"The data contained in {nameof(buffer)} is larger than expected.");
            }

            this.protocolVersion = this.Deserialize(this.protocolVersion, buffer, ref offset);
            this.optionsFlags = this.Deserialize(this.optionsFlags, buffer, ref offset);
        }
    }
}
