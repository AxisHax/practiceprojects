//	<copyright file="CipGeneralStatusCode.cs"  company="Alliant Technologies">
//		Copyright © 2024 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for CipGeneralStatusCode.
//	</summary>
namespace CIP_EthernetIP_Library.EnumStructures
{
    /// <summary>
    /// Status codes that may be present in the General Status Code field of an Error Response message.
    /// </summary>
    public enum CipGeneralStatusCode : byte
    {
        /// <summary>Service was successfully performed by the object specified.</summary>
        Success = 0x00,

        /// <summary>A connection related service failed along the connection path.</summary>
        ConnectionFailure,

        /// <summary>Resources needed for the object to perform the requested service were unavailable.</summary>
        ResourceUnavailable,

        /// <summary>See status code <see cref="InvalidParameter"/>, which is the preferred value to use for this condition.</summary>
        InvalidParameterValue,

        /// <summary>The path segment identifier or the segment syntax was not understood by the processing node.</summary>
        PathSegmentError,

        /// <summary>The path is referencing an object class, instance or structure element that is not known or is not contained in the processing node.</summary>
        PathDestinationUnknown,

        /// <summary>Only part of the expected data was transferred.</summary>
        PartialTransfer,

        /// <summary>The messaging connection was lost.</summary>
        ConnectionLost,

        /// <summary>The requested service was not implemented or was not defined for this object class/instance.</summary>
        ServiceNotSupported,

        /// <summary>Invalid attribute data detected.</summary>
        InvalidAttributeValue,

        /// <summary>An attribute in the Get Attribute List or Set Attribute List response has a non-zero status.</summary>
        AttributeListError,

        /// <summary>The object is already in the mode/state being requested by the service.</summary>
        AlreadyInRequestedModeOrState,

        /// <summary>The object cannot perform the requested service in it's current mode/state.</summary>
        ObjectStateConflict,

        /// <summary>The requested instance of object to be created already exists.</summary>
        ObjectAlreadyExists,

        /// <summary>A request to modify a non-modifiable attribute was received.</summary>
        AttributeNotSettable,

        /// <summary>A permission/privilege check failed.</summary>
        PriviledgeViolation,

        /// <summary>The device's current mode/state prohibits the execution of the requested service.</summary>
        DeviceStateConflict,

        /// <summary>The data to be transmitted in the response buffer is larger than the allocated response buffer.</summary>
        ReplyDataTooLarge,

        /// <summary>The service specified an operation that is going to fragment a primitive data value, i.e. half a REAL data type.</summary>
        FragmentationOfPrimitiveValue,

        /// <summary>The service did not supply enough data to perform the specified operation.</summary>
        NotEnoughData,

        /// <summary>The attribute specified in the request is not supported.</summary>
        AttributeNotSupported,

        /// <summary>The service supplied more data than was expected.</summary>
        TooMuchData,

        /// <summary>The object specified does not exist.</summary>
        ObjectDoesNotExist,

        /// <summary>The fragmentation sequence for this service is not currently active for this data.</summary>
        ServiceFragmentationSequenceNotInProgress,

        /// <summary>The attribute data of this object was not saved prior to the requested service.</summary>
        NoStoredAttributeData,

        /// <summary>The attribute data of this object was not saved due to a failure during the attempt.</summary>
        StoreOperationFailure,

        /// <summary>The service request packet was too large for transmission on a network in the path to the destination. The routing device was forced to abort the service.</summary>
        RoutingFailure_RequestTooLarge,

        /// <summary>The service response packet was too large for transmission on a network in the path from the destination. The routing device was forced to abort the service.</summary>
        RoutingFailure_ResponsePacketTooLarge,

        /// <summary>The service did not supply an attribute in a list of attributes that was needed by the service to perform the requested behavior.</summary>
        MissingAttributeListEntryData,

        /// <summary>The service is returning the list of attributes supplied with status information for those attributes that were invalid.</summary>
        InvalidAttributeValueList,

        /// <summary>An embedded service resulted in an error.</summary>
        EmbeddedServiceError,

        /// <summary>A vendor specific error has been encountered. The additional code field of the error response defines the particular error encountered.</summary>
        VendorServiceError,

        /// <summary>
        /// A parameter associated with the request was invalid. 
        /// The code is used when a parameter does not meet the requirements of this specification and/or the requirements defined in an application object specification.
        /// </summary>
        InvalidParameter,

        /// <summary>An attempt was made to write to a write-once medium (e.g WORM drive, PROM) that has already been written, or to modify a value that cannot be changed once established.</summary>
        WriteOnceValueOrMediumAlreadyWritten,

        /// <summary>
        /// An invalid reply is received (e.g. reply service code does not match the request service code, or reply message is shorter than the minimum expected reply size.
        /// The status code can serve for other causes of invalid replies.
        /// </summary>
        InvalidReplyReceived,

        /// <summary>The key segment that was included as the first segment in the path does not match the destination module. The object specific status shall indicate which part of the key check failed.</summary>
        KeyFailureInPath = 0x25,

        /// <summary>The size of the path which was sent with the service request is either not large enough to allow the request to be routed to an object or too much routing data was included.</summary>
        PathSizeInvalid,

        /// <summary>An attempt was made to set an attribute that is not able to be set at this time.</summary>
        UnexpectedAttributeInList,

        /// <summary>The member ID specified in the request does not exist in the specified class/instance/attribute.</summary>
        InvalidMemberID,

        /// <summary>A request to modify a non-modifiable member was received.</summary>
        MemberNotSettable,

        /// <summary>This error code may only be reported by DeviceNet Group 2 Only servers with 4K or less code space and only in place of service not supported, attribute not supported, and attribute not settable.</summary>
        Group2OnlyServerGeneralFailure
    }
}
