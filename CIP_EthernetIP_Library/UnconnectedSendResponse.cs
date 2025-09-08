//	<copyright file="UnconnectedSendResponse.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for UnconnectedSendResponse.
//	</summary>

namespace CIP_EthernetIP_Library
{
    using CIP_EthernetIP_Library.EnumStructures;

    /// <summary>
    /// Structure of the server response after receiving an <see cref="UnconnectedSendRequest"/>.
    /// </summary>
    /// <seealso cref="CIP_EthernetIP_Library.MessageBase" />
    internal class UnconnectedSendResponse : MessageBase
    {
        /// <summary>This is the value added to the service code sent. Subtract this from <see cref="replyService"/> to get the actual code.</summary>
        private const byte ReplyCode = 0x80;

        /// <summary>The reply service code.</summary>
        private CipCommonServiceCode replyService;

        /// <summary>
        /// The value is 0 for successful transactions, otherwise it will be one of the other <see cref="CipGeneralStatusCode"/>s.
        /// If a routing error occurred, it shall be limited to the values specified in <see cref="RoutingErrorValues"/>.
        /// </summary>
        private CipGeneralStatusCode generalStatus;

        /// <summary>The number of 16-bit words in <see cref="AdditionalStatus"/>.</summary>
        private byte sizeOfAdditionalStatus;

        /// <summary>
        /// When returning an error from a target which is a DeviceNet node, the Additional Status shall contain the
        /// 8-bit Additional Error Code from the target in the lower 8 bits and a 0 in the upper 8 bits.
        /// </summary>
        private RoutingErrorValues additionalStatus;

        /// <summary>
        /// This field is only present with routing type errors and indicates the number of words in the original
        /// route path (Route_Path parameter of Unconnected Send Request) as seen by the router that detects the
        /// error.
        /// </summary>
        private byte remainingPathSize;

        /// <summary>The response data, if any.</summary>
        private MessageBase? responseData;

        /// <summary>Initializes a new instance of the <see cref="UnconnectedSendResponse"/> class using a byte buffer.</summary>
        /// <param name="responseData">The response data. This data will be deserialized into this object instance.</param>
        /// <param name="startingOffset">The offset where the valid response data begins.</param>
        /// <param name="validDataLength">Length of the valid data to read.</param>
        public UnconnectedSendResponse(byte[] responseData, int startingOffset, int validDataLength)
        {
            this.Deserialize(responseData, startingOffset, validDataLength);
        }

        /// <summary>Gets the size, in bytes, of the <see cref="UnconnectedSendResponse"/>.</summary>
        /// <value>The size of the data.</value>
        public override ushort DataSize { get; }

        /// <summary>
        /// Gets the reply service code.
        /// </summary>
        public CipCommonServiceCode ReplyService { get => this.replyService; }

        /// <summary>
        /// Gets the general status.
        /// The value is 0 for successful transactions, otherwise it will be one of the other <see cref="CipGeneralStatusCode"/>s.
        /// If a routing error occurred, it shall be limited to the values specified in <see cref="RoutingErrorValues"/>.
        /// </summary>
        public CipGeneralStatusCode GeneralStatus { get => this.generalStatus; }

        /// <summary>
        /// Gets the size of the additional status.
        /// When returning an error from a target which is a DeviceNet node, the Additional Status shall contain the
        /// 8-bit Additional Error Code from the target in the lower 8 bits and a 0 in the upper 8 bits.
        /// </summary>
        public byte SizeOfAdditionalStatus { get => this.sizeOfAdditionalStatus; }

        /// <summary>
        /// Gets the additional status.
        /// This field is only present with routing type errors and indicates the number of words in the original
        /// route path (Route_Path parameter of Unconnected Send Request) as seen by the router that detects the
        /// error.
        /// </summary>
        public RoutingErrorValues AdditionalStatus { get => this.additionalStatus; }

        /// <summary>
        /// Gets the size of the remaining path.
        /// This field is only present with routing type errors and indicates the number of words in the original
        /// route path (Route_Path parameter of Unconnected Send Request) as seen by the router that detects the
        /// error.
        /// </summary>
        /// <value>The size of the remaining path.</value>
        public byte RemainingPathSize { get => this.remainingPathSize; }

        /// <summary>Deserializes the specified buffer.</summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="startingOffset">The starting offset.</param>
        /// <param name="length">The length of valid data in the buffer.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided buffer is null.</exception>
        /// <exception cref="System.FormatException">Thrown when the length of the data is not valid.</exception>
        public override void Deserialize(byte[] buffer, int startingOffset, int length)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            if (buffer.Length == 0)
            {
                throw new FormatException(Properties.Resources.InvalidDataLengthFormatException);
            }

            int offset = startingOffset;

            // These two fields will always be present for any message router response.
            MessageBase.Deserialize(ref this.replyService, buffer, ref offset);
            
            // Account for the reserved byte.
            offset++;

            MessageBase.Deserialize(ref this.generalStatus, buffer, ref offset);

            // Check the error code to determine if this response is a successful or
            // unsuccessful response.
            if (this.GeneralStatus != CipGeneralStatusCode.Success)
            {
                // This means the request was unsuccessful.
                this.sizeOfAdditionalStatus = buffer[offset];
                offset++;

                MessageBase.Deserialize(ref this.additionalStatus, buffer, ref offset);

                this.remainingPathSize = buffer[offset];
                offset++;
            }
            else
            {
                // This means the request was successful.
                // There will be another reserved byte we need to account for.
                offset++;

                // TODO: Create message object based on the reply code. (this.replyService - ReplyCode)
                switch (this.replyService - ReplyCode) 
                { 
                    default:
                        this.responseData = null;
                        break; 
                }
            }
        }

        /// <summary>Serializes this instance.</summary>
        /// <returns>A byte array containing the serialized data of this message.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when this method is called from an object that doesn't support serialization.</exception>
        public override byte[] Serialize()
        {
            // For now, throw an invalid operation exception.
            // Serialization isn't supported by this object and should not be called.
            throw new InvalidOperationException(Properties.Resources.SerializationNotSupportedInvalidOperationException);
        }
    }
}
