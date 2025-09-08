//	<copyright file="MessageRouterResponse.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for MessageRouterResponse.
//	</summary>

namespace CIP_EthernetIP_Library
{
    using CIP_EthernetIP_Library.EnumStructures;

    /// <summary>
    /// The standard format for receiving data from the Message Router object.
    /// </summary>
    internal sealed class MessageRouterResponse : MessageBase
    {
        /// <summary>This is the value added to the service code sent. Subtract this from <see cref="replyService"/> to get the actual code.</summary>
        private const byte ReplyCode = 0x80;

        /// <summary>Reply service code.</summary>
        private CipCommonServiceCode replyService;

        /// <summary>One of the general status codes.</summary>
        private CipGeneralStatusCode generalStatus;

        /// <summary>Number of 16-bit words in <see cref="additionalStatus"/> array.</summary>
        private byte sizeOfAdditionalStatus;

        /// <summary>Additional status.</summary>
        private RoutingErrorValues additionalStatus;

        /// <summary>Response data or additional error data if <see cref="generalStatus"/> indicated an error.</summary>
        private MessageBase? responseData;

        /// <summary>Initializes a new instance of the <see cref="MessageRouterResponse"/> class.</summary>
        /// <param name="responseData">A byte array that contains the <see cref="MessageRouterResponse"/> data. </param>
        public MessageRouterResponse(byte[] responseData, int offset, int validDataLength)
        {
            // Simply deserialize the responseData contents into this object to populate fields.
            this.Deserialize(responseData, offset, validDataLength);
            this.DataSize = (this.ResponseData is not null) ? (ushort)(sizeof(byte) + sizeof(byte) + sizeof(byte) + this.SizeOfAdditionalStatus * 2 + this.ResponseData.DataSize) 
                : (ushort)(sizeof(byte) + sizeof(byte) + sizeof(byte) + this.SizeOfAdditionalStatus * 2);
        }

        /// <summary>
        /// The size, in bytes, of the <see cref="MessageRouterRequest"/>.
        /// </summary>
        public override ushort DataSize { get; }

        /// <summary>
        /// Gets or sets the reply service code.
        /// </summary>
        public CipCommonServiceCode ReplyService { get => this.replyService; set => this.replyService = value; }

        /// <summary>
        /// Gets or sets the general status.
        /// </summary>
        public CipGeneralStatusCode GeneralStatus { get => this.generalStatus; set => this.generalStatus = value; }

        /// <summary>
        /// Gets or sets the size of the additional status array.
        /// </summary>
        public byte SizeOfAdditionalStatus { get => this.sizeOfAdditionalStatus; set => this.sizeOfAdditionalStatus = value; }

        /// <summary>
        /// Gets or sets the additional status.
        /// </summary>
        public RoutingErrorValues AdditionalStatus { get => this.additionalStatus; set => this.additionalStatus = value; }

        /// <summary>
        /// Gets or sets the response data.
        /// </summary>
        public MessageBase? ResponseData { get => this.responseData; set => this.responseData = value; }

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

            MessageBase.Deserialize(ref this.replyService, buffer, ref offset);
            
            // Increment offset by 1 to account for the reserved byte.
            offset++;

            MessageBase.Deserialize(ref this.generalStatus, buffer, ref offset);

            this.sizeOfAdditionalStatus = buffer[offset];
            offset++;

            MessageBase.Deserialize(ref this.additionalStatus, buffer, ref offset);

            // Deserialize whatever message object we expect.
            // TODO: Create message object based on the expected response data. (this.replyService)
            switch (this.replyService - ReplyCode)
            {
                default:
                    break;
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
