//	<copyright file="ItemIDNumber.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for ItemIdeNumber.
//	</summary>
namespace CIP_EthernetIP_Library.EnumStructures
{
    /// <summary>
    /// Common Packet Format item ID numbers. Used for the Type ID field of the data/address 
    /// item section of the Common Packet Format.
    /// </summary>
    public enum ItemIDNumber : ushort
    {
        /// <summary>
        /// Address. Used for UCMM messages. Indicates that encapsulation routing is NOT needed.
        /// Target is either local (ethernet) or routing info is in a data item.
        /// </summary>
        Null = 0x0000,

        /// <summary>ListIdentity response.</summary>
        ListIdentityResponse = 0x000C,

        /// <summary>Address. Used for connected messages.</summary>
        ConnectedAddressItem = 0x00A1,

        /// <summary>Data. Connected Transport Packet.</summary>
        ConnectedDataItem = 0x00B1,

        /// <summary>Data. Unconnected message.</summary>
        UnconnectedDataItem = 0x00B2,

        /// <summary>ListServices response.</summary>
        ListServicesResponse = 0x0100,

        /// <summary>Data. Socket address information (originator to target).</summary>
        SockAddrInfo_OriginatorToTarget = 0x8000,

        /// <summary>Data. Socket address information (target to originator).</summary>
        SockAddrInfo_TargetToOriginator = 0x8001,

        /// <summary>Sequenced Address item.</summary>
        SequencedAddressItem = 0x8002
    }
}
