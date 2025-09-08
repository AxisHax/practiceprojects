//	<copyright file="RoutingErrorValues.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for RoutingErrorValues.
//	</summary>
namespace CIP_EthernetIP_Library.EnumStructures
{
    /// <summary>
    /// Standard routing error table for Additional Status fields.
    /// </summary>
    public enum RoutingErrorValues : ushort
    {
        /// <summary>
        /// Timeout indicator. Returned when there's a failure to establish an Explicit Messaging Connection.
        /// Timeout event occurs when waiting for an Explicit Messaging Response. After decreasing the timing parameters
        /// when an Unconnected Send request is received, the CIP Router determines that there is not enough time left
        /// to continue this transaction (a Requesting Device timeout is imminent).
        /// </summary>
        TimeoutIndicator = 0x0204,

        /// <summary>Invalid Port ID specified in the Route_Path field.</summary>
        InvalidPortID = 0x0311,

        /// <summary>Invalid Node Address specified in the Route_Path field.</summary>
        InvalidNodeAddress = 0x0312,

        /// <summary>Invalid segment type in the Route_Path field.</summary>
        InvalidSegmentType = 0x0315
    }
}
