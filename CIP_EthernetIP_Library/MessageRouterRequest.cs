//	<copyright file="MessageRouterRequest.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for MessageRouterRequest.
//	</summary>

namespace CIP_EthernetIP_Library
{
    using CIP_EthernetIP_Library.EnumStructures;

    /// <summary>
    /// The standard data format for delivering data to and from the Message Router object.
    /// </summary>
    /// <seealso cref="CIP_EthernetIP_Library.MessageBase" />
    internal sealed class MessageRouterRequest : MessageBase
    {
        /// <summary>
        /// Service code of the request.
        /// </summary>
        private CipCommonServiceCode service;
        
        /// <summary>
        /// The number of 16-bit words in the <see cref="requestPath"/> field (next element).
        /// </summary>
        private byte requestPathSize;
        
        /// <summary>
        /// An array of bytes containing the path of the request (Class ID, Instance ID, etc.) for this transaction.
        /// </summary>
        private byte[] requestPath;
        
        /// <summary>
        /// Service specific data to be delivered in the Explicit Messaging Request.
        /// If no additional data needs to be sent with the Explicit Messaging Request then this
        /// will be empty.
        /// </summary>
        private MessageBase? requestData;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRouterRequest"/> class.
        /// </summary>
        /// <param name="paddedEpath">
        /// This is an array of bytes whose contents convey the path of the request (Class ID, Instance ID, etc.) for this transaction.
        /// </param>
        /// <param name="requestData">
        /// Service specific data to be delivered in the Explicit Messaging Request. If no additional data needs to be sent with the 
        /// Explicit Messaging Request then this array will be empty.
        /// </param>
        /// <exception cref="FormatException">Thrown when the data length is not valid.</exception>
        public MessageRouterRequest(byte[] paddedEpath, MessageBase? requestData = null)
        {
            ArgumentNullException.ThrowIfNull(paddedEpath, nameof(paddedEpath));

            if (paddedEpath.Length == 0)
            {
                throw new FormatException(Properties.Resources.InvalidDataLengthFormatException);
            }

            this.requestPath = paddedEpath;
            this.requestData = requestData;

            int wordsInPath = this.requestPath.Length / 2;
            this.requestPathSize = (byte)wordsInPath;

            this.DataSize = (ushort)(sizeof(CipCommonServiceCode) + sizeof(byte) + this.requestPath.Length + (this.requestData is null ? 0 : this.requestData.DataSize));
        }

        /// <summary>
        /// The size, in bytes, of the <see cref="MessageRouterRequest"/>.
        /// </summary>
        public override ushort DataSize { get; }

        /// <summary>
        /// Gets or sets the service code.
        /// </summary>
        public CipCommonServiceCode Service { get => this.service; set => this.service = value; }

        /// <summary>
        /// Gets or sets the request path size.
        /// </summary>
        public byte RequestPathSize { get => this.requestPathSize; set => this.requestPathSize = value; }

        /// <summary>
        /// Gets or sets the request path.
        /// </summary>
        public byte[] RequestPath { get => this.requestPath; set => this.requestPath = value; }
                                                                    
        /// <summary>                                               
        /// Gets or sets the request data.                          
        /// </summary>                                              
        public MessageBase? RequestData { get => this.requestData; set => this.requestData = value; }

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

            MessageBase.Serialize(this.service, serializedData, ref offset);
            MessageBase.Serialize(this.requestPathSize, serializedData, ref offset);

            Array.Copy(this.requestPath, serializedData, this.requestPath.Length);
            offset += this.requestPath.Length;
            
            if (this.requestData is not null)
            {
                Array.Copy(MessageBase.Serialize(this.requestData), serializedData, this.requestData.DataSize);
                offset += this.requestData.DataSize;
            }

            return serializedData;
        }
    }
}
