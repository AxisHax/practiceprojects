//	<copyright file="EncapsulationPacket.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for EncapsulationPacket.
//	</summary>
namespace EthernetIP_Library_v3
{
    /// <summary>
    /// The encapsulation packet message class and it's associated methods.
    /// </summary>
    public class EncapsulationPacket
    {
        /// <summary>
        /// The header portion of the encapsulation packet.
        /// </summary>
        public EncapsulationHeader Header;

        /// <summary>
        /// ARRAY of 0 - 65511 bytes. Octet. The encapsulation data portion of the message is required only for certain commands.
        /// </summary>
        public CommandSpecificData EncapsulatedData;

        /// <summary>
        /// The length of the command specific/encapsulated data segment. Initialized on object creation.
        /// </summary>
        private int encapsulatedDataLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="EncapsulationPacket"/> class.
        /// </summary>
        /// <param name="encapsulatedDataLength">Length in bytes of the encapsulated data segment.</param>
        public EncapsulationPacket(int encapsulatedDataLength)
        {
            this.Header = new EncapsulationHeader();
            this.EncapsulatedData = new CommandSpecificData(encapsulatedDataLength);
            this.encapsulatedDataLength = encapsulatedDataLength;
        }

        /// <summary>
        /// Get the serialized encapsulation packet data.
        /// </summary>
        /// <returns>A byte array consisting of the encapsulation packet data.</returns>
        public byte[] GetSerializedPacket()
        {
            byte[] serializedPacket = new byte[EncapsulationHeader.HeaderSize + this.encapsulatedDataLength];

            Array.Copy(this.Header.GetSerializedHeader(), 0, serializedPacket, 0, EncapsulationHeader.HeaderSize);
            Array.Copy(this.EncapsulatedData.GetSerializedCommandSpecificData(), 0, serializedPacket, EncapsulationHeader.HeaderSize, this.encapsulatedDataLength);

            return serializedPacket;
        }

        /// <summary>
        /// Get just the serialized header.
        /// </summary>
        /// <returns>A byte array consisting of the encapsulation header data.</returns>
        public byte[] GetSerializedHeader()
        {
            byte[] serializedHeader = new byte[EncapsulationHeader.HeaderSize];

            Array.Copy(this.Header.GetSerializedHeader(), serializedHeader, EncapsulationHeader.HeaderSize);

            return serializedHeader;
        }

        /// <summary>
        /// Deserialize the byte buffer into the current packet data.
        /// </summary>
        /// <param name="buffer">A byte buffer we wish to deserialize.</param>
        public void DeserializeBuffer(byte[] buffer)
        {
            int commandSpecificDataStartOffset = this.Header.DeserializeHeaderData(buffer);
            this.EncapsulatedData.SaveCommandSpecificDataFromBuffer(buffer, commandSpecificDataStartOffset);
        }
    }
}
