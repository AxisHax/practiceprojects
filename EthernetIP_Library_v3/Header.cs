//	<copyright file="Header.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for Header.
//	</summary>
namespace EthernetIP_Library_v3
{
    /// <summary>
    /// Base header class.
    /// </summary>
    public class Header
    {
        /// <summary>
        /// Size of the header in bytes.
        /// </summary>
        public const int HeaderSize = 24;

        /// <summary>
        /// Encapsulation command.
        /// </summary>
        public ushort Command;

        /// <summary>
        /// Length, in bytes, of the data portion of the message, i.e., the number of bytes following the header.
        /// </summary>
        public ushort Length;

        /// <summary>
        /// Session identification.
        /// </summary>
        public uint SessionHandle;

        /// <summary>
        /// Status code.
        /// </summary>
        public uint Status;

        /// <summary>
        /// Information pertinent only to the sender of an encapsulation command.
        /// </summary>
        public long SenderContext;

        /// <summary>
        /// Options flags.
        /// </summary>
        public uint Options;

        /// <summary>
        /// List of all fields in order.
        /// </summary>
        private List<object>? fields;

        /// <summary>
        /// Initializes a new instance of the <see cref="Header"/> class.
        /// </summary>
        public Header()
        {
            this.Command = 0x0000;
            this.Length = 0;
            this.SessionHandle = 0;
            this.Status = 0;
            this.SenderContext = 0;
            this.Options = 0;
        }

        /// <summary>
        /// Get all header field values.
        /// </summary>
        /// <returns>An object list containing all field members.</returns>
        public List<object> GetHeaderFields()
        {
            this.fields = new List<object>()
            {
                this.Command,
                this.Length,
                this.SessionHandle,
                this.Status,
                this.SenderContext,
                this.Options
            };

            return this.fields;
        }

        /// <summary>
        /// Update the header fields.
        /// </summary>
        /// <param name="updatedFields">A list of field values. Each element must be in the correct order and of the correct type.</param>
        public void UpdateHeaderFields(List<object> updatedFields)
        {
            // Throw exception if the list is null.
            ArgumentNullException.ThrowIfNull(updatedFields, nameof(updatedFields));

            // Update our fields.
            this.Command = (ushort)updatedFields[0];
            this.Length = (ushort)updatedFields[1];
            this.SessionHandle = (uint)updatedFields[2];
            this.Status = (uint)updatedFields[3];
            this.SenderContext = (long)updatedFields[4];
            this.Options = (uint)updatedFields[5];

            // Finally, update our list.
            this.fields = updatedFields;
        }
    }
}
