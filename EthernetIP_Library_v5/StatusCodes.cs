//	<copyright file="StatusCodes.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for StatusCodes.
//	</summary>
namespace EthernetIP_Library
{
    /// <summary>
    /// Error codes.
    /// </summary>
    public enum StatusCodes : uint
    {
        /// <summary>
        /// Success code.
        /// </summary>
        Success = 0x0000,

        /// <summary>
        /// The sender issued an invalid or unsupported encapsulation command.
        /// </summary>
        InvalidCommand = 0x0001,

        /// <summary>
        /// Insufficient memory resources in the receiver to handle the command. This is not an application error. 
        /// Instead, it only results if the encapsulation layer cannot obtain memory resources that it needs. 
        /// </summary>
        InsufficientMemory = 0x0002,

        /// <summary>
        /// Poorly formed or incorrect data in the data portion of the encapsulation message.
        /// </summary>
        PoorlyFormedOrIncorrectEncapsulationData = 0x0003,

        /// <summary>
        /// An originator used an invalid session handle when sending an encapsulation message to the target.
        /// </summary>
        InvalidSessionHandle = 0x0064,

        /// <summary>
        /// The target received a message of invalid length.
        /// </summary>
        InvalidMessageLength = 0x0065,

        /// <summary>
        /// Unsupported encapsulation protocol revision.
        /// </summary>
        UnsupportedEncapsulationProtocolRevision = 0x0069
    }
}
