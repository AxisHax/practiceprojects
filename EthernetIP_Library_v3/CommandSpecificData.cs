//	<copyright file="CommandSpecificData.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for CommandSpecificData.
//	</summary>
namespace EthernetIP_Library_v3
{
    /// <summary>
    /// The encapsulated data segment of the encapsulation packet and its associated methods.
    /// </summary>
    public class CommandSpecificData
    {
        /// <summary>
        /// Starting offset of the command specific data in the encapsulation packet
        /// </summary>
        private const int CommandSpecificDataRegion = 24;

        /// <summary>
        /// Command specific data stored in a byte array for convenience when sending. Size determined on sending commands.
        /// </summary>
        private byte[] encapsulatedData;

        /// <summary>
        /// The offset used to insert elements into the Data array once initialized.
        /// </summary>
        private int currentOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandSpecificData"/> class.
        /// </summary>
        /// <param name="encapsulatedDataLength">Length of the data in bytes.</param>
        public CommandSpecificData(int encapsulatedDataLength)
        {
            this.encapsulatedData = new byte[encapsulatedDataLength];
            this.currentOffset = 0;
        }

        /// <summary>
        /// Add command specific data.
        /// </summary>
        /// <param name="data">Data to be added to the command specific data segment.</param>
        public void AddData(ushort data)
        {
            byte[] dataBytes = BitConverter.GetBytes(data);

            Array.Copy(dataBytes, 0, this.encapsulatedData, this.currentOffset, dataBytes.Length);

            this.currentOffset += dataBytes.Length;
        }

        /// <summary>
        /// Get the serialized command specific data segment.
        /// </summary>
        /// <returns>A byte array.</returns>
        public byte[] GetSerializedCommandSpecificData()
        {
            return this.encapsulatedData;
        }

        /// <summary>
        /// Store the command specific data.
        /// </summary>
        /// <param name="buffer">A buffer we want to read.</param>
        /// <param name="startingOffset">Starting position to read from.</param>
        public void SaveCommandSpecificDataFromBuffer(byte[] buffer, int startingOffset)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            // The command specific data region is expected to begin after the header, which is 24 bytes long.
            // If the starting offset is greater than 24 we may be trying to read invalid data, or something went wrong somewhere.
            if (startingOffset > CommandSpecificDataRegion)
            {
                throw new InvalidDataException($"The given offset is larger than what was expected. Check header size or offset value.");
            }

            // Get how many elements are in the encapsulated data segment.
            int numberOfElements = buffer.Length - startingOffset;

            // If the number of elements we want to copy is greater than our length it means the data must be invalid.
            if (numberOfElements > this.encapsulatedData.Length)
            {
                throw new InvalidDataException($"Attempting to write more data from \"{nameof(buffer)}\" to \"{nameof(this.encapsulatedData)}\" than expected.");
            }

            Array.Copy(buffer, startingOffset, this.encapsulatedData, 0, this.encapsulatedData.Length);
        }
    }
}
