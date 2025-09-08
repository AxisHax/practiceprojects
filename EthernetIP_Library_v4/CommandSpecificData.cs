//	<copyright file="CommandSpecificData.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for CommandSpecificData.
//	</summary>
namespace EthernetIP_Library_v4
{
    /// <summary>
    /// Class representing the encapsulated data segment of the encapsulation packet.
    /// </summary>
    public sealed class CommandSpecificData : MessageBase
    {
        /// <summary>
        /// The maximum size, in bytes, of the encapsulated data region.
        /// </summary>
        public const int MaxSize = 65511;

        /// <summary>
        /// The encapsulated data.
        /// </summary>
        public byte[] EncapsulatedData;

        /// <summary>
        /// Current offset. Used only for storing the data initially.
        /// </summary>
        private int currentOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandSpecificData"/> class.
        /// </summary>
        /// <param name="length">The size, in bytes, of the encapsulated data region.</param>
        /// <exception cref="ArgumentOutOfRangeException">Exception thrown if attempting to allocate size larger than the maximum permitted size.</exception>
        public CommandSpecificData(int length) : base(length)
        {
            // We cannot have a data region larger than the maximum permitted size.
            if (length > MaxSize)
            {
                throw new ArgumentOutOfRangeException($"The length of the data segment cannot be greater than {MaxSize}.", nameof(length));
            }

            this.EncapsulatedData = new byte[length];
            this.currentOffset = 0;
        }

        /// <summary>
        /// Insert data into the encapsulated data region.
        /// </summary>
        /// <param name="values">Any number of data to add to the data region.</param>
        /// <exception cref="ArgumentException">Exception thrown if attempting to add more data then there's space for.</exception>
        public void AddData(params ushort[] values)
        {
            // Make sure we aren't trying to add more data than we allocated space for.
            if (values.Length > this.size)
            {
                throw new ArgumentException($"Attempting to store more data than is allocated for. Check how many parameters you're passing in.", nameof(values));
            }

            foreach (ushort value in values)
            {
                this.Serialize(value, this.EncapsulatedData, ref this.currentOffset);
            }
        }

        /// <summary>
        /// Get the encapsulated data.
        /// </summary>
        /// <returns>A byte array that contains the encapsulated data.</returns>
        public byte[] GetEncapsulatedData()
        {
            return this.EncapsulatedData;
        }

        /// <summary>
        /// Deserialize the encapsulated data region returned from the server.
        /// </summary>
        /// <param name="buffer">The byte buffer containing the response data from the server.</param>
        /// <param name="offset">The starting position of the encapsulated data region.</param>
        /// <exception cref="InvalidDataException">Exception thrown if the size of the data segment we want to read from is larger than expected.</exception>
        public void DeserializeEncapsulatedData(byte[] buffer, int offset)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            int encapsulatedDataRegion = buffer.Length - offset;

            if (encapsulatedDataRegion > this.size)
            {
                throw new InvalidDataException($"The data contained in {nameof(buffer)} is larger than expected.");
            }
            this.Deserialize(buffer, EncapsulatedData, offset);
        }
    }
}
