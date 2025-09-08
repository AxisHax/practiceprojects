//	<copyright file="UnconnectedSendRequest.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for UnconnectedSendRequest.
//	</summary>
namespace CIP_EthernetIP_Library
{
    /// <summary>
    /// Unconnected Send Request object. Allows an application to send a message to a device without
    /// first setting up a connection.
    /// </summary>
    /// <seealso cref="CIP_EthernetIP_Library.MessageBase" />
    internal sealed class UnconnectedSendRequest : MessageBase
    {
        /// <summary>The reserved byte. Always 0.</summary>
        private const byte Reserved = 0;

        /// <summary>Used to calculate request timeout information.</summary>
        private sbyte priority;

        /// <summary>Used to calculate request timeout information.</summary>
        private byte timeout;

        /// <summary>Specifies the number of bytes in the <see cref="MessageRouterRequest"/>.</summary>
        private ushort messageRequestSize;

        /// <summary>The number of 16-bit words in <see cref="RoutePath"/>.</summary>
        private byte routePathSize;

        /// <summary>The message router request.</summary>
        private MessageRouterRequest messageRequest;

        /// <summary>Initializes a new instance of the <see cref="UnconnectedSendRequest"/> class.</summary>
        /// <param name="routePath">The route path.</param>
        /// <param name="requestPath">The request path.</param>
        /// <param name="requestData">The request data to be sent to the target device. <see cref="Nullable"/>.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="routePath"/> or <paramref name="requestPath"/> are null. </exception>
        /// <exception cref="System.FormatException">Thrown when <paramref name="routePath"/> is of an invalid length.</exception>
        public UnconnectedSendRequest(byte[] routePath, byte[] requestPath, MessageBase? requestData = null)
        {
            ArgumentNullException.ThrowIfNull(routePath, nameof(routePath));
            ArgumentNullException.ThrowIfNull(requestPath, nameof(requestPath));

            if (routePath.Length <= Byte.MaxValue)
            {
                this.routePathSize = (byte)routePath.Length;
            }
            else
            {
                throw new FormatException(Properties.Resources.InvalidDataLengthFormatException);
            }

            this.RoutePath = routePath;

            // Message to target device?
            this.messageRequest = new MessageRouterRequest(requestPath, requestData);
            this.messageRequestSize = this.messageRequest.DataSize;

            this.DataSize = (ushort)(sizeof(sbyte) + sizeof(byte) + sizeof(ushort) + this.messageRequest.DataSize + sizeof(byte) + this.RoutePath.Length);
        }

        /// <summary>Gets the length, in bytes, of the <see cref="UnconnectedSendRequest"/>.</summary>
        /// <value>The length, in bytes, of this instance.</value>
        public override ushort DataSize { get ; }

        /// <summary>Gets or sets the priority.</summary>
        /// <value>The priority.</value>
        public sbyte Priority { get => this.priority; set => this.priority = value; }

        /// <summary>Gets or sets the timeout.</summary>
        /// <value>The timeout.</value>
        public byte Timeout { get => this.timeout; set => this.timeout = value; }

        /// <summary>Gets or sets the size of the message request.</summary>
        /// <value>The size of the message request.</value>
        public ushort MessageRequestSize { get => this.messageRequestSize; set => this.messageRequestSize = value; }

        /// <summary>Gets or sets the message request.</summary>
        /// <value>The <see cref="MessageRouterRequest"/> object of this instance.</value>
        public MessageRouterRequest MessageRequest { get => messageRequest; }

        /// <summary>Only present if <see cref="MessageRequestSize"/> is an odd value.</summary>
        /// <value>The pad.</value>
        public static byte Pad => 0;

        /// <summary>Gets or sets the size of the route path.</summary>
        /// <value>The size of the route path.</value>
        public byte RoutePathSize { get => this.routePathSize; }

        /// <summary>Gets or sets the route path.</summary>
        /// <value>The route path.</value>
        public byte[] RoutePath { get; set; }

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
            // Size depends on if the message request is an even number of bytes or not.
            byte[] serializedData = new byte[(this.messageRequest.DataSize % 2 != 0) ? this.DataSize : this.DataSize + 1];
            int offset = 0;

            MessageBase.Serialize(this.priority, serializedData, ref offset);
            MessageBase.Serialize(this.timeout, serializedData, ref offset);
            MessageBase.Serialize(this.messageRequestSize, serializedData, ref offset);

            if (this.MessageRequest is not null)
            {
                Array.Copy(MessageBase.Serialize(this.messageRequest), serializedData, this.MessageRequest.DataSize);
                offset += this.MessageRequest.DataSize;

                // Only include the pad byte if the length of the message router request is odd.
                if (this.MessageRequest.DataSize % 2 != 0)
                {
                    MessageBase.Serialize(Pad, serializedData, ref offset);
                }
            }

            MessageBase.Serialize(this.routePathSize, serializedData, ref offset);
            MessageBase.Serialize(Reserved, serializedData, ref offset);
            Array.Copy(this.RoutePath, serializedData, this.RoutePath.Length);

            return serializedData;
        }
    }
}
