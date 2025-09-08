//	<copyright file="MessageBase.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for MessageBase.
//	</summary>
namespace EthernetIP_Library
{
    /// <summary>
    /// Base class all messages inherit from.
    /// </summary>
    public class MessageBase
    {
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
        /// Serialize a Command enum value into the given buffer and increment offset by the size of the data.
        /// </summary>
        /// <param name="command">A 16-bit enum</param>
        /// <param name="buffer">The destination byte buffer.</param>
        /// <param name="offset">Position in the byte buffer to insert the field value into.</param>
        public void Serialize(Commands command, byte[] buffer, ref int offset)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            byte[] fieldData = BitConverter.GetBytes((ushort)command);
            Array.Copy(fieldData, 0, buffer, offset, fieldData.Length);
            offset += sizeof(ushort);
        }

        /// <summary>
        /// Serialize a StatusCodes enum value into the given buffer and increment offset by the size of the data.
        /// </summary>
        /// <param name="status">A 16-bit enum</param>
        /// <param name="buffer">The destination byte buffer.</param>
        /// <param name="offset">Position in the byte buffer to insert the field value into.</param>
        public void Serialize(StatusCodes status, byte[] buffer, ref int offset)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            byte[] fieldData = BitConverter.GetBytes((ushort)status);
            Array.Copy(fieldData, 0, buffer, offset, fieldData.Length);
            offset += sizeof(ushort);
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

        /// <summary>
        /// Deserialize a value stored in the buffer at the given offset into the provided field, then increment the offset by the size of the data.
        /// </summary>
        /// <param name="field">An unsigned 16-bit integer.</param>
        /// <param name="buffer">The source byte buffer</param>
        /// <param name="offset">Position in the byte buffer to read from.</param>
        /// <returns>A command code of type <see cref="Commands"/>.</returns>
        public Commands Deserialize(Commands field, byte[] buffer, ref int offset)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            field = (Commands)BitConverter.ToUInt16(buffer, offset);
            offset += sizeof(ushort);
            return field;
        }

        /// <summary>
        /// Deserialize a value stored in the buffer at the given offset into the provided field, then increment the offset by the size of the data.
        /// </summary>
        /// <param name="field">An unsigned 32-bit integer.</param>
        /// <param name="buffer">The source byte buffer</param>
        /// <param name="offset">Position in the byte buffer to read from.</param>
        /// <returns>A status code of type <see cref="StatusCodes"/>.</returns>
        public StatusCodes Deserialize(StatusCodes field, byte[] buffer, ref int offset)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            field = (StatusCodes)BitConverter.ToUInt16(buffer, offset);
            offset += sizeof(uint);
            return field;
        }

        /// <summary>
        /// Deserialize a value stored in the buffer at the given offset into the provided field, then increment the offset by the size of the data.
        /// </summary>
        /// <param name="field">An unsigned 16-bit integer.</param>
        /// <param name="buffer">The source byte buffer</param>
        /// <param name="offset">Position in the byte buffer to read from.</param>
        /// <returns>A 16-bit value.</returns>
        public ushort Deserialize(ushort field, byte[] buffer, ref int offset)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            field = BitConverter.ToUInt16(buffer, offset);
            offset += sizeof(ushort);
            return field;
        }

        /// <summary>
        /// Deserialize a value stored in the buffer at the given offset into the provided field, then increment the offset by the size of the data.
        /// </summary>
        /// <param name="field">An unsigned 32-bit integer.</param>
        /// <param name="buffer">The source byte buffer</param>
        /// <param name="offset">Position in the byte buffer to read from.</param>
        /// <returns>A 32-bit value.</returns>
        public uint Deserialize(uint field, byte[] buffer, ref int offset)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            field = BitConverter.ToUInt32(buffer, offset);
            offset += sizeof(uint);
            return field;
        }

        /// <summary>
        /// Deserialize a value stored in the buffer at the given offset into the provided field, then increment the offset by the size of the data.
        /// </summary>
        /// <param name="field">A 64-bit integer.</param>
        /// <param name="buffer">The source byte buffer</param>
        /// <param name="offset">Position in the byte buffer to read from.</param>
        /// <returns>A value of type long.</returns>
        public long Deserialize(long field, byte[] buffer, ref int offset)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            field = BitConverter.ToInt64(buffer, offset);
            offset += sizeof(long);
            return field;
        }

        /// <summary>
        /// Serialize a <see cref="Header"/> object.
        /// </summary>
        /// <param name="header">An encapsulation header.</param>
        /// <returns>A byte array containing the contents of the header.</returns>
        internal byte[] Serialize(Header header)
        {
            byte[] serializedHeader = new byte[Header.HeaderSize];
            int offset = 0;

            this.Serialize(header.Command, serializedHeader, ref offset);
            this.Serialize(header.Length, serializedHeader, ref offset);
            this.Serialize(header.SessionHandle, serializedHeader, ref offset);
            this.Serialize(header.Status, serializedHeader, ref offset);
            this.Serialize(header.SenderContext, serializedHeader, ref offset);
            this.Serialize(header.Options, serializedHeader, ref offset);

            return serializedHeader;
        }

        /// <summary>
        /// Deserialize a <see cref="Header"/> object from the given buffer.
        /// </summary>
        /// <param name="header">An encapsulation header.</param>
        /// <param name="buffer">The source byte buffer.</param>
        /// <returns>The end position of the header segment in the byte buffer.</returns>
        internal int Deserialize(Header header, byte[] buffer)
        {
            int offset = 0;

            header.Command = this.Deserialize(header.Command, buffer, ref offset);
            header.Length = this.Deserialize(header.Length, buffer, ref offset);
            header.SessionHandle = this.Deserialize(header.SessionHandle, buffer, ref offset);
            header.Status = this.Deserialize(header.Status, buffer, ref offset);
            header.SenderContext = this.Deserialize(header.SenderContext, buffer, ref offset);
            header.Options = this.Deserialize(header.Options, buffer, ref offset);

            return offset;
        }
    }
}
