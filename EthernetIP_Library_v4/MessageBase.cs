//	<copyright file="MessageBase.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for MessageBase.
//	</summary>
namespace EthernetIP_Library_v4
{
    using static EthernetIP_Library_v4.Commands;
    using static EthernetIP_Library_v4.StatusCodes;

    /// <summary>
    /// MessageBase class.
    /// </summary>
    /// <param name="size"></param>
    public class MessageBase(int size)
    {
        /// <summary>
        /// The size, in bytes, of the message.
        /// </summary>
        public int size = size;

        /// <summary>
        /// Serialize the header and command specific data portions of the encapsulation packet and store it into the given buffer.
        /// </summary>
        /// <param name="headerData">The header of the encapsulation packet.</param>
        /// <param name="commandSpecificData">The command specific specific data region of the packet.</param>
        /// <param name="buffer">The destination byte buffer.</param>
        public void Serialize(byte[] headerData, byte[] commandSpecificData, byte[] buffer)
        {
            ArgumentNullException.ThrowIfNull(headerData, nameof(headerData));
            ArgumentNullException.ThrowIfNull(commandSpecificData, nameof(commandSpecificData));
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            byte[] packetData = new byte[headerData.Length + commandSpecificData.Length];
            Array.Copy(headerData, 0, buffer, 0, headerData.Length);
            Array.Copy(commandSpecificData, 0, buffer, headerData.Length, commandSpecificData.Length);
        }

        /// <summary>
        /// Serialize a 32-bit integer field value into the given buffer and increment offset by the size of the data.
        /// </summary>
        /// <param name="field">An unsigned 32-bit integer.</param>
        /// <param name="buffer">The destination byte buffer.</param>
        /// <param name="offset">Position in the byte buffer to insert the field value into.</param>
        public void Serialize(uint field, byte[] buffer, ref int offset)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            byte[] fieldData = BitConverter.GetBytes(field);
            Array.Copy(fieldData, 0, buffer, offset, fieldData.Length);
            offset += sizeof(uint);
        }

        /// <summary>
        /// Serialize a 16-bit integer field value into the given buffer and increment offset by the size of the data.
        /// </summary>
        /// <param name="field">An unsigned 16-bit integer.</param>
        /// <param name="buffer">The destination byte buffer.</param>
        /// <param name="offset">Position in the byte buffer to insert the field value into.</param>
        public void Serialize(ushort field, byte[] buffer, ref int offset)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            byte[] fieldData = BitConverter.GetBytes(field);
            Array.Copy(fieldData, 0, buffer, offset, fieldData.Length);
            offset += sizeof(ushort);
        }

        /// <summary>
        /// Serialize a 64-bit integer field value into the given buffer and increment offset by the size of the data.
        /// </summary>
        /// <param name="field">A 64-bit integer.</param>
        /// <param name="buffer">The destination byte buffer.</param>
        /// <param name="offset">Position in the byte buffer to insert the field value into.</param>
        public void Serialize(long field, byte[] buffer, ref int offset)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            byte[] fieldData = BitConverter.GetBytes(field);
            Array.Copy(fieldData, 0, buffer, offset, fieldData.Length);
            offset += sizeof(long);
        }

        public void Deserialize(byte[] buffer, byte[] encapsulatedData, int offset)
        {
            Array.Copy(buffer, offset, encapsulatedData, 0, encapsulatedData.Length);
        }

        /// <summary>
        /// Deserialize a value stored in the buffer at the given offset into the provided field, then increment the offset by the size of the data.
        /// </summary>
        /// <param name="field">An unsigned 16-bit integer.</param>
        /// <param name="buffer">The source byte buffer</param>
        /// <param name="offset">Position in the byte buffer to read from.</param>
        public void Deserialize(ref Command field, byte[] buffer, ref int offset)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            field = (Command)BitConverter.ToUInt16(buffer, offset);
            offset += sizeof(ushort);
        }

        /// <summary>
        /// Deserialize a value stored in the buffer at the given offset into the provided field, then increment the offset by the size of the data.
        /// </summary>
        /// <param name="field">An unsigned 32-bit integer.</param>
        /// <param name="buffer">The source byte buffer</param>
        /// <param name="offset">Position in the byte buffer to read from.</param>
        public void Deserialize(ref StatusCode field, byte[] buffer, ref int offset)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            field = (StatusCode)BitConverter.ToUInt16(buffer, offset);
            offset += sizeof(uint);
        }

        /// <summary>
        /// Deserialize a value stored in the buffer at the given offset into the provided field, then increment the offset by the size of the data.
        /// </summary>
        /// <param name="field">An unsigned 16-bit integer.</param>
        /// <param name="buffer">The source byte buffer</param>
        /// <param name="offset">Position in the byte buffer to read from.</param>
        public void Deserialize(ref ushort field, byte[] buffer, ref int offset)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            field = BitConverter.ToUInt16(buffer, offset);
            offset += sizeof(ushort);
        }

        /// <summary>
        /// Deserialize a value stored in the buffer at the given offset into the provided field, then increment the offset by the size of the data.
        /// </summary>
        /// <param name="field">An unsigned 32-bit integer.</param>
        /// <param name="buffer">The source byte buffer</param>
        /// <param name="offset">Position in the byte buffer to read from.</param>
        public void Deserialize(ref uint field, byte[] buffer, ref int offset)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            field = BitConverter.ToUInt32(buffer, offset);
            offset += sizeof(uint);
        }

        /// <summary>
        /// Deserialize a value stored in the buffer at the given offset into the provided field, then increment the offset by the size of the data.
        /// </summary>
        /// <param name="field">A 64-bit integer.</param>
        /// <param name="buffer">The source byte buffer</param>
        /// <param name="offset">Position in the byte buffer to read from.</param>
        public void Deserialize(ref long field, byte[] buffer, ref int offset)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            field = BitConverter.ToInt64(buffer, offset);
            offset += sizeof(long);
        }
    }
}
