//	<copyright file="AddressAndDataItem.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for AddressAndDataItem.
//	</summary>
namespace CIP_EthernetIP_Library
{
    using System.Buffers.Binary;
    using CIP_EthernetIP_Library.EnumStructures;

    /// <summary>
    /// Represents address and data item members of the <see cref="CommonPacketFormat"/> structure.
    /// The <see cref="ItemIDNumber"/> indicates what kind of address/data item this object is.
    /// </summary>
    /// <seealso cref="MessageBase" />
    internal sealed class AddressAndDataItem : MessageBase
    {
        /// <summary>The sin_family value for a SockAddr info item. If this field is sent, it shall be sent in big endian order.</summary>
        private const short AF_INET = 2;

        /// <summary>The type of item encapsulated.</summary>
        private readonly ItemIDNumber typeID;

        /// <summary>The length, in bytes, of the data item to follow. If no data items follow, this will be 0.</summary>
        private ushort length;

        /// <summary>The connection identifier. For a connected address item.</summary>
        private uint connectionIdentifier;

        /// <summary>The sequence number. For a sequenced data item.</summary>
        private uint sequenceNumber;

        /// <summary>The sin family. For a SockAddr info item.</summary>
        private short sinFamily;

        /// <summary>The sin port. For a SockAddr info item.</summary>
        private ushort sinPort;

        /// <summary>The sin address. For a SockAddr info item.</summary>
        private uint sinAddress;

        /// <summary>The sin zero. For a SockAddr info item.</summary>
        private byte[]? sinZero;

        /// <summary>The message router request object. For an unconnected data item.</summary>
        private MessageBase? messageRouterData;

        /// <summary>Gets the type of item encapsulated.</summary>
        /// <value>The type id.</value>
        public ItemIDNumber TypeID { get => typeID; }

        /// <summary>Gets the length, in bytes, of the data item to follow. If no data items follow, this will be 0.</summary>
        /// <value>The length, in bytes, of the data item that follows.</value>
        public ushort Length { get => length; }

        /// <summary>Gets the connection identifier. For a connected address item.</summary>
        /// <value>The connection identifier.</value>
        public uint ConnectionIdentifier { get => connectionIdentifier; }

        /// <summary>Gets the sequence number. For a sequenced data item.</summary>
        /// <value>The sequence number.</value>
        public uint SequenceNumber { get => sequenceNumber; }

        /// <summary>Gets the sin family. For a SockAddr info item.</summary>
        /// <value>The sin family.</value>
        public short SinFamily { get => sinFamily; }

        /// <summary>Gets the sin port. For a SockAddr info item.</summary>
        /// <value>The sin port.</value>
        public ushort SinPort { get => sinPort; }

        /// <summary>Gets the sin address. For a SockAddr info item.</summary>
        /// <value>The sin address.</value>
        public uint SinAddress { get => sinAddress; }

        /// <summary>Gets the sin zero. For a SockAddr info item.</summary>
        /// <value>The sin zero.</value>
        public byte[]? SinZero { get => sinZero; }

        /// <summary>Gets the message router request object. For an unconnected data item.</summary>
        /// <value>The message router request.</value>
        public MessageBase? MessageRouterData { get => messageRouterData; }

