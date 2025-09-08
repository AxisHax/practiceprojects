//	<copyright file="CommonPacketFormat.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for CommonPacketFormat.
//	</summary>
namespace CIP_EthernetIP_Library
{
    /// <summary>
    /// The base class for CIP packets. Defines the standard format for protocol packets that are transported with the encapsulation protocol.
    /// </summary>
    /// <seealso cref="CIP_EthernetIP_Library.MessageBase" />
    internal abstract class CommonPacketFormat : MessageBase
    {
        /// <summary>Gets the item count, i.e. the number of <see cref="AddressAndDataItem"/>'s contained within this object.</summary>
        /// <value>The item count.</value>
        public abstract ushort ItemCount { get; }

        /// <summary>Gets the <see cref="AddressAndDataItem"/> list which contains addressing information for the encapsulated packet and its data (if any).</summary>
        /// <value>The <see cref="AddressAndDataItem"/> list.</value>
        public abstract List<AddressAndDataItem> ItemList { get; }
    }
}
