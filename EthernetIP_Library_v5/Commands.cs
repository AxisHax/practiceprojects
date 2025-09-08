//	<copyright file="Commands.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for Commands.
//	</summary>
namespace EthernetIP_Library
{
    /// <summary>
    /// List of encapsulation commands.
    /// </summary>
    public enum Commands : ushort
    {
        /// <summary>
        /// May be sent only using TCP.
        /// </summary>
        NOP = 0x0000,

        /// <summary>
        /// May be sent using either UDP or TCP.
        /// </summary>
        ListServices = 0x0004,

        /// <summary>
        /// May be sent using either UDP or TCP.
        /// </summary>
        ListIdentity = 0x0063,

        /// <summary>
        /// OPTIONAL. May be sent using either UDP or TCP.
        /// </summary>
        ListInterfaces = 0x0064,

        /// <summary>
        /// May be sent only using TCP.
        /// </summary>
        RegisterSession = 0x0065,

        /// <summary>
        /// May be sent only using TCP.
        /// </summary>
        UnRegisterSession = 0x0066,

        /// <summary>
        /// May be sent only using TCP.
        /// </summary>
        SendRRData = 0x006F,

        /// <summary>
        /// May be sent only using TCP.
        /// </summary>
        SendUnitData = 0x0070,

        /// <summary>
        /// OPTIONAL. May be sent only using TCP.
        /// </summary>
        IndicateStatus = 0x0072,

        /// <summary>
        /// OPTIONAL. May be sent only using TCP.
        /// </summary>
        Cancel = 0x0073
    }
}
