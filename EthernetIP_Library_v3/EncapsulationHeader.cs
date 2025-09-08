//	<copyright file="EncapsulationHeader.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for EncapsulationHeader.
//	</summary>
namespace EthernetIP_Library_v3
{
    /// <summary>
    /// Encapsulation header and it's associated methods.
    /// </summary>
    public class EncapsulationHeader : Header
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EncapsulationHeader"/> class.
        /// </summary>
        public EncapsulationHeader() : base()
        {
        }

        /// <summary>
        /// Serialize the encapsulation header fields.
        /// </summary>
        /// <returns>A byte array containing the encapsulation header data.</returns>
        public byte[] GetSerializedHeader()
        {
            List<object> fields = this.GetHeaderFields();

            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    foreach (var field in fields)
                    {
                        if (field.GetType() == typeof(ushort))
                        {
                            writer.Write((ushort)field);
                        }
                        else if (field.GetType() == typeof(uint))
                        {
                            writer.Write((uint)field);
                        }
                        else
                        {
                            writer.Write((long)field);
                        }
                    }
                }

                return m.ToArray();
            }
        }

        /// <summary>
        /// Deserialize and save the header data stored in the buffer.
        /// </summary>
        /// <param name="dataBuffer">Data buffer we want to read from.</param>
        /// <returns>Offset where the command specific data begins.</returns>
        public int DeserializeHeaderData(byte[] dataBuffer)
        {
            // If the buffer is null throw an exception.
            ArgumentNullException.ThrowIfNull(dataBuffer, nameof(dataBuffer));

            List<object> fields = this.GetHeaderFields();

            // We can't do anything if there's nothing in the buffer.
            if (dataBuffer.Length == 0)
            {
                Console.WriteLine($"The data buffer has no elements.");
                return -1;
            }

            int offset = 0;

            for (int i = 0; i < fields.Count; i++)
            {
                if (fields[i].GetType() == typeof(ushort))
                {
                    fields[i] = BitConverter.ToUInt16(dataBuffer, offset);
                    offset += sizeof(ushort);
                }
                else if (fields[i].GetType() == typeof(uint))
                {
                    fields[i] = BitConverter.ToUInt32(dataBuffer, offset);
                    offset += sizeof(uint);
                }
                else
                {
                    fields[i] = BitConverter.ToInt64(dataBuffer, offset);
                    offset += sizeof(long);
                }
            }

            this.UpdateHeaderFields(fields);

            return offset;
        }
    }
}
