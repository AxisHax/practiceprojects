//	<copyright file="SendRRData.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for SendRRData.
//	</summary>
namespace CIP_EthernetIP_Library
{
    /// <summary>
    /// The <see cref="SendRRData"/> command data used to transfer an encapsulated request/reply between the originator and target.
    /// The request data is encapsulate within this object's <see cref="SendRRDataPacket"/> field.
    /// </summary>
    /// <seealso cref="CIP_EthernetIP_Library.MessageBase" />
    internal sealed class SendRRData : MessageBase
    {
        /// <summary>Operation timeout.</summary>
        private ushort timeout;

        /// <summary>Initializes a new instance of the <see cref="SendRRData"/> class. Constructs a request.</summary>
        /// <param name="routePath">The route path.</param>
        /// <param name="paddedEpath">The padded epath.</param>
        public SendRRData(byte[] routePath, byte[] paddedEpath)
        {
            this.Packet = new SendRRDataPacket(routePath, paddedEpath);
            this.DataSize = (ushort)(sizeof(ushort) + sizeof(uint) + this.Packet.DataSize);
        }

        /// <summary>Initializes a new instance of the <see cref="SendRRData"/> class. Constructs a response.</summary>
        /// <param name="responseData">The response data.</param>
        /// <param name="startingOffset">The starting offset.</param>
        /// <param name="validDataLength">Length of the valid data.</param>
        public SendRRData(byte[] responseData, int startingOffset, int validDataLength)
        {
            this.Packet = new SendRRDataPacket(responseData, startingOffset, validDataLength);
            this.DataSize = (ushort)(sizeof(ushort) + sizeof(uint) + this.Packet.DataSize);
        }

        /// <summary>Gets the interface handle. Shall be 0 for CIP.</summary>
        /// <value>The interface handle.</value>
        public static uint InterfaceHandle => 0;

        /// <summary>Gets the length, in bytes, of <see cref="SendRRData"/>.</summary>
        /// <value>The length, in bytes, of this instance.</value>
        public override ushort DataSize { get; }

        /// <summary>Gets or sets the timeout.</summary>
        /// <value>The timeout.</value>
        public ushort Timeout { get => this.timeout; set => this.timeout = value; }

        /// <summary>Gets or sets the <see cref="SendRRDataPacket"/>.</summary>
        /// <value>The <see cref="SendRRDataPacket"/> object of this instance.</value>
        public SendRRDataPacket Packet { get; set; }

        /// <summary>Deserializes the specified buffer.</summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="startingOffset">The starting offset.</param>
        /// <param name="length">The length of valid data in the buffer.</param>
        /// <exception cref="System.InvalidOperationException">Thrown when this method is called from an object that doesn't support deserialization.</exception>
        public override void Deserialize(byte[] buffer, int startingOffset, int length)
        {
            // For now, throw an invalid operation exception.
            // Deserialization isn't supported by this object and should not be called.
            throw new InvalidOperationException(Properties.Resources.DeserializationNotSupportedInvalidOperationException);
        }

        /// <summary>
        /// Serializes this instance.
        /// </summary>
        /// <returns>A byte array containing the serialized data of this message.</returns>
        public override byte[] Serialize()
        {
            byte[] serializedData = new byte[this.DataSize];
            int offset = 0;

            MessageBase.Serialize(InterfaceHandle, serializedData, ref offset);
            MessageBase.Serialize(this.timeout, serializedData, ref offset);
            Array.Copy(MessageBase.Serialize(this.Packet), serializedData, this.Packet.DataSize);

            return serializedData;
        }
    }
}
