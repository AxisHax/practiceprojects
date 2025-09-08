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
    public abstract class MessageBase
    {
        /// <summary>Gets the size of the data.</summary>
        /// <value>The size of the data.</value>
        public abstract ushort DataSize { get; }

        /// <summary>
        /// Serializes this instance.
        /// </summary>
        /// <returns>A byte array containing the serialized data of this message.</returns>
        public abstract byte[] Serialize();

        /// <summary>Deserializes the specified buffer.</summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="startingOffset">The starting offset.</param>
        /// <param name="length">The length of valid data in the buffer.</param>
        public abstract void Deserialize(byte[] buffer, int startingOffset, int length);

        /// <summary>
        /// Serialize a 32-bit integer field value into the given buffer and increment offset by the size of the data.
        /// </summary>
        /// <param name="value">An unsigned 32-bit integer.</param>
        /// <param name="buffer">The destination byte buffer.</param>
        /// <param name="offset">Position in the byte buffer to insert the field value into.</param>
        protected static void Serialize(uint value, byte[] buffer, ref int offset)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            Array.Copy(BitConverter.GetBytes(value), 0, buffer, offset, sizeof(uint));
            offset += sizeof(uint);
        }

        /// <summary>
        /// Serialize an enum into the given buffer and increment offset by the size of the data
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="value">An enum.</param>
        /// <param name="buffer">The destination byte buffer.</param>
        /// <param name="offset">Position in the byte buffer to insert the field value into.</param>
        protected static void Serialize<T>(T value, byte[] buffer, ref int offset) where T : Enum
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            Type underlyingEnumType = Enum.GetUnderlyingType(value.GetType());

            if (underlyingEnumType == typeof(sbyte))
            {
                Array.Copy(new sbyte[] { (sbyte)(object)value }, 0, buffer, offset, sizeof(sbyte));
                offset += sizeof(sbyte);
            }
            else if (underlyingEnumType == typeof(byte))
            {
                Array.Copy(new byte[] { (byte)(object)value }, 0, buffer, offset, sizeof(byte));
                offset += sizeof(byte);
            }
            else if (underlyingEnumType == typeof(short))
            {
                Array.Copy(BitConverter.GetBytes((short)(object)value), 0, buffer, offset, sizeof(short));
                offset += sizeof(short);
            }
            else if (underlyingEnumType == typeof(ushort))
            {
                Array.Copy(BitConverter.GetBytes((ushort)(object)value), 0, buffer, offset, sizeof(ushort));
                offset += sizeof(ushort);
            }
            else if (underlyingEnumType == typeof(int))
            {
                Array.Copy(BitConverter.GetBytes((int)(object)value), 0, buffer, offset, sizeof(int));
                offset += sizeof(int);
            }
            else if (underlyingEnumType == typeof(uint))
            {
                Array.Copy(BitConverter.GetBytes((uint)(object)value), 0, buffer, offset, sizeof(uint));
                offset += sizeof(uint);
            }
            else if (underlyingEnumType == typeof(long))
            {
                Array.Copy(BitConverter.GetBytes((long)(object)value), 0, buffer, offset, sizeof(long));
                offset += sizeof(long);
            }
            else if (underlyingEnumType == typeof(ulong))
            {
                Array.Copy(BitConverter.GetBytes((ulong)(object)value), 0, buffer, offset, sizeof(ulong));
                offset += sizeof(ulong);
            }
        }

        /// <summary>
        /// Serialize a 16-bit integer field value into the given buffer and increment offset by the size of the data.
        /// </summary>
        /// <param name="value">An unsigned 16-bit integer.</param>
        /// <param name="buffer">The destination byte buffer.</param>
        /// <param name="offset">Position in the byte buffer to insert the field value into.</param>
        protected static void Serialize(ushort value, byte[] buffer, ref int offset)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            Array.Copy(BitConverter.GetBytes(value), 0, buffer, offset, sizeof(ushort));
            offset += sizeof(ushort);
        }

        /// <summary>
        /// Serialize a 64-bit integer field value into the given buffer and increment offset by the size of the data.
        /// </summary>
        /// <param name="value">A 64-bit integer.</param>
        /// <param name="buffer">The destination byte buffer.</param>
        /// <param name="offset"> Position in the byte buffer to read from.</param>
        protected static void Serialize(long value, byte[] buffer, ref int offset)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            Array.Copy(BitConverter.GetBytes(value), 0, buffer, offset, sizeof(long));
            offset += sizeof(long);
        }

        /// <summary>
        /// Deserialize a value stored in the buffer at the given offset into the provided field, then increment the offset by the size of the data.
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="value">An enum.</param>
        /// <param name="buffer">The source byte buffer.</param>
        /// <param name="offset">Position in the byte buffer to read from.</param>
        protected static void Deserialize<T>(ref T value, byte[] buffer, ref int offset) where T : Enum
        {
            Type valueType = Enum.GetUnderlyingType(value.GetType());

            if (valueType == typeof(sbyte))
            {
                value = (T)Enum.ToObject(value.GetType(), buffer[offset]);
                offset += sizeof(sbyte);
            }
            else if (valueType == typeof(byte))
            {
                value = (T)Enum.ToObject(value.GetType(), buffer[offset]);
                offset += sizeof(byte);
            }
            else if (valueType == typeof(short))
            {
                value = (T)Enum.ToObject(value.GetType(), BitConverter.ToUInt16(buffer, offset));
                offset += sizeof(short);
            }
            else if (valueType == typeof(ushort))
            {
                value = (T)Enum.ToObject(value.GetType(), BitConverter.ToUInt16(buffer, offset));
                offset += sizeof(ushort);
            }
            else if (valueType == typeof(int))
            {
                value = (T)Enum.ToObject(value.GetType(), BitConverter.ToInt32(buffer, offset));
                offset += sizeof(int);
            }
            else if (valueType == typeof(uint))
            {
                value = (T)Enum.ToObject(value.GetType(), BitConverter.ToUInt32(buffer, offset));
                offset += sizeof(uint);
            }
            else if (valueType == typeof(long))
            {
                value = (T)Enum.ToObject(value.GetType(), BitConverter.ToInt64(buffer, offset));
                offset += sizeof(long);
            }
            else if (valueType == typeof(ulong))
            {
                value = (T)Enum.ToObject(value.GetType(), BitConverter.ToUInt64(buffer, offset));
                offset += sizeof(ulong);
            }
        }

        /// <summary>
        /// Deserialize a value stored in the buffer at the given offset into the provided field, then increment the offset by the size of the data.
        /// </summary>
        /// <param name="value">An unsigned 16-bit integer.</param>
        /// <param name="buffer">The source byte buffer</param>
        /// <param name="offset">Position in the byte buffer to read from.</param>
        protected static void Deserialize(ref ushort value, byte[] buffer, ref int offset)
        {
            value = BitConverter.ToUInt16(buffer, offset);
            offset += sizeof(ushort);
        }

        /// <summary>
        /// Deserialize a value stored in the buffer at the given offset into the provided field, then increment the offset by the size of the data.
        /// </summary>
        /// <param name="value">An unsigned 32-bit integer.</param>
        /// <param name="buffer">The source byte buffer</param>
        /// <param name="offset">Position in the byte buffer to read from.</param>
        protected static void Deserialize(ref uint value, byte[] buffer, ref int offset)
        {
            value = BitConverter.ToUInt32(buffer, offset);
            offset += sizeof(uint);
        }

        /// <summary>
        /// Deserialize a value stored in the buffer at the given offset into the provided field, then increment the offset by the size of the data.
        /// </summary>
        /// <param name="value">A 64-bit integer.</param>
        /// <param name="buffer">The source byte buffer</param>
        /// <param name="offset">Position in the byte buffer to read from.</param>
        protected static void Deserialize(ref long value, byte[] buffer, ref int offset)
        {
            value = BitConverter.ToInt64(buffer, offset);
            offset += sizeof(long);
        }
    }
}
