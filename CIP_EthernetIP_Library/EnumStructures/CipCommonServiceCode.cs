//	<copyright file="CipCommonServiceCode.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//
//	File				$HeadURL$
//	Last Modified By	$Author$
//	Last Modified On	$Date$
//	Last Modified		$Revision$
//	<summary>
//		Class file for ServiceCode.
//	</summary>
namespace CIP_EthernetIP_Library.EnumStructures
{
    /// <summary>
    /// The codes and names of the CIP Common Services.
    /// </summary>
    public enum CipCommonServiceCode : byte
    {
        /// <summary>Returns the contents of all attributes of the class.</summary>
        GetAttributesAll = 0x01,

        /// <summary>Modifies the contents of the attributes of the class or object.</summary>
        SetAttributesAllRequest = 0x02,

        /// <summary>Returns the contents of the selected gettable attributes of the specified object class or instance.</summary>
        GetAttributeList = 0x03,

        /// <summary>Set the contents of selected attributes of the specified object class or instance.</summary>
        SetAttributeList = 0x04,

        /// <summary>Invokes the reset service of the specified class/object. Typically this would cause a transition to a default state/mode.</summary>
        Reset = 0x05,

        /// <summary>Invokes the start service of the specified class/object. Typically, this would place an object into a running state/mode.</summary>
        Start = 0x06,

        /// <summary>Invokes the stop service of the specified class/object. Typically this would place an object into a stopped or idle state/mode.</summary>
        Stop = 0x07,

        /// <summary>Results in the instantiation of a new object within the specified class.</summary>
        Create = 0x08,

        /// <summary>Deletes an object instance of the specified class.</summary>
        Delete = 0x09,

        /// <summary>Performs a set of services as an autonomous sequence.</summary>
        MultipleServicePacket = 0x0A,

        /// <summary>Causes attribute values whose use is pending to become actively used.</summary>
        ApplyAttributes = 0x0D,

        /// <summary>Return the contents of the specified attribute.</summary>
        GetAttributeSingle = 0x0E,

        /// <summary>Modifies an attribute value.</summary>
        SetAttributeSingle = 0x10,

        /// <summary>Causes the specified class to search for and return a list of instance IDs associated with existing object instances.
        /// Service supported at the class level only.
        /// </summary>
        FindNextObjectInstance = 0x11,

        /// <summary>Restores the contents of a class/object's attributes from a storage location accessible by the <see cref="Save"/> service.</summary>
        Restore = 0x15,

        /// <summary>Copies the contents of a class/object's attributes to a location accessible by the <see cref="Restore"/> service.</summary>
        Save = 0x16,

        /// <summary>This merely causes the receiving object to return a No Operation response without carrying out any internal action.</summary>
        Nop = 0x17,

        /// <summary>Returns member(s) information from within an attribute.</summary>
        GetMember = 0x18,

        /// <summary>Sets member(s) information in an attribute.</summary>
        SetMember = 0x19,

        /// <summary>Inserts member(s) into an attribute.</summary>
        InsertMember = 0x1A,

        /// <summary>Removes member(s) from an attribute.</summary>
        RemoveMember = 0x1B,

        /// <summary>Verify each member of a group is synchronized to the System Time.</summary>
        GroupSync = 0x1C
    }
}