        /// <summary>Gets the size, in bytes, of the <see cref="AddressAndDataItem"/>.</summary>
        /// <value>The size of the data.</value>
        public override ushort DataSize { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressAndDataItem"/> class.
        /// </summary>
        /// <param name="typeID"></param>
        public AddressAndDataItem(ItemIDNumber typeID)
        {
            this.typeID = typeID;

            // We can only have 6 kinds of items.
            // Handle connected and unconnected data items in other constructor.
            switch (TypeID)
            {
                case ItemIDNumber.ConnectedAddressItem:
                    this.length = 4;
                    this.connectionIdentifier = 0;
                    this.DataSize = sizeof(ushort) + sizeof(ushort) + sizeof(uint);
                    break;
                case ItemIDNumber.SequencedAddressItem:
                    this.length = 8;
                    this.sequenceNumber = 0;
                    this.DataSize = sizeof(ushort) + sizeof(ushort) + sizeof(uint);
                    break;
                case ItemIDNumber.SockAddrInfo_OriginatorToTarget:
                    InitializeSockAddrInfoItem();
                    this.DataSize = (ushort)(sizeof(ushort) + sizeof(ushort) + sizeof(ushort) + sizeof(uint) + this.SinZero!.Length);
                    break;
                case ItemIDNumber.SockAddrInfo_TargetToOriginator:
                    InitializeSockAddrInfoItem();
                    this.DataSize = (ushort)(sizeof(ushort) + sizeof(ushort) + sizeof(ushort) + sizeof(uint) + this.SinZero!.Length);
                    break;
                default:
                    this.length = 0;
                    this.DataSize = sizeof(ushort) + sizeof(ushort);
                    break;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="AddressAndDataItem"/> class, currently used for an Unconnected Data item.</summary>
        /// <param name="typeID">The type identifier.</param>
        /// <param name="paddedEpath">The padded epath.</param>
        /// <param name="requestData">The request data to be sent to the target device. <see cref="Nullable"/>.</param>
        public AddressAndDataItem(ItemIDNumber typeID, byte[] routePath, byte[] paddedEpath, MessageBase? requestData = null)
        {
            this.typeID = typeID;

            if (this.TypeID == ItemIDNumber.UnconnectedDataItem)
            {
                this.messageRouterData = new MessageRouterRequest(paddedEpath, new UnconnectedSendRequest(routePath, paddedEpath, requestData));
                this.length = messageRouterData.DataSize;
            }
            /*
            else if (this.TypeId == ItemIDNumber.ConnectedDataItem)
            {
                // TODO: Determine what type of object messageRouterData will be based on conditions.
            }
             */

            this.DataSize = this.messageRouterData != null ? (ushort)(sizeof(ushort) + sizeof(ushort) + this.messageRouterData.DataSize) : (ushort)(sizeof(ushort) + sizeof(ushort));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressAndDataItem"/> class. This is for response data from the server. Currently used for
        /// an Unconnected Data item.
        /// </summary>
        /// <param name="typeID">The type identifier.</param>
        /// <param name="responseData">The response data.</param>
        /// <param name="startingOffset">The starting offset.</param>
        /// <param name="validDataLength">Length of the valid data.</param>
        /// <exception cref="System.ArgumentNullException">responseData</exception>
        public AddressAndDataItem(ItemIDNumber typeID, byte[] responseData, int startingOffset, int validDataLength)
        {
            ArgumentNullException.ThrowIfNull(responseData, nameof(responseData));
            this.typeID = typeID;

            if (this.TypeID == ItemIDNumber.UnconnectedDataItem)
            {
                this.messageRouterData = new MessageRouterResponse(responseData, startingOffset, validDataLength);
                this.length = messageRouterData.DataSize;
            }
            /*
            else if (this.TypeId == ItemIDNumber.ConnectedDataItem)
            {
                // TODO: Determine what type of object messageRouterData will be based on conditions.
            }
             */

            this.DataSize = this.messageRouterData != null ? (ushort)(sizeof(ushort) + sizeof(ushort) + this.messageRouterData.DataSize) : (ushort)(sizeof(ushort) + sizeof(ushort));
        }

        /// <summary>
        /// Deserializes the specified buffer. Not supported by this object.
        /// </summary>
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
        /// <exception cref="NotImplementedException">Thrown when attempting to serialize this object when it's type id is <see cref="ItemIDNumber.ConnectedAddressItem"/>.</exception>
        public override byte[] Serialize()
        {
            byte[] serializedData = new byte[this.DataSize];
            int offset = 0;

            // Serialize based on the type id of the item.
            // Type ID and Length will always be serialized.
            MessageBase.Serialize(this.TypeID, serializedData, ref offset);
            MessageBase.Serialize(this.Length, serializedData, ref offset);

            if (this.TypeID != ItemIDNumber.Null)
            {
                switch (this.TypeID)
                {
                    case ItemIDNumber.ConnectedAddressItem:
                        MessageBase.Serialize(this.ConnectionIdentifier, serializedData, ref offset);
                        break;
                    case ItemIDNumber.SequencedAddressItem:
                        MessageBase.Serialize(this.ConnectionIdentifier, serializedData, ref offset);
                        break;
                    case ItemIDNumber.UnconnectedDataItem:
                        Array.Copy(MessageBase.Serialize(this.messageRouterData!), serializedData, this.messageRouterData!.DataSize);
                        offset += this.messageRouterData!.DataSize;
                        break;
                    case ItemIDNumber.ConnectedDataItem:
                        // Serialize the transport packet here. Not handling yet since transport packet hasn't been created yet.
                        throw new NotImplementedException();
                    case ItemIDNumber.SockAddrInfo_OriginatorToTarget:
                        SerializeSockAddrInfoItem(serializedData, ref offset);
                        break;
                    case ItemIDNumber.SockAddrInfo_TargetToOriginator:
                        SerializeSockAddrInfoItem(serializedData, ref offset);
                        break;
                }
            }

            return serializedData;
        }

        /// <summary>Serializes the SockAddr information item.</summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        private void SerializeSockAddrInfoItem(byte[] buffer, ref int offset)
        {
            MessageBase.Serialize(this.SinFamily, buffer, ref offset);
            MessageBase.Serialize(this.SinPort, buffer, ref offset);
            MessageBase.Serialize(this.sinAddress, buffer, ref offset);
            MessageBase.Serialize(this.SinAddress, buffer, ref offset);
        }

        private void InitializeSockAddrInfoItem()
        {
            this.length = 16;
            // Needs to be in big endian so we call ReverseEndianness.
            this.sinFamily = BinaryPrimitives.ReverseEndianness(AF_INET);

            // Not implementing this yet for the sake of simplifying things.
            this.sinPort = 0;
            this.sinAddress = 0;
            this.sinZero = new byte[8];
        }
    }
}
