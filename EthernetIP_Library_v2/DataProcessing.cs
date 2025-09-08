//	<copyright file="DataProcessing.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for DataProcessing.
//	</summary>

namespace EthernetIP_Library_v2
{
    using System.Reflection;

    /// <summary>
    /// Class for serializing, deserializing, and processing the encapsulation packet data.
    /// </summary>
    public class DataProcessing
    {
        /// <summary>
        /// The maximum size for our encapsulation packet.
        /// </summary>
        private const int PacketSize = (sizeof(ushort) * 2) + (sizeof(int) * 2) + sizeof(long) + sizeof(uint) + (sizeof(ushort) * 2);

        /// <summary>
        /// Serialize the EncapsulationPacket into a byte array.
        /// </summary>
        /// <returns>A byte array.</returns>
        public static byte[] SerializePacket(EncapsulationPacket packet)
        {
            ArgumentNullException.ThrowIfNull(packet);

            // Create the serialized packet data buffer.
            byte[] serializedPacket = new byte[PacketSize];
            int offset = 0;

            // Need the type and field information for this object so we can automate the serialization process.
            Type type = packet.GetType();
            FieldInfo[] fields = type.GetFields();

            // For every field that we have in EncapsulationPacket, serialize it into the buffer.
            for (int i = 0; i < fields.Length; i++)
            {
                CopyFieldToSerializationBuffer(packet, fields[i], serializedPacket, ref offset);
            }

            return serializedPacket;
        }

        /// <summary>
        /// Deserialize a serialized EncapsulationPacket's data into this EncapsulationPacket object.
        /// </summary>
        /// <param name="serializedPacket">A byte array that represents the EncapsulationPacket object.</param>
        public static void DeserializePacket(EncapsulationPacket packet, byte[] serializedPacket, int offset = 0)
        {
            if (packet == null)
            {
                throw new ArgumentNullException($"{nameof(packet)} cannot be null.", nameof(packet));
            }

            if (serializedPacket == null || serializedPacket.Length < PacketSize)
            {
                // This means the serialized data is either corrupted or not of the type EncapsulationPacket.
                throw new InvalidDataException($"The data contained in {nameof(serializedPacket)} is invalid.");
            }

            // Get the fields.
            FieldInfo[] fields = packet.GetType().GetFields();

            // Write the fields according to their types.
            for (int i = 0; i < fields.Length; i++)
            {
                if (String.Equals(fields[i].FieldType.Name, "UInt16", StringComparison.OrdinalIgnoreCase))
                {
                    fields[i].SetValue(packet, BitConverter.ToUInt16(serializedPacket, offset));
                    offset += sizeof(ushort);
                }
                else if (String.Equals(fields[i].FieldType.Name, "UInt32", StringComparison.OrdinalIgnoreCase))
                {
                    fields[i].SetValue(packet, BitConverter.ToUInt32(serializedPacket, offset));
                    offset += sizeof(uint);
                }
                else
                {
                    fields[i].SetValue(packet, BitConverter.ToInt64(serializedPacket, offset));
                    offset += sizeof(long);
                }
            }
        }

        /// <summary>
        /// Copy EncapsulationField data to the provided data buffer.
        /// </summary>
        /// <param name="field">FieldInfo object that represents a current field of the EncapsulationPacket.</param>
        /// <param name="serializationBuffer">The buffer to write into to store the serialized result.</param>
        /// <param name="offset">Current offset.</param>
        private static void CopyFieldToSerializationBuffer(EncapsulationPacket packet, FieldInfo field, byte[] serializationBuffer, ref int offset)
        {
            Type fieldType = field.FieldType;

            // Restrict allowed types to be only ushort, uint, and long. Internally their names are UInt16, UInt32, and Int64 respectively.
            if (!String.Equals(fieldType.Name, typeof(ushort).Name, StringComparison.OrdinalIgnoreCase)
                && !String.Equals(fieldType.Name, typeof(uint).Name, StringComparison.OrdinalIgnoreCase)
                && !String.Equals(fieldType.Name, typeof(long).Name, StringComparison.OrdinalIgnoreCase))
            {

                throw new ArgumentException($"Parameter is of an invalid type. Accepted types are {typeof(ushort).Name}, {typeof(uint).Name}, or {typeof(long).Name}.", nameof(field));
            }

            object fieldObj = field.GetValue(packet)!;

            // This value will dynamically change its type to whatever fieldType is at runtime.
            dynamic fieldValue = Convert.ChangeType(fieldObj, fieldType);
            byte[] propertyBytes = BitConverter.GetBytes(fieldValue);

            Array.Copy(propertyBytes, 0, serializationBuffer, offset, propertyBytes.Length);

            // Check the type of fieldValue and increment the offset accordingly.
            if (fieldValue.GetType() == typeof(ushort))
            {
                offset += sizeof(ushort);
            }
            else if (fieldValue.GetType() == typeof(uint))
            {
                offset += sizeof(uint);
            }
            else
            {
                offset += sizeof(long);
            }
        }
    }
}
