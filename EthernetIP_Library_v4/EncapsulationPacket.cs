//	<copyright file="EncapsulationPacket.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for EncapsulationPacket.
//	</summary>
namespace EthernetIP_Library_v4
{
    /// <summary>
    /// Class for the encapsulation packet.
    /// </summary>
    public sealed class EncapsulationPacket : MessageBase
    {
        /// <summary>
        /// The header portion of the encapsulation packet.
        /// </summary>
        public Header Header;

        /// <summary>
        /// The encapsulation data portion of the message. Is required only for certain commands.
        /// </summary>
        public CommandSpecificData Data;

        /// <summary>
        /// Initializes a new instance of the <see cref="EncapsulationPacket"/> class.
        /// </summary>
        /// <param name="lengthOfDataPortion">The length, in bytes, of the command specific data region.</param>
        public EncapsulationPacket(ushort lengthOfDataPortion) : base(Header.HeaderSize + lengthOfDataPortion)
        {
            this.Header = new Header(lengthOfDataPortion);
            this.Data = new CommandSpecificData(lengthOfDataPortion);
        }

        /// <summary>
        /// Get the serialized packet.
        /// </summary>
        /// <returns>A byte array containing the serialized packet data.</returns>
        public byte[] GetSerializedPacket()
        {
            byte[] serializedPacket = new byte[this.size];

            this.Serialize(this.Header.GetSerializedHeader(), this.Data.GetEncapsulatedData(), serializedPacket);

            return serializedPacket;
        }

        /// <summary>
        /// Get only the serialized header.
        /// </summary>
        /// <returns>A byte array containing only the serialized header.</returns>
        public byte[] GetOnlySerializedHeader()
        {
            return this.Header.GetSerializedHeader();
        }

        /// <summary>
        /// Deserialize the data in the given buffer and store it into the current instance of this object.
        /// </summary>
        /// <param name="buffer">A byte array containing the encapsulation packet data.</param>
        public void DeserializePacket(byte[] buffer)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            int dataRegionStartOffset = this.Header.DeserializeHeader(buffer);

            this.Data.DeserializeEncapsulatedData(buffer, dataRegionStartOffset);
        }
    }
}
