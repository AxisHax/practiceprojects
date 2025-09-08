//	<copyright file="SendRRDataPacket.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for SendRRDataPacket.
//	</summary>

namespace CIP_EthernetIP_Library
{
    using CIP_EthernetIP_Library.EnumStructures;

    /// <summary>
    /// The data packet for <see cref="SendRRData"/>.
    /// </summary>
    /// <seealso cref="CIP_EthernetIP_Library.CommonPacketFormat" />
    internal sealed class SendRRDataPacket : CommonPacketFormat
    {
        /// <summary>The item count, i.e. the number of <see cref="AddressAndDataItem" />s contained within this object.</summary>
        private readonly ushort itemCount;

        /// <summary>The <see cref="AddressAndDataItem" /> list which contains addressing information for the encapsulated packet and its data (if any).</summary>
        private readonly List<AddressAndDataItem> itemList = [];

        /// <summary>The the size, in bytes, of the <see cref="SendRRDataPacket"/>.</summary>
        private readonly ushort dataSize;

        /// <summary>Initializes a new instance of the <see cref="SendRRDataPacket"/> class.</summary>
        /// <param name="routePath">The route path.</param>
        /// <param name="paddedEpath">The padded epath.</param>
        /// <param name="isResponse">Set to true if this is a reply from the server, or false if this is a request.</param>
        public SendRRDataPacket(byte[] routePath, byte[] paddedEpath)
        {
            // For now we're only handling the case of sending an unconnected message for now so designing is easier.
            // Will handle other packet content types after having a solid base to work off of.
            // TODO: handle other message content types.

            // This object will contain request data.
            this.itemList.Add(new AddressAndDataItem(ItemIDNumber.Null));

            // Instantiate a null address item and unconnected send data item with route path, padded EPATH and request Data.
            this.itemList.Add(new AddressAndDataItem(ItemIDNumber.UnconnectedDataItem, routePath, paddedEpath, null));

            this.itemCount = (ushort)this.itemList.Count;
            this.dataSize = sizeof(ushort);

            foreach(AddressAndDataItem item in this.itemList)
            {
                this.dataSize += item.DataSize;
            }
        }

        public SendRRDataPacket(byte[] responseData, int startingOffset, int validDataLength)
        {
            itemList.Add(new AddressAndDataItem(ItemIDNumber.Null));
            itemList.Add(new AddressAndDataItem(ItemIDNumber.UnconnectedDataItem, responseData, startingOffset, validDataLength));

            dataSize = sizeof(ushort);

            foreach (AddressAndDataItem item in this.itemList)
            {
                this.dataSize += item.DataSize;
            }
        }

        /// <summary>
        /// Gets the item count, i.e. the number of <see cref="AddressAndDataItem" />s contained within this object.
        /// </summary>
        /// <value>The item count.</value>
        public override ushort ItemCount => itemCount;

        /// <summary>
        /// Gets the <see cref="AddressAndDataItem" /> list which contains addressing information for the encapsulated packet and its data (if any).
        /// </summary>
        /// <value>The <see cref="AddressAndDataItem" /> list.</value>
        public override List<AddressAndDataItem> ItemList => itemList;

        /// <summary>Gets the size, in bytes, of the <see cref="SendRRDataPacket"/>.</summary>
        /// <value>The size, in bytes, of the data.</value>
        public override ushort DataSize => dataSize;

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

            MessageBase.Serialize(this.itemCount, serializedData, ref offset);
            
            // Serialize each item in the itemList.
            foreach(MessageBase message in itemList)
            {
                Array.Copy(MessageBase.Serialize(message), serializedData, message.DataSize);
                offset += message.DataSize;
            }

            return serializedData;
        }
    }
}
