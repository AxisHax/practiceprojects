//	<copyright file="EncapsulationPacket.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for EncapsulationPacket.
//	</summary>
namespace EthernetIP_Library_v2
{
    using System.Reflection;

    /// <summary>
    /// Encapsulation Packet class.
    /// </summary>
    public class EncapsulationPacket
    {
        /// <summary>
        /// Encapsulation Command. Must be a valid command code.
        /// </summary>
        public ushort Command;

        /// <summary>
        /// Length, in bytes, of the data portion of the message, i.e, the number of bytes following the header.
        /// </summary>
        public ushort Length;

        /// <summary>
        /// Session identification (application dependent).
        /// </summary>
        public uint SessionHandle;

        /// <summary>
        /// Status code.
        /// </summary>
        public uint Status;

        /// <summary>
        /// Information pertinent only to the sender of an encapsulation command. Length of 8.
        /// </summary>
        public long SenderContext;

        /// <summary>
        /// Options flags.
        /// </summary>
        public uint Options;

        /// <summary>
        /// Requested protocol version.
        /// </summary>
        public ushort ProtocolVersion;

        /// <summary>
        /// Session options.
        /// </summary>
        public ushort OptionsFlags;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="senderContext">The sender context.</param>
        public EncapsulationPacket (long senderContext)
        {
            // Set default values.
            this.Command = 0x0000;
            this.Length = 0;
            this.SessionHandle = 0;
            this.Status = 0;
            this.SenderContext = senderContext;
            this.Options = 0;
            this.ProtocolVersion = 0;
            this.OptionsFlags = 0;
        }
    }
}
